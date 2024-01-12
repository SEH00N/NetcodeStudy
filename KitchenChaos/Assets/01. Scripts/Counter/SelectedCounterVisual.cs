using System;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    private ClearCounter counter;
    private GameObject visual;

    private void Awake()
    {
        counter = transform.parent.GetComponent<ClearCounter>();
        visual = transform.Find("Visual")?.gameObject;
    }

	private void Start()
    {
        DEFINE.Player.OnSelectedCounterChanged += HandleCounterChanged;
    }

    private void HandleCounterChanged(ClearCounter counter)
    {
        visual.SetActive(this.counter == counter);
    }
}
