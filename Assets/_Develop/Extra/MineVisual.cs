using UnityEngine;

public class MineVisual : MonoBehaviour
{
    [SerializeField] private GameObject _destroyEffectPrefab;

    private Mine _mine;

    private Color _currentGizmosColor;
    private Color _transparentRed = new Color(1f, 0f, 0f, 0.25f);
    private Color _transparentGreen = new Color(0f, 1f, 0f, 0.25f);

    private void Awake()
    {
        _mine = GetComponent<Mine>();
        _currentGizmosColor = _transparentGreen;
    }
    private void Update()
    {
        if (_mine.IsTriggered())
        {
            _currentGizmosColor = _transparentRed;
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = _currentGizmosColor;
            Gizmos.DrawSphere(transform.position, _mine.GetExplosionRadius());
        }
    }

    public void AnimateDetonate()
    {
        Instantiate(_destroyEffectPrefab, transform.position, Quaternion.identity, null);
    }
}
