using UnityEngine;

public class MouseClickInput : IMouseClickInput
{
    private Vector3 _mouseHitPosition;
    private float _rayShootDistance;
    private int _groundLayerMask;
    private Ray _currentRay;

    public MouseClickInput(float rayShootDistance, int groundLayerMask)
    {
        _rayShootDistance = rayShootDistance;
        _groundLayerMask = groundLayerMask;
    }

    public bool MouseClickButtonPressed => Input.GetMouseButtonDown(0);

    public Vector3 MousePosition => _mouseHitPosition;

    public void ShootRay(float rayShootDistance, int groundLayerMask)
    {
        _currentRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_currentRay, out RaycastHit hitInfo, rayShootDistance, groundLayerMask))
            _mouseHitPosition = hitInfo.point;
    }

    public void Update(float deltaTime)
    {
        ShootRay(_rayShootDistance, _groundLayerMask);
    }
}
