using UnityEngine;

public class MouseClickInput : IMouseClickInput
{
    public bool MouseClickButtonPressed => Input.GetMouseButtonDown(0);
}
