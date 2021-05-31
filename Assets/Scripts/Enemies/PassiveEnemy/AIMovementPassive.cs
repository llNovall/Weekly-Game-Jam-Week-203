using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovementPassive : AIMovement
{
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

    public override void ChangeState(AIPassiveState state)
    {
        if (_state != state)
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

        Vector3 destination = gameObject.transform.position + ((gameObject.transform.position - playerPosition).normalized * _data.FleeRadius) + Random.insideUnitSphere * _data.FleeRadius;
        destination.y = 0;
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
