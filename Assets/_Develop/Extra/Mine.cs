using System.Collections;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private float _mineTriggerTime = 3f;
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private int _explosionDamage = 35;

    private bool _isTriggered = false;

    private MineVisual _visual;

    private void Awake()
    {
        _visual = GetComponent<MineVisual>();
    }

    private void Update()
    {
    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isTriggered == false && other.TryGetComponent<IDamageable>(out IDamageable iDamageable))
            TriggerMine();
    }

    private void DetonateMine()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider target in targets)
        { 
            if (target.TryGetComponent<IDamageable>(out IDamageable iDamageable))
                iDamageable.TakeDamage(_explosionDamage);
        }

        _visual.AnimateDetonate();
        Destroy(gameObject);
    }

    private IEnumerator TriggerMineCouroutine()
    {
        yield return new WaitForSeconds(_mineTriggerTime);
        DetonateMine();
    }

    public void TriggerMine()
    {
        _isTriggered = true;
        StartCoroutine(TriggerMineCouroutine());
    }

    public bool IsTriggered() => _isTriggered;
    public float GetExplosionRadius() => _explosionRadius;
}
