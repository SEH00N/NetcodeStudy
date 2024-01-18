using System;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    private StoveCounter counter;
	private AudioSource audioSource;

    private void Awake()
    {
        counter = transform.parent.GetComponent<StoveCounter>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        counter.OnStateChangedEvent += HandleState;
    }

    private void HandleState(StoveCounter.State state)
    {
        bool play = (state == StoveCounter.State.Frying) || (state == StoveCounter.State.Fried);
        if(play)
            audioSource.Play();
        else
            audioSource.Pause();
    }
}
