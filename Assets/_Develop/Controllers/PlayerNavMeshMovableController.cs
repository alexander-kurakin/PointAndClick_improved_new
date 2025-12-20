using UnityEngine;
using UnityEngine.AI;

public class PlayerNavMeshMovableController : Controller
{
    private INavMeshMovable _movable;
    private IMouseClickInput _mouseClickInput;
    private Vector3 _mouseHitPosition = Vector3.zero;
    private NavMeshPath _pathToTarget = new NavMeshPath();

    public PlayerNavMeshMovableController(INavMeshMovable movable, IMouseClickInput mouseClickInput)
    {
        _movable = movable;
        _mouseClickInput = mouseClickInput;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        _mouseHitPosition = _movable.MouseHitPosition;

        if (_mouseClickInput.MouseClickButtonPressed) 
            _movable.SetDestination(_mouseHitPosition);

        if (_mouseHitPosition != Vector3.zero && _movable.CanMove)
        {
            if (_movable.TryGetPath(_mouseHitPosition, _pathToTarget))
            {
                _movable.ResumeMove();
                return;
            }
        }

        _movable.StopMove();
    }

}
