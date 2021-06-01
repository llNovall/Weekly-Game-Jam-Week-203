using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEnemyKillTracker : MonoBehaviour
{
    public static PlayerEnemyKillTracker Current;

    [SerializeField]
    private int _currentKillCount;

    private UnityAction<int> OnKillCountChanged;
    private void Awake()
    {
        Current = this;
    }

    public int GetCurrentKillCount() => _currentKillCount;
    public void AddKillCount(int points)
    {
        _currentKillCount += points;
        OnKillCountChanged?.Invoke(_currentKillCount);
    }

    public void ResetPlayerKillProgress()
    {
        _currentKillCount = 0;
        OnKillCountChanged?.Invoke(_currentKillCount);
    }

    #region Subscriptions
    public void SubscribeToOnKillCountChanged(UnityAction<int> callback) => HelperUtility.SubscribeTo(ref OnKillCountChanged, ref callback);
    public void UnsubscribeFromOnKillCountChangedh(UnityAction<int> callback) => HelperUtility.UnsubscribeFrom(ref OnKillCountChanged, ref callback);

    #endregion
}
