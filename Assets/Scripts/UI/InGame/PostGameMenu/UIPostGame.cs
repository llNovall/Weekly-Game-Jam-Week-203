using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPostGame : MonoBehaviour
{
    [SerializeField]
    private GameObject _objPostGameMenu;

    [SerializeField]
    private TextMeshProUGUI _txtPoints;

    [SerializeField]
    private Button _btnMainMenu;

    private void Start()
    {
        PlayerTracker.PlayerHealth.SubscribeToOnHealthDepleted(PlayerHealth_OnHealthDepleted);

        TimeManager.Current.SubscribeToOnGameTimeOver(TimeManager_OnGameTimeOver);

        _btnMainMenu.onClick.AddListener(BtnMainMenu_OnClick);


    }

    private void BtnMainMenu_OnClick()
    {
        Time.timeScale = 1;
        GameStateManager.Current.ChangeGameState(GameState.MainMenu);
        SceneManager.LoadScene(0);
    }

    private void TimeManager_OnGameTimeOver()
    {
        Time.timeScale = 0;
        ShowMenu();
        UpdatePoints();
    }

    private void PlayerHealth_OnHealthDepleted()
    {
        Time.timeScale = 0;
        ShowMenu();
        UpdatePoints();
    }

    private void ShowMenu() => _objPostGameMenu.SetActive(true);

    private void UpdatePoints() => _txtPoints.text = PlayerEnemyKillTracker.Current.GetCurrentKillCount().ToString();
}
