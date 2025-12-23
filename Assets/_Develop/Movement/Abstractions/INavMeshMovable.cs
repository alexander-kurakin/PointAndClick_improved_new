using UnityEngine;
using UnityEngine.AI;

public interface INavMeshMovable
{
    Vector3 CurrentVelocity { get; }
    bool CanMove { get; }
    void SetDestination(Vector3 position);
    bool TryGetPath(Vector3 targetPosition, NavMeshPath pathToTarget);
    void StopMove();
    void ResumeMove();

    bool InJumpProcess { get; }
    bool IsOnNavMeshLink(out OffMeshLinkData offMeshLinkData);
    void Jump(OffMeshLinkData offMeshLinkData);

}
