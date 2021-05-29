using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private HealthData _healthData;

    private event UnityAction<float> OnHealthUpdated;
    private event UnityAction OnHealthDepleted;
    public void ModifyHealth(float health)
    {
        _healthData.Health = Mathf.Max(_healthData.Health - health, 0);
        OnHealthUpdated?.Invoke(_healthData.Health);

        if (_healthData.Health == 0)
            OnHealthDepleted?.Invoke();
    }

    public HealthData GetHealthData() => _healthData;

    #region Event Subscriptions
    public void SubscribeToOnHealthUpdated(UnityAction<float> callback) => HelperUtility.SubscribeTo(ref OnHealthUpdated, ref callback);
    public void UnsubscribeFromOnHealthUpdated(UnityAction<float> callback) => HelperUtility.UnsubscribeFrom(ref OnHealthUpdated, ref callback);
    public void SubscribeToOnHealthDepleted(UnityAction callback) => HelperUtility.SubscribeTo(ref OnHealthDepleted, ref callback);
    public void UnsubscribeFromOnHealthDepleted(UnityAction callback) => HelperUtility.UnsubscribeFrom(ref OnHealthDepleted, ref callback);

    #endregion
}
