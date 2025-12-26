using TMPro;
using UnityEngine;

public class HealSpawnerView : MonoBehaviour
{
    [SerializeField] private TMP_Text _debugText;
    [SerializeField] private HealSpawner _healSpawner;

    private void Update()
    {
        ShowSpawnerState();
    }

    private void ShowSpawnerState()
    {
        _debugText.text = "Health Pack Spawn Enabled=" + _healSpawner.SpawnerEnabled;
    }
}
