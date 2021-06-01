using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Current;

    [SerializeField]
    private float _maxTime, _remainingTime;

    [SerializeField]
    private bool _isGameFinished;

    private event UnityAction<float> OnRemainingTimeChanged;
    private event UnityAction OnGameTimeOver;

    private void Awake()
    {
        Current = this;
    }
    private void Start()
    {
        _remainingTime = _maxTime;
    }
    private void Update()
    {
        if (!_isGameFinished)
        {
            _remainingTime = Mathf.Max(_remainingTime - Time.deltaTime, 0);
            OnRemainingTimeChanged?.Invoke(_remainingTime);
            if(_remainingTime == 0)
            {
                _isGameFinished = true;
                OnGameTimeOver?.Invoke();
            }
        }
        
    }

    public float GetRemainingTime() => _remainingTime;
    #region Event Subscriptions
    public void SubscribeToOnRemainingTimeChanged(UnityAction<float> callback) => HelperUtility.SubscribeTo(ref OnRemainingTimeChanged, ref callback);
    public void UnsubscribeFromOnRemainingTimeChanged(UnityAction<float> callback) => HelperUtility.UnsubscribeFrom(ref OnRemainingTimeChanged, ref callback);

    public void SubscribeToOnGameTimeOver(UnityAction callback) => HelperUtility.SubscribeTo(ref OnGameTimeOver, ref callback);
    public void UnsubscribeFromOnGameTimeOver(UnityAction callback) => HelperUtility.UnsubscribeFrom(ref OnGameTimeOver, ref callback);
    #endregion
}
