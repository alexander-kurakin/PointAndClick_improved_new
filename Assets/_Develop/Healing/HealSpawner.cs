using System.Collections;
using UnityEngine;

public class HealSpawner : MonoBehaviour
{
    private Coroutine _courotineObject;
    private bool _isSpawnerEnabled = false;

    [SerializeField] private Heal _prefabToSpawn;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _spawnRadius = 5f;
    [SerializeField] private float _originalYoffset = 50f;
    [SerializeField] private float _groundCheckDistance = 100f;
    
    public bool SpawnerEnabled => _isSpawnerEnabled;

    public void Update()
    {
        SpawnerStart();
    }

    private void SpawnerStart()
    {
        if (_isSpawnerEnabled && _courotineObject == null)
            _courotineObject = StartCoroutine(ProcessSpawn());
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

    public void ToggleSpawner()
    {
        _isSpawnerEnabled = !_isSpawnerEnabled;
    }
}
