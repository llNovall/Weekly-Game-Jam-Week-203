using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    [SerializeField]
    private Slider _sldHealth;

    private void Start()
    {

        HealthData healthData = PlayerTracker.PlayerHealth.GetHealthData();

        _sldHealth.minValue = 0;
        _sldHealth.maxValue = healthData.MaxHealth;
        _sldHealth.value = healthData.Health;

        PlayerTracker.PlayerHealth.SubscribeToOnHealthUpdated(PlayerHealth_OnHealthUpdated);
    }

    private void PlayerHealth_OnHealthUpdated(float health) => _sldHealth.value = health;
}
