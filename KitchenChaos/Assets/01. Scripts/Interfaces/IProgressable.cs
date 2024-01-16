using System;
using UnityEngine;

public interface IProgressable
{
    // <current, max, immediately>
    public event Action<float, float, bool> OnProgressChangedEvent;
    
}
