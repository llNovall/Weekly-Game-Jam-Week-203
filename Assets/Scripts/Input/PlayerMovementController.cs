using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private PlayerInputController _playerInputController;

    [SerializeField]
    private bool _isAbleToMove, _isGrounded;

    [SerializeField]
    private float _movementSpeed;

    [SerializeField]
    private float _jumpSpeed, _jumpScaler;

    [SerializeField]
    private float _defaultGravity;

    [SerializeField]
    private Vector3 _rayCastOffset;

    [SerializeField]
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        if(!(_rigidbody2D = gameObject.GetComponent<Rigidbody2D>()))
            _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();

        _rigidbody2D.mass = 2;
        _rigidbody2D.drag = 1;
    }
    private void Start()
    {
        _playerInputController = gameObject.GetComponent<PlayerInputController>();
        _playerInputController.SubscribeToOnPlayerMovementInputUpdated(PlayerInputController_OnPlayerMovementInputUpdated);
        _playerInputController.SubscribeToOnJump(PlayerInputController_OnJump);
    }

    private void LateUpdate()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(gameObject.transform.position + _rayCastOffset, -gameObject.transform.up, 1);

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject != gameObject)
                {
                    _isGrounded = true;
                    break;
                }
            }
        }
        else
            _isGrounded = false;
        
    }
    private void PlayerInputController_OnJump()
    {
        if (_isGrounded)
        {
            if (_isAbleToMove)
            {
                _rigidbody2D.AddForce(Vector2.up * _jumpSpeed * _jumpScaler);
            }
        }
    }

    private void PlayerInputController_OnPlayerMovementInputUpdated(float movementInput)
    {
        if (_isAbleToMove)
        {
            float sideMovement = movementInput * _movementSpeed;
            //gameObject.transform.position += new Vector3(sideMovement, 0, 0);

            _rigidbody2D.AddForce(new Vector2(sideMovement, 0));
 
        }
    }

    public void EnableGravity(bool isEnabled)
    {
        if (isEnabled)
            _rigidbody2D.gravityScale = _defaultGravity;
        else
            _rigidbody2D.gravityScale = 0;

        //if (isEnabled)
        //    _rigidbody2D.isKinematic = false;
        //else
        //    _rigidbody2D.isKinematic = true;
    }
    public void EnableMovement(bool isEnabled)
    {
        _isAbleToMove = isEnabled;

        if (!_isAbleToMove)
        {
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.angularVelocity = 0;
        }
            
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Ray ray = new Ray(gameObject.transform.position + _rayCastOffset, Vector3.down);
        Gizmos.DrawRay(ray);
    }
}
