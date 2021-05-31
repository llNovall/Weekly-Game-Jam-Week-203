using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIMovement : MonoBehaviour
{
    [SerializeField]
    protected NavMeshAgent _agent;

    [SerializeField]
    protected EnemyAgentMovementData _data;

    [SerializeField]
    protected AIPassiveState _state;

    [SerializeField]
    protected PlayerDetector _playerDetector;

    private void Start()
    {
        _playerDetector.SubscribeToOnPlayerDetected(PlayerDetector_OnPlayerDetected);
    }
    public void Initialize(EnemyAgentMovementData data)
    {
        _data = data;

        if (!(_agent = gameObject.GetComponent<NavMeshAgent>()))
            _agent = gameObject.AddComponent<NavMeshAgent>();

        _agent.speed = _data.Speed;
        _agent.angularSpeed = _data.AngularSpeed;
        _agent.acceleration = _data.Acceleration;
    }

    public abstract void ChangeState(AIPassiveState state);

    private void PlayerDetector_OnPlayerDetected(bool isPlayerDetected)
    {
        if (isPlayerDetected)
            ChangeState(AIPassiveState.Flee);
        else
            ChangeState(AIPassiveState.Idle);
    }

}
