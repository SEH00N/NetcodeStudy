using System;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    private StoveCounter counter;

	private GameObject stoveObject;
	private GameObject particleObject;

    private void Awake()
    {
        counter = transform.parent.GetComponent<StoveCounter>();

        stoveObject = transform.Find("StoveGlow").gameObject;
        particleObject = transform.Find("Particle").gameObject;   
    }

    private void Start()
    {
        counter.OnStateChangedEvent += HandleState;
    }

    private void HandleState(StoveCounter.State state)
    {
        Display(state == StoveCounter.State.Frying || state == StoveCounter.State.Fried);
    }

    public void Display(bool value)
    {
        stoveObject.SetActive(value);
        particleObject.SetActive(value);
    }
}
