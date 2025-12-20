using UnityEngine;

public class MouseRayScanner : IMouseRayShooter
{
    private Vector3 _mouseHitPosition;
    private float _rayShootDistance;
    private int _groundLayerMask;

    public MouseRayScanner(float rayShootDistance, int groundLayerMask)
    {
        _rayShootDistance = rayShootDistance;
        _groundLayerMask = groundLayerMask;
    }

    public Vector3 MouseHitPosition => _mouseHitPosition;

    public void ShootRay(float rayShootDistance, int groundLayerMask)
    {
        Ray mousePointRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mousePointRay, out RaycastHit hitInfo, rayShootDistance, groundLayerMask))
            _mouseHitPosition = hitInfo.point;
    }

    public void Update(float deltaTime)
    { 
        ShootRay(_rayShootDistance, _groundLayerMask);
    }
}
