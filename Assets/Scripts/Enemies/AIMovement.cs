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

    public void EnableAgent(bool isEnabled) => _agent.enabled = isEnabled;

    protected void MoveToRandomLocation()
    {
        if (_agent.hasPath)
            return;

        _agent.speed = _data.Speed;

        Vector3 destination = GetARandomPositionOnNavMesh(_data.WanderRadius);

        if (destination != Vector3.one * -1)
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
    protected abstract void PlayerDetector_OnPlayerDetected(bool isPlayerDetected);

}
