using UnityEngine;
using UnityEngine.Audio;

public class Gameplay : MonoBehaviour
{
    private Controller _playerController;
    private MouseClickInput _mouseClickInput;
    private HealSpawnerController _healSpawnerController;
    private AudioSettings _audioSettings;
    private AudioSettingsController _audioSettingsController;

    [SerializeField] private float _rayShootDistance = 100f;
    [SerializeField] private NavMeshCharacter _character;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private MouseClickInputView _mouseView;
    [SerializeField] private HealSpawner _healSpawner;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioSettingsView _audioSettingsView;

    private void Awake()
    {
        _mouseClickInput = new MouseClickInput(_rayShootDistance, _groundLayerMask);
        _mouseView.Initialize(_mouseClickInput);

        _healSpawnerController = new HealSpawnerController(_healSpawner);
        _healSpawnerController.Enable();

        _audioSettings = new AudioSettings(_audioMixer);
        _audioSettingsController = new AudioSettingsController(_audioSettings);
        _audioSettingsController.InitializeController();
        _audioSettingsController.Enable();
        _audioSettingsView.Initialize(_audioSettings);

        _playerController = new CompositeController(
            new PlayerNavMeshMovableController(_character, _mouseClickInput),
            new PlayerRotatableController(_character, _character));

        _playerController.Enable();
    }

    private void Update()
    {
        _playerController.Update(Time.deltaTime);
        _mouseClickInput.Update(Time.deltaTime);
        _healSpawnerController.Update(Time.deltaTime);
    }

}
