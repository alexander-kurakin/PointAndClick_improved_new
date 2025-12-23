using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickInputView : MonoBehaviour
{
    private MouseClickInput _mouseClickInput;

    public void Initialize(MouseClickInput input)
    {
        _mouseClickInput = input;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_mouseClickInput.MousePosition, 0.1f);
        }
    }
}
