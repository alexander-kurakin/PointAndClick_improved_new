using UnityEngine;

public class HealView : MonoBehaviour
{
    [SerializeField] private float _yAnimationOffset = 0.5f;
    [SerializeField] private GameObject _healEffectPrefab;
    public void AnimateHeal()
    {
        Instantiate(_healEffectPrefab, transform.position + Vector3.up * _yAnimationOffset, Quaternion.identity, null);
    }
}
