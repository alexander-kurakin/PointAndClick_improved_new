using UnityEngine;

public class PlayerMousePointRotatableController : Controller
{
    private IDirectionalRotatable _rotatable;
    private INavMeshMovable _movable;

    public PlayerMousePointRotatableController(IDirectionalRotatable rotatable, INavMeshMovable movable)
    {
        _rotatable = rotatable;
        _movable = movable;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        _rotatable.SetRotationDirection(_movable.CurrentVelocity);
    }
}
