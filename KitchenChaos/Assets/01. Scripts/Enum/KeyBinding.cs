using UnityEngine;
using static Controls;

public partial class InputReaderSO : ScriptableObject, IPlayerActions
{
	public enum KeyBinding
    {
        Move,
        Interact,
        InteractAlternate
    }
}
