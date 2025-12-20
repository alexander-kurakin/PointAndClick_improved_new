using UnityEngine;

public interface IMouseRayShooter
{
    Vector3 MouseHitPosition { get; }
    void ShootRay(float rayShootDistance, int groundLayerMask);
}
