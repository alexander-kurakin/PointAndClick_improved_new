using UnityEngine;

public class Gameplay : MonoBehaviour
{
    [SerializeField] private NavMeshCharacter _character;

    private Controller _playerController;
    private MouseClickInput _mouseClickInput;

    private void Awake()
    {
        _mouseClickInput = new MouseClickInput();

        _playerController = new CompositeController(
            new PlayerNavMeshMovableController(_character, _mouseClickInput), 
            new PlayerMousePointRotatableController(_character,_character));

        _playerController.Enable();
    }

    private void Update()
    {
        _playerController.Update(Time.deltaTime);
    }

}
