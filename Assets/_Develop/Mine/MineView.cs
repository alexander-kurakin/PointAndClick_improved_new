using UnityEngine;

public class MineView : MonoBehaviour
{
    [SerializeField] private GameObject _destroyEffectPrefab;
    [SerializeField] private AudioClip _tickingSound;
    [SerializeField] private Material _triggeredMaterial;

    private Mine _mine;
    private AudioSource _mineAudio;
    private MeshRenderer _meshRenderer;

    private Color _currentGizmosColor;
    private Color _transparentRed = new Color(1f, 0f, 0f, 0.25f);
    private Color _transparentGreen = new Color(0f, 1f, 0f, 0.25f);

    private void Awake()
    {
        _mine = GetComponent<Mine>();
        _mineAudio = GetComponent<AudioSource>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _currentGizmosColor = _transparentGreen;

    }
    private void Update()
    {
        if (_mine.IsTriggered())
        {
            if (_mineAudio.isPlaying == false)
                _mineAudio.PlayOneShot(_tickingSound);

            _meshRenderer.material = _triggeredMaterial;

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
