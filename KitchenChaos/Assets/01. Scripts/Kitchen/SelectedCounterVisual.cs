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
        if(DEFINE.LocalPlayer != null)
        {
            DEFINE.LocalPlayer.OnSelectedCounterChanged -= HandleCounterChanged;
            DEFINE.LocalPlayer.OnSelectedCounterChanged += HandleCounterChanged;
        }
        else
        {
            Player.OnAnyPlayerSpawned += HandlePlayerSpawned;
        }
    }

    private void HandlePlayerSpawned()
    {
        if(DEFINE.LocalPlayer == null)
            return;

        DEFINE.LocalPlayer.OnSelectedCounterChanged -= HandleCounterChanged;
        DEFINE.LocalPlayer.OnSelectedCounterChanged += HandleCounterChanged;
    }

    private void HandleCounterChanged(BaseCounter counter)
    {
        visual.SetActive(this.counter == counter);
    }
}
