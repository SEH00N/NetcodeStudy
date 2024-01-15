using System;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    private BaseCounter counter;
    private GameObject visual;

    private void Awake()
    {
        counter = transform.parent.GetComponent<BaseCounter>();
        visual = transform.Find("Visual")?.gameObject;
    }

	private void Start()
    {
        DEFINE.Player.OnSelectedCounterChanged += HandleCounterChanged;
    }

    private void HandleCounterChanged(BaseCounter counter)
    {
        visual.SetActive(this.counter == counter);
    }
}
