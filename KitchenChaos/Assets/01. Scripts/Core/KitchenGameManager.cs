using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class KitchenGameManager : NetworkBehaviour
{
    public static KitchenGameManager Instance = null;

    [SerializeField] InputReaderSO inputReader;
    [SerializeField] Transform playerPrefab;

    [Space(10f)]
    // [SerializeField] float startingTime = 1f;
    [SerializeField] float countdownTime = 1f;
    [SerializeField] float playingTime = 1f;

    public event Action<State> OnStateChangedEvent;
    public event Action<bool> OnLocalGamePausedEvent;
    public event Action<bool> OnGlobalGamePausedEvent;
    public event Action OnLocalPlayerReadyChangedEvent;

    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    public State GameState => state.Value;

    private bool isLocalPlayerReady = false;
    public bool IsLocalPlayerReady => isLocalPlayerReady;

    private NetworkVariable<float> timer = new NetworkVariable<float>(0f);
    public float Timer => timer.Value;
    public float GamePlayingTimeNormalized => 1 - (timer.Value / playingTime);

    public bool GamePlaying => (state.Value == State.GamePlaying);
    public bool IsLocalGamePaused {get; private set;} =false;
    private NetworkVariable<bool> isGamePaused = new NetworkVariable<bool>(false);
    private bool autoCheckGamePausedState = false;

    private Dictionary<ulong, bool> playerReadies;
    private Dictionary<ulong, bool> playerPauses;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        state.OnValueChanged += HandleStateValueChanged;
        isGamePaused.OnValueChanged += HandleGamePausedValueChanged;

        if(IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += HandleLoadEventCompleted;
        }
    }

    private void Awake()
    {
        Instance = this;
        playerReadies = new Dictionary<ulong, bool>();
        playerPauses = new Dictionary<ulong, bool>();
    }

    private void Start()
    {
        inputReader.OnPauseEvent += TogglePauseGame;
        inputReader.OnInteractEvent += HandleInteract;
    }

    private void Update()
    {
        if(IsServer == false)
            return;

        switch (state.Value)
        {
            case State.WaitingToStart:
                // TimeOut(State.CountdownToStart, countdownTime);
                break;
            case State.CountdownToStart:
                TimeOut(State.GamePlaying, playingTime);
                break;
            case State.GamePlaying:
                TimeOut(State.GameOver, 0f);
                break;
            case State.GameOver:
                break;
        }
    }

    private void LateUpdate()
    {
        if(autoCheckGamePausedState == false)
            return;

        autoCheckGamePausedState = false;
        CheckGamePausedState();
    }

    private void HandleInteract()
    {
        if(state.Value == State.WaitingToStart)
        {
            isLocalPlayerReady = true;
            OnLocalPlayerReadyChangedEvent?.Invoke();
            SetPlayerReadyServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadies[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach(ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if((playerReadies.ContainsKey(clientID) == false) || (playerReadies[clientID] == false))
            {
                allClientsReady = false;
                break;
            }
        }

        if(allClientsReady)
        {
            timer.Value = countdownTime;
            ChangeState(State.CountdownToStart);
        }
    }

    private void TimeOut(State nextState, float cooldown)
    {
        timer.Value -= Time.deltaTime;
        if (timer.Value <= 0f)
        {
            ChangeState(nextState);
            timer.Value = cooldown;
        }
    }

    private void ChangeState(State nextState)
    {
        state.Value = nextState;
    }

    private void HandleStateValueChanged(State previousValue, State newValue)
    {
        OnStateChangedEvent?.Invoke(newValue);
    }

    public void TogglePauseGame()
    {
        IsLocalGamePaused = !IsLocalGamePaused;
        PauseGameServerRpc(IsLocalGamePaused);
        OnLocalGamePausedEvent?.Invoke(IsLocalGamePaused);
    }

    [ServerRpc(RequireOwnership = false)]
    private void PauseGameServerRpc(bool isPaused, ServerRpcParams serverRpcParams = default)
    {
        playerPauses[serverRpcParams.Receive.SenderClientId] = isPaused;
        CheckGamePausedState();
    }

    private void CheckGamePausedState()
    {
        foreach(ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if(playerPauses.ContainsKey(clientID) && playerPauses[clientID])
            {
                isGamePaused.Value = true;
                return;
            }
        }
    
        // all players are unpaused
        isGamePaused.Value = false;
    }

    private void HandleGamePausedValueChanged(bool previousValue, bool newValue)
    {
        Time.timeScale = isGamePaused.Value ? 0f : 1f;
        OnGlobalGamePausedEvent?.Invoke(isGamePaused.Value);
    }

    private void HandleClientDisconnect(ulong clientId)
    {
        autoCheckGamePausedState = true;
    }

    private void HandleLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach(ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerInstance = Instantiate(playerPrefab);
            playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID, true);
        }
    }
}
