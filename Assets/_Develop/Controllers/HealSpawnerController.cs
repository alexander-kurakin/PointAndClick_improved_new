using UnityEngine;

public class HealSpawnerController : Controller
{
    private const KeyCode _spawnerKey = KeyCode.F;

    private HealSpawner _healSpawner;

    public HealSpawnerController(HealSpawner healSpawner)
    { 
        _healSpawner = healSpawner;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        if (Input.GetKeyDown(_spawnerKey))
            _healSpawner.ToggleSpawner();
    }
}
