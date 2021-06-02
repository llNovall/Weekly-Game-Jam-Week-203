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

    private event UnityAction<Vector2> OnPlayerMovementInputUpdated;
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
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = _playerInput.actions["Movement"].ReadValue<Vector2>();
        if (inputVector != Vector2.zero)
            OnPlayerMovementInputUpdated?.Invoke(inputVector);

    }
    private void Ability2_performed(InputAction.CallbackContext obj)
    {
        Debug.LogError($"{GetType().FullName} : Ability2 Activated");
        OnAbility2?.Invoke();
    }

    private void Ability1_performed(InputAction.CallbackContext obj)
    {
        //Debug.LogError($"{GetType().FullName} : Ability1 Activated");
        OnAbility1?.Invoke();
    }

    #region

    public void SubscribeToOnPlayerMovementInputUpdated(UnityAction<Vector2> callback) => HelperUtility.SubscribeTo(ref OnPlayerMovementInputUpdated, ref callback);
    public void UnsubscribeFromOnPlayerMovementInputUpdated(UnityAction<Vector2> callback) => HelperUtility.UnsubscribeFrom(ref OnPlayerMovementInputUpdated, ref callback);
    public void SubscribeToOnAbility1(UnityAction callback) => HelperUtility.SubscribeTo(ref OnAbility1, ref callback);
    public void UnsubscribeFromOnAbility1(UnityAction callback) => HelperUtility.UnsubscribeFrom(ref OnAbility1, ref callback);

    public void SubscribeToOnAbility2(UnityAction callback) => HelperUtility.SubscribeTo(ref OnAbility2, ref callback);
    public void UnsubscribeFromOnAbility2(UnityAction callback) => HelperUtility.UnsubscribeFrom(ref OnAbility2, ref callback);

    #endregion
}
