using UnityEngine;

public class Health
{
    private int _currentHealth;
    private int _maxHealth;

    public Health( int maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;
    }

    public void IncreaseHealth(int increaseAmount)
    {
        _currentHealth += increaseAmount;

        if (_currentHealth >= _maxHealth)
            _currentHealth = _maxHealth;
    }

    public void DecreaseHealth(int decreaseAmount)
    {
        _currentHealth -= decreaseAmount;

        if (_currentHealth <= 0)
            _currentHealth = 0;
    }

    public int CurrentHealth => _currentHealth;
    public bool HealthIsDrained => _currentHealth <= 0;
}
