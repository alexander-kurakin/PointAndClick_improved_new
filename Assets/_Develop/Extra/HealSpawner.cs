using System.Collections;
using TMPro;
using UnityEngine;

public class HealSpawner : MonoBehaviour
{
    private const KeyCode _spawnerKey = KeyCode.F;

    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _spawnRadius = 5f;
    [SerializeField] private float _originalYoffset = 50f;
    [SerializeField] private float _groundCheckDistance = 100f;

    [SerializeField] private TMP_Text _debugText;
    [SerializeField] private GameObject _prefabToSpawn;
    [SerializeField] private LayerMask _groundLayer;

    private Coroutine _courotineObject;
    private bool _isSpawnerEnabled = false;

    public void Update()
    {
        _debugText.text = "Health Pack Spawn Enabled=" + _isSpawnerEnabled;

        if (Input.GetKeyDown(_spawnerKey))
        {
            _isSpawnerEnabled = !_isSpawnerEnabled;

            if (_isSpawnerEnabled && _courotineObject == null)
                _courotineObject = StartCoroutine(ProcessSpawn());
        }
    }

    private IEnumerator ProcessSpawn()
    {
        while (_isSpawnerEnabled)
        {
            RandomSpawn();

            yield return new WaitForSeconds(_spawnInterval);
        }

        _courotineObject = null;
    }

    private void RandomSpawn()
    {
        Vector3 randomSpawnPoint = new Vector3(
            Random.Range(transform.position.x - _spawnRadius, transform.position.x + _spawnRadius),
            transform.position.y + _originalYoffset, 
            Random.Range(transform.position.z - _spawnRadius, transform.position.z + _spawnRadius));

        if (Physics.Raycast(randomSpawnPoint, Vector3.down, out RaycastHit hitInfo, _groundCheckDistance, _groundLayer))
            Instantiate(_prefabToSpawn, hitInfo.point, Quaternion.identity);
    }
}
