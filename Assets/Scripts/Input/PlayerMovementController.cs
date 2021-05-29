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
    private Rigidbody2D _rigidbody2D;

    private Vector2 _movement;

    [SerializeField]
    private float _offset;

    private void Awake()
    {
        if(!(_rigidbody2D = gameObject.GetComponent<Rigidbody2D>()))
            _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();

        _rigidbody2D.isKinematic = true;
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
            gameObject.transform.position += new Vector3(movement.x, movement.y,0);
        }
    }
    private void FaceMouse()
    {
        if (_isAbleToMove)
        {

            Vector2 direction = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - gameObject.transform.position;
            direction = direction.normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            gameObject.transform.rotation = Quaternion.Euler(0,0,angle + _offset);
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
