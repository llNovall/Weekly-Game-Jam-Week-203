using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _objInGameMenu;

    [SerializeField]
    private Button _btnResume, _btnGoToMainMenu;

    [SerializeField]
    private PlayerInput _playerInput;
    private void Start()
    {
        _playerInput = gameObject.AddComponent<PlayerInput>();
        _playerInput.actions = Resources.Load<InputActionAsset>("Controls");
        _playerInput.notificationBehavior = PlayerNotifications.InvokeUnityEvents;
        _playerInput.defaultControlScheme = "Keyboard & Mouse";
        _playerInput.SwitchCurrentActionMap("UI");

        foreach (InputAction item in _playerInput.actions)
            item.Enable();

        InputAction pause = _playerInput.actions["Pause"];
        pause.performed += Pause_performed;

        _btnResume.onClick.AddListener(BtnResume_OnPress);
        _btnGoToMainMenu.onClick.AddListener(BtnGoToMainMenu);
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        _objInGameMenu.SetActive(true);
        Time.timeScale = 0;
    }

    private void BtnResume_OnPress()
    {
        Time.timeScale = 1;
        _objInGameMenu.SetActive(false);
    }

    private void BtnGoToMainMenu()
    {
        Time.timeScale = 1;
        GameStateManager.Current.ChangeGameState(GameState.MainMenu);
        SceneManager.LoadScene(0);
    }
}
