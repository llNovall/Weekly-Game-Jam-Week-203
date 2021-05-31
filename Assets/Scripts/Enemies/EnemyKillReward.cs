using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillReward : MonoBehaviour
{
    [SerializeField]
    private EnemyHealth _enemyHealth;

    [SerializeField]
    private int _rewardAmount;

    private void Start()
    {
        _enemyHealth.SubscribeToOnDeath(EnemyHealth_OnDeath);
    }

    private void EnemyHealth_OnDeath(bool isDead)
    {
        if (isDead)
        {
            PlayerEnemyKillTracker.Current.AddKillCount(_rewardAmount);
        }
    }
}
