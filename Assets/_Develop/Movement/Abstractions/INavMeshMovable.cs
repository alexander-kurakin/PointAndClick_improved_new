using UnityEngine;
using UnityEngine.AI;

public interface INavMeshMovable
{
    Vector3 CurrentVelocity { get; }
    Vector3 MouseHitPosition { get; }
    bool CanMove { get; }
    void SetDestination(Vector3 position);
    bool TryGetPath(Vector3 targetPosition, NavMeshPath pathToTarget);
    void StopMove();
    void ResumeMove();

}
