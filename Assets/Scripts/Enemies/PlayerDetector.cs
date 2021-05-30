using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField]
    private float _detectionRadius;

    [SerializeField]
    private SphereCollider _detectionCollider;

    private UnityAction<bool> OnPlayerDetected;

    private void Start()
    {
        _detectionCollider.radius = _detectionRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == PlayerTracker.Current.gameObject)
        {
            Debug.LogError("Player Detected");
            OnPlayerDetected?.Invoke(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerTracker.Current.gameObject)
        {
            Debug.LogError("Player Exited");
            OnPlayerDetected?.Invoke(false);
        }
    }

    #region Event Subscriptions
    public void SubscribeToOnPlayerDetected(UnityAction<bool> callback) => HelperUtility.SubscribeTo(ref OnPlayerDetected, ref callback);
    public void UnsubscribeFromOnPlayerDetected(UnityAction<bool> callback) => HelperUtility.UnsubscribeFrom(ref OnPlayerDetected, ref callback);

    #endregion
}
