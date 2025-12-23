using UnityEngine;

public class Gameplay : MonoBehaviour
{
    [SerializeField] private NavMeshCharacter _character;
    [SerializeField] private float _rayShootDistance = 100f;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private MouseClickInputView _mouseView;

    private Controller _playerController;
    private MouseClickInput _mouseClickInput;

    private void Awake()
    {
        _mouseClickInput = new MouseClickInput(_rayShootDistance, _groundLayerMask);
        _mouseView.Initialize(_mouseClickInput);

        _playerController = new CompositeController(
            new PlayerNavMeshMovableController(_character, _mouseClickInput),
            new PlayerRotatableController(_character, _character));

        _playerController.Enable();
    }

    private void Update()
    {
        _playerController.Update(Time.deltaTime);
        _mouseClickInput.Update(Time.deltaTime);
    }

}
