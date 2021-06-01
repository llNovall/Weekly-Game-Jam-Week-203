using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITimeRemaining : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _txtTimePassed;

    private void Start()
    {
        TimeManager.Current.SubscribeToOnRemainingTimeChanged(TimeManager_OnRemainingTimeChanged);
    }

    private void TimeManager_OnRemainingTimeChanged(float remainingTime)
    {
        _txtTimePassed.text = $"{(int) remainingTime / 60} : {Mathf.FloorToInt(remainingTime - (int) remainingTime / 60 * 60)} ";
    }
}
