using System;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    private ContainerCounter counter;
	private Animator animator;

    private readonly int OpenCloseHash = Animator.StringToHash("OpenClose");

    private void Awake()
    {
        counter = transform.parent.GetComponent<ContainerCounter>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        counter.OnPlayerGraabedEvent += HandleGrabbedEvent;
    }

    private void HandleGrabbedEvent()
    {
        animator.SetTrigger(OpenCloseHash);
    }
}
