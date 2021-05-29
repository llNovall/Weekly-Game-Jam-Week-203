using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct HealthData
{
    public float Health;
    public float MaxHealth;

    public HealthData(float health, float maxHealth)
    {
        Health = health;
        MaxHealth = maxHealth;
    }
}
