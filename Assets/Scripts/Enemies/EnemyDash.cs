using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDash : MonoBehaviour
{
    [SerializeField]
    private EnemySound _enemySound;

    [SerializeField]
    private PlayerSound _playerSound;

    [SerializeField]
    private bool _isAbilityActivated;

    [SerializeField]
    private float _damage;

    [SerializeField]
    private float _maxDashDistance;

    [SerializeField]
    private float _dashSpeed;

    [SerializeField]
    private float _minDistanceFromWall;

    [SerializeField]
    private float _timeRequiredBeforeMoving;

    [SerializeField]
    private float _attackRange;

    [SerializeField]
    private AIMovement _aiMovement;

    [SerializeField]
    private PlayerDetector _playerDetector;

    [SerializeField]
    private Transform _player;

    [SerializeField]
    private bool _isPlayerDetected;

    [SerializeField, Tooltip("This is only for debugging")]
    private Vector3 _dashDestination;

    private void Start()
    {
        _playerDetector.SubscribeToOnPlayerDetected(PlayerDetector_OnPlayerDetected);
        _player = PlayerTracker.Current.transform;
        _playerSound = PlayerTracker.PlayerSound;
    }

    private void Update()
    {
        if(_isPlayerDetected && !_isAbilityActivated)
        {
            if (IsPlayerWithinAttackRange())
                ActivateAbility();
        }
    }

    private bool IsPlayerWithinAttackRange()
    {
        if(Vector3.Distance(_player.position, gameObject.transform.position) <= _maxDashDistance)
        {
            Vector3 direction = (_player.position - gameObject.transform.position).normalized;
            direction.y = 0;

            int layerMask = LayerMask.GetMask("Player");

            return Physics.Raycast(gameObject.transform.position, direction, _maxDashDistance, layerMask);
        }

        return false;
    }
    private void PlayerDetector_OnPlayerDetected(bool isPlayerDetected) => _isPlayerDetected = isPlayerDetected;

    private void ActivateAbility()
    {
        if (!_isAbilityActivated)
        {
            Vector3 destination = GetDestination();

            if(destination != Vector3.one * -1)
            {
                _isAbilityActivated = true;
                PrePerform();
                StartCoroutine(DashToPoint(destination));
            }
        }
            
    }

    protected void PrePerform()
    {
        _aiMovement.EnableAgent(false);
    }

    protected void PostPerform()
    {
        _aiMovement.EnableAgent(true);
        _isAbilityActivated = false;
    }
    private Vector3 GetDestination()
    {
        int playerLayerMask = LayerMask.GetMask("Player");
        Vector3 playerDirection = (_player.position - gameObject.transform.position).normalized;
        playerDirection.y = 0;

        if (Physics.Raycast(gameObject.transform.position, playerDirection, _maxDashDistance, playerLayerMask))
        {
            Vector3 direction = (_player.position - gameObject.transform.position).normalized;
            direction.y = 0;

            int layerMask = LayerMask.GetMask("Walls");
            Ray ray = new Ray(gameObject.transform.position, direction);
            RaycastHit hit;
            Vector3 destination = Vector3.zero;
            if (Physics.Raycast(ray, out hit, _maxDashDistance, layerMask))
            {
                destination = hit.point + (gameObject.transform.position - hit.point).normalized * _minDistanceFromWall;
                destination.y = 0;
                return destination;
            }
            else
            {
                destination = gameObject.transform.position + direction * _maxDashDistance;
                destination.y = 0;
                return destination;
            }
        }
        else
            return Vector3.one * -1;
    }
    private IEnumerator DashToPoint(Vector3 destination)
    {
        WaitForFixedUpdate waitForEndOfFrame = new WaitForFixedUpdate();
        //Vector3 destination = GetDestination();
        _dashDestination = destination;

        Vector3 startPosition = gameObject.transform.position;
        float currentDistance = Vector3.Distance(startPosition, _dashDestination);

        Health playerHealth = _player.GetComponent<Health>();
        _enemySound.PlayDashSound();
        while (currentDistance > 0.1f)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _dashDestination, _dashSpeed * Time.fixedDeltaTime);
            currentDistance = Vector3.Distance(gameObject.transform.position, _dashDestination);
            if(Vector3.Distance(gameObject.transform.position, _player.position) <= _attackRange)
            {
                
                if (playerHealth)
                {
                    Debug.LogError($"{GetType().FullName} : Player Found");
                    playerHealth.ModifyHealth(-_damage);
                    _playerSound.PlayTakeHitSound();
                }
                else
                    Debug.LogError($"{GetType().FullName} : Failed to find Player Health.");
            }
            yield return waitForEndOfFrame;
        }

        float timePassed = 0;

        while(timePassed < _timeRequiredBeforeMoving)
        {
            timePassed += Time.deltaTime;
            yield return waitForEndOfFrame;
        }

        PostPerform();
    }

    private void OnDisable()
    {
        _isAbilityActivated = false;
    }

    private void OnDrawGizmos()
    {
        if (_isAbilityActivated)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(gameObject.transform.position, _dashDestination);

        }
        Gizmos.color = Color.red;
        //_dashDestination = GetDestination();
        //Gizmos.DrawCube(GetDestination(), Vector3.one * 2);
    }
}
