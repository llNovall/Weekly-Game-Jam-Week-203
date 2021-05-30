using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private PlayerInputController _playerInputController;

    [SerializeField]
    private bool _isAbleToMove;

    [SerializeField]
    private float _movementSpeed;

    [SerializeField]
    private float _minDistanceFromWall;

    [SerializeField]
    private Rigidbody _rBody;

    private Vector2 _movement;

    [SerializeField]
    private float _offset;

    private void Awake()
    {
        if(!(_rBody = gameObject.GetComponent<Rigidbody>()))
            _rBody = gameObject.AddComponent<Rigidbody>();

        _rBody.isKinematic = true;
    }
    private void Start()
    {
        _playerInputController = gameObject.GetComponent<PlayerInputController>();
        _playerInputController.SubscribeToOnPlayerMovementInputUpdated(PlayerInputController_OnPlayerMovementInputUpdated);
    }

    private void Update()
    {
        FaceMouse();
    }
    private void PlayerInputController_OnPlayerMovementInputUpdated(Vector2 movementInput)
    {
        if (_isAbleToMove)
        {
            Vector2 movement = movementInput * _movementSpeed * Time.deltaTime;
            _movement = movement;

            int layerMask = LayerMask.GetMask("Walls");
            bool isHit = Physics2D.Raycast(gameObject.transform.position, movementInput, _minDistanceFromWall, layerMask);

            if (isHit)
                return;
            gameObject.transform.position += new Vector3(movement.x, 0,movement.y);
        }
    }
    private void FaceMouse()
    {
        if (_isAbleToMove)
        {

            Vector3 direction = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - gameObject.transform.position;
            direction = direction.normalized;
            //Debug.LogError(direction);
            float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            gameObject.transform.rotation = Quaternion.Euler(-90, -angle + _offset,0);
            //gameObject.transform.LookAt(direction);
        }     
    }

    public void EnableMovement(bool isEnabled) => _isAbleToMove = isEnabled;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Ray ray = new Ray(gameObject.transform.position, _movement);
        Gizmos.DrawRay(ray);
    }
}