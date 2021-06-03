using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAbility : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _txtCooldown, _txtKey;

    [SerializeField]
    private Image _imgCooldown, _imgAbility;

    [SerializeField]
    private Ability _ability;
    public void Initialize(Ability ability)
    {
        _ability = ability;

        AbilityData abilityData = ability.GetAbilityData();

        _txtCooldown.text = "0";
        _imgCooldown.gameObject.SetActive(false);
        ability.SubscribeToOnCooldownUpdated(Ability_OnCooldownUpdated);

        _txtKey.text = abilityData.ActivationKey;

        _imgAbility.sprite = Resources.Load<Sprite>(abilityData.SpriteLocation);
    }

    private void Ability_OnCooldownUpdated(float cooldown)
    {
        _imgCooldown.gameObject.SetActive(cooldown > 0);

        AbilityData abilityData = _ability.GetAbilityData();

        _imgCooldown.fillAmount = abilityData.Cooldown / abilityData.CooldownRequired;

        _txtCooldown.text = Math.Round(cooldown, 1).ToString();

    }
}
