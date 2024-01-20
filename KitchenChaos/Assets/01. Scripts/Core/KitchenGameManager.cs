using System;
using UnityEngine;

public partial class KitchenGameManager : MonoBehaviour
{
    public static KitchenGameManager Instance = null;

    [SerializeField] InputReaderSO inputReader;

    [Space(10f)]
    [SerializeField] float startingTime = 1f;
    [SerializeField] float countdownTime = 1f;
    [SerializeField] float playingTime = 1f;

    public event Action<State> OnStateChangedEvent;
    public event Action<bool> OnGamePausedEvent;

    private State state;
    public State GameState => state;

    private float timer = 0f;
    public float Timer => timer;
    public float GamePlayingTimeNormalized => 1 - (timer / playingTime);

    public bool GamePlaying => (state == State.GamePlaying);
    public bool GamePaused {get; private set;} =false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // state = State.WaitingToStart;
        // timer = startingTime;
        inputReader.OnPauseEvent += TogglePauseGame;

        ChangeState(State.CountdownToStart);
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                TimeOut(State.CountdownToStart, countdownTime);
                break;
            case State.CountdownToStart:
                TimeOut(State.GamePlaying, playingTime);
                break;
            case State.GamePlaying:
                TimeOut(State.GameOver, 0f);
                break;
            case State.GameOver:
                HandleGameOver();
                break;
        }
    }

    private void HandleGameOver()
    {
        
    }

    private void TimeOut(State nextState, float cooldown)
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ChangeState(nextState);
            timer = cooldown;
        }
    }

    private void ChangeState(State nextState)
    {
        state = nextState;
        OnStateChangedEvent?.Invoke(state);
    }
        
    public void TogglePauseGame()
    {
        GamePaused = !GamePaused;
        Time.timeScale = GamePaused ? 0f : 1f;
        OnGamePausedEvent?.Invoke(GamePaused);
    }
}
