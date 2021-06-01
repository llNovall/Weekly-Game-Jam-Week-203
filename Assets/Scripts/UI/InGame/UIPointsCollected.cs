using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPointsCollected : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _txtPointsCollected;

    private void Start()
    {
        PlayerEnemyKillTracker.Current.SubscribeToOnKillCountChanged(PlayerEnemyKillTracker_OnKillCountChanged);
    }

    private void PlayerEnemyKillTracker_OnKillCountChanged(int points)
    {
        _txtPointsCollected.text = points.ToString();
    }
}
