using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private float _maxHealth;

    [SerializeField]
    private float _currentHealth;

    private event UnityAction<bool> OnDeath;

    public void Initialize(EnemyHealthData EnemyHealthData)
    {
        _maxHealth = EnemyHealthData.MaxHealth;
        _currentHealth = _maxHealth;

        OnDeath?.Invoke(false);
    }

    /// <summary>
    /// Use this method to reduce the health
    /// </summary>
    /// <param name="healthToReduce"></param>
    public void ReduceHealth(float healthToReduce)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - healthToReduce, 0, _maxHealth);

        if (_currentHealth == 0)
        {
            //Probably a dirty way to get a kill counter. I will change this when I get time
            PlayerEnemyKillTracker.Current.AddKillCount(1);
            if (PlayerEnemyKillTracker.Current)
                OnDeath?.Invoke(true);
        }
    }

    #region Subscriptions
    public void SubscribeToOnDeath(UnityAction<bool> callback) => HelperUtility.SubscribeTo(ref OnDeath, ref callback);
    public void UnsubscribeFromOnDeath(UnityAction<bool> callback) => HelperUtility.UnsubscribeFrom(ref OnDeath, ref callback);

    #endregion
}
