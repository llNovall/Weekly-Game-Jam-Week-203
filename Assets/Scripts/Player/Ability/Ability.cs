using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Ability : MonoBehaviour
{
    [SerializeField]
    protected AbilityData _abilityData;

    protected event UnityAction<bool> OnAbilityActivated;
    protected event UnityAction<float> OnCooldownUpdated;

    private void Update()
    {
        if (!_abilityData.IsAbilityActivated)
            ReduceCooldown();
    }

    public virtual void ActivateAbility()
    {
        if (_abilityData.Cooldown <= 0 && !_abilityData.IsAbilityActivated)
        {
            PrePerform();
        }
    }

    public AbilityData GetAbilityData() => _abilityData;
    protected virtual void PrePerform()
    {
        _abilityData.Cooldown = _abilityData.CooldownRequired;
        _abilityData.IsAbilityActivated = true;
        OnAbilityActivated_RaiseEvent(true);
    }
    protected virtual void PostPerform()
    {
        OnAbilityActivated_RaiseEvent(false);
    }

    protected void OnAbilityActivated_RaiseEvent(bool isAbilityActivated)
    {
        _abilityData.IsAbilityActivated = isAbilityActivated;
        OnAbilityActivated?.Invoke(_abilityData.IsAbilityActivated);
    }
    protected void ReduceCooldown()
    {
        _abilityData.Cooldown = Mathf.Max(_abilityData.Cooldown - Time.deltaTime, 0);
        OnCooldownUpdated?.Invoke(_abilityData.Cooldown);
    }
    protected bool IsAbilityActivatable() => _abilityData.Cooldown <= 0;

    #region Event Subscriptions
    public void SubscribeToOnAbilityActivated(UnityAction<bool> callback) => HelperUtility.SubscribeTo(ref OnAbilityActivated, ref callback);
    public void UnsubscribeFromOnAbilityActivated(UnityAction<bool> callback) => HelperUtility.UnsubscribeFrom(ref OnAbilityActivated, ref callback);

    public void SubscribeToOnCooldownUpdated(UnityAction<float> callback) => HelperUtility.SubscribeTo(ref OnCooldownUpdated, ref callback);
    public void UnsubscribeFromOnCooldownUpdated(UnityAction<float> callback) => HelperUtility.UnsubscribeFrom(ref OnCooldownUpdated, ref callback);

    #endregion
}
