using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.Events;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;

    private event UnityAction<float> OnPlayerMovementInputUpdated;
    private event UnityAction OnJump;
    private event UnityAction OnAbility1;
    private event UnityAction OnAbility2;

    private void Start()
    {
        _playerInput = gameObject.AddComponent<PlayerInput>();
        _playerInput.actions = Resources.Load<InputActionAsset>("Controls");
        _playerInput.notificationBehavior = PlayerNotifications.InvokeUnityEvents;
        _playerInput.defaultControlScheme = "Keyboard & Mouse";

        foreach (InputAction item in _playerInput.actions)
            item.Enable();

        InputAction ability1 = _playerInput.actions["Ability1"];
        ability1.performed += Ability1_performed;

        InputAction ability2 = _playerInput.actions["Ability2"];
        ability2.performed += Ability2_performed;

        InputAction jump = _playerInput.actions["Jump"];
        jump.performed += Jump_performed;

    }

    private void LateUpdate()
    {
        float inputVector = _playerInput.actions["SideMovement"].ReadValue<float>();
        if (inputVector != 0)
            OnPlayerMovementInputUpdated?.Invoke(inputVector);
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        OnJump?.Invoke();
    }

    private void Ability2_performed(InputAction.CallbackContext obj)
    {
        Debug.LogError($"{GetType().FullName} : Ability2 Activated");
        OnAbility2?.Invoke();
    }

    private void Ability1_performed(InputAction.CallbackContext obj)
    {
        Debug.LogError($"{GetType().FullName} : Ability1 Activated");
        OnAbility1?.Invoke();
    }

    #region

    public void SubscribeToOnPlayerMovementInputUpdated(UnityAction<float> callback) => HelperUtility.SubscribeTo(ref OnPlayerMovementInputUpdated, ref callback);
    public void UnsubscribeFromOnPlayerMovementInputUpdated(UnityAction<float> callback) => HelperUtility.UnsubscribeFrom(ref OnPlayerMovementInputUpdated, ref callback);

    public void SubscribeToOnJump(UnityAction callback) => HelperUtility.SubscribeTo(ref OnJump, ref callback);
    public void UnsubscribeFromOnJump(UnityAction callback) => HelperUtility.UnsubscribeFrom(ref OnJump, ref callback);

    public void SubscribeToOnAbility1(UnityAction callback) => HelperUtility.SubscribeTo(ref OnAbility1, ref callback);
    public void UnsubscribeFromOnAbility1(UnityAction callback) => HelperUtility.UnsubscribeFrom(ref OnAbility1, ref callback);

    public void SubscribeToOnAbility2(UnityAction callback) => HelperUtility.SubscribeTo(ref OnAbility2, ref callback);
    public void UnsubscribeFromOnAbility2(UnityAction callback) => HelperUtility.UnsubscribeFrom(ref OnAbility2, ref callback);





    #endregion
}
