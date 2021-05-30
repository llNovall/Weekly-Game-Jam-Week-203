using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPassive : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _agent;

    [SerializeField]
    private EnemyAgentMovementData _data;

    [SerializeField]
    private AIPassiveState _state;

    [SerializeField]
    private PlayerDetector _playerDetector;

    private void Awake()
    {
        if (!(_agent = gameObject.GetComponent<NavMeshAgent>()))
            _agent = gameObject.AddComponent<NavMeshAgent>();

        _agent.speed = _data.Speed;
        _agent.angularSpeed = _data.AngularSpeed;
        _agent.acceleration = _data.Acceleration;
    }
    private void Start()
    {
        _playerDetector.SubscribeToOnPlayerDetected(PlayerDetector_OnPlayerDetected);
    }

    private void PlayerDetector_OnPlayerDetected(bool isPlayerDetected)
    {
        if (isPlayerDetected)
            ChangeState(AIPassiveState.Flee);
        else
            ChangeState(AIPassiveState.Idle);
    }

    private void Update()
    {
        switch (_state)
        {
            case AIPassiveState.Idle:
                MoveToRandomLocation();
                break;
            case AIPassiveState.Flee:
                FleeFromPlayer();
                break;
            default:
                break;
        }
        if(_agent.remainingDistance < 1)
        {
            _agent.SetDestination(GetARandomPositionOnNavMesh(200));
        }
    }

    private void MoveToRandomLocation()
    {
        if (_agent.hasPath)
            return;

        _agent.speed = _data.Speed;

        Vector3 destination = GetARandomPositionOnNavMesh(_data.WanderRadius);

        if(destination != Vector3.one * -1)
        {
            NavMeshPath path = new NavMeshPath();

            NavMesh.CalculatePath(gameObject.transform.position, destination, NavMesh.AllAreas, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                if (_agent.SetPath(path))
                {
                    if (_agent.isStopped)
                        _agent.isStopped = false;
                }
                else
                    Debug.LogError($"{GetType().FullName} : Failed to set path.");
            }
        }
    }

    private Vector3 GetARandomPositionOnNavMesh(float searchRadius)
    {
        Vector3 randomPoint = gameObject.transform.position + UnityEngine.Random.insideUnitSphere * searchRadius;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, searchRadius, NavMesh.AllAreas))
            return hit.position;
        else
            return Vector3.one * -1;
    }

    public void ChangeState(AIPassiveState state)
    {
        if(_state != state)
        {
            _state = state;

            switch (_state)
            {
                case AIPassiveState.Idle:
                    MoveToRandomLocation();
                    break;
                case AIPassiveState.Flee:
                    _agent.ResetPath();
                    FleeFromPlayer();
                    break;
                default:
                    break;
            }
        }
    }
    private void FleeFromPlayer()
    {
        Vector3 playerPosition = PlayerTracker.Current.gameObject.transform.position;

        Vector3 destination = gameObject.transform.position + ((gameObject.transform.position - playerPosition).normalized * _data.FleeRadius);
        _agent.speed = _data.FleeSpeed;

        NavMeshPath path = new NavMeshPath();

        NavMesh.CalculatePath(gameObject.transform.position, destination, NavMesh.AllAreas, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            if (_agent.SetPath(path))
            {
                if (_agent.isStopped)
                    _agent.isStopped = false;
            }
            else
                Debug.LogError($"{GetType().FullName} : Failed to set path.");
        }
    }
}
