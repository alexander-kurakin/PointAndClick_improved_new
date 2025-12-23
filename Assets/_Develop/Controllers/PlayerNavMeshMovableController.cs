using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

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
        if (_movable.IsOnNavMeshLink(out OffMeshLinkData offMeshLinkData))
        {
            if (_movable.InJumpProcess == false)
                _movable.Jump(offMeshLinkData);

            return;
        }

        _mouseHitPosition = _movable.MouseHitPosition;

        if (_mouseClickInput.MouseClickButtonPressed && EventSystem.current.IsPointerOverGameObject() == false) 
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
