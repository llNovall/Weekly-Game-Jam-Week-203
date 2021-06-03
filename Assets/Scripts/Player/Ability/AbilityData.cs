using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AbilityData
{
    public bool IsAbilityActivated;

    public float CooldownRequired, Cooldown;

    public string ActivationKey;

    public string SpriteLocation;

    public AbilityData(bool isAbilityActivated, float cooldownRequired, float cooldown, string activationKey, string spriteLocation)
    {
        IsAbilityActivated = isAbilityActivated;
        CooldownRequired = cooldownRequired;
        Cooldown = cooldown;
        ActivationKey = activationKey;
        SpriteLocation = spriteLocation;
    }
}
