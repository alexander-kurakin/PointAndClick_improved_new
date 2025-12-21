using UnityEngine;

public class Heal : MonoBehaviour
{
    [SerializeField] private int _healAmount;

    private HealVisual _visual;

    private void Awake()
    {
        _visual = GetComponent<HealVisual>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IHealable>(out IHealable iHealable))
        {
            iHealable.Heal(_healAmount);
            PlayEffectAndDestroy();
        }
    }

    private void PlayEffectAndDestroy()
    {
        _visual.AnimateHeal();
        Destroy(gameObject);
    }

}
