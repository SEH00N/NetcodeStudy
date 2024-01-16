using System;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private CuttingCounter counter;
	private Animator animator;

    private readonly int CutHash = Animator.StringToHash("Cut");

    private void Awake()
    {
        counter = transform.parent.GetComponent<CuttingCounter>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        counter.OnProgressChangedEvent += HandleProgress;
    }

    private void HandleProgress(float cur, float max, bool imm)
    {
        if(imm)
            return;

        animator.SetTrigger(CutHash);
    }
}
