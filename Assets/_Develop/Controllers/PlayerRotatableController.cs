using UnityEngine;
using UnityEngine.AI;

public class PlayerRotatableController : Controller
{
    private IDirectionalRotatable _rotatable;
    private INavMeshMovable _movable;

    public PlayerRotatableController(IDirectionalRotatable rotatable, INavMeshMovable movable)
    {
        _rotatable = rotatable;
        _movable = movable;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        if (_movable.IsOnNavMeshLink(out OffMeshLinkData offMeshLinkData))
        {
            _rotatable.SetRotationDirection(offMeshLinkData.endPos - offMeshLinkData.startPos);
            return;
        }

        _rotatable.SetRotationDirection(_movable.CurrentVelocity);
    }
}
