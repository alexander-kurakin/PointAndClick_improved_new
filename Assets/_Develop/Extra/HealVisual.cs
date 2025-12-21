using UnityEngine;

public class HealVisual : MonoBehaviour
{
    [SerializeField] private GameObject _healEffectPrefab;
    public void AnimateHeal()
    {
        Instantiate(_healEffectPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity, null);
    }
}
