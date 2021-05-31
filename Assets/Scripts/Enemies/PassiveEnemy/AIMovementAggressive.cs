using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovementAggressive : AIMovement
{
    private AIMovementAggressiveState _currentState;

    [SerializeField]
    private Transform _player;

    private void Start()
    {
        _player = PlayerTracker.Current.transform;
    }

    private void Update()
    {
        if (_agent.isActiveAndEnabled)
        {
            switch (_currentState)
            {
                case AIMovementAggressiveState.Idle:
                    MoveToRandomLocation();
                    break;
                case AIMovementAggressiveState.ChasePlayer:
                    ChasePlayer();
                    break;
                default:
                    break;
            }
        }
    }


    private void ChangeState(AIMovementAggressiveState state)
    {
        if (_currentState != state)
        {
            _currentState = state;

            switch (_currentState)
            {
                case AIMovementAggressiveState.Idle:
                    MoveToRandomLocation();
                    break;
                case AIMovementAggressiveState.ChasePlayer:
                    _agent.ResetPath();
                    ChasePlayer();
                    break;
                default:
                    break;
            }
        }
    }

    private void ChasePlayer()
    {
        _agent.speed = _data.ChaseSpeed;

        _agent.SetDestination(_player.position);
        if (_agent.isStopped)
            _agent.isStopped = false;
    }
    protected override void PlayerDetector_OnPlayerDetected(bool isPlayerDetected)
    {
        if (isPlayerDetected)
            ChangeState(AIMovementAggressiveState.ChasePlayer);
        else
            ChangeState(AIMovementAggressiveState.Idle);
    }
}
