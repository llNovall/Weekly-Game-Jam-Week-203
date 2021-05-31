using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInitializer : MonoBehaviour
{
    [SerializeField]
    protected EnemyData _enemyData;

    [SerializeField]
    private AIMovement _enemyMovementAI;

    [SerializeField]
    public EnemyHealth EnemyHealth;

    [SerializeField]
    private PlayerDetector _playerDetector;

    [SerializeField]
    private EnemySound _enemySound;
    public virtual void Initialize(EnemyStartData enemyStartData)
    {
        gameObject.transform.position = enemyStartData.Position;
        //gameObject.transform.rotation = enemyStartData.Rotation;

        if (!_enemyMovementAI)
            _enemyMovementAI = gameObject.GetComponent<AIMovement>();

        if (_enemyMovementAI)
            _enemyMovementAI.Initialize(_enemyData.MovementData);
        else
            Debug.LogError($"{GetType().FullName} : Failed to find {typeof(AIMovement).FullName}.");

        if (!EnemyHealth)
            EnemyHealth = gameObject.GetComponent<EnemyHealth>();

        if (EnemyHealth)
            EnemyHealth.Initialize(_enemyData.HealthData);
        else
            Debug.LogError($"{GetType().FullName} : Failed to find {typeof(EnemyHealth).FullName}.");

        if (!_enemySound)
            _enemySound = gameObject.GetComponent<EnemySound>();

        if (_enemySound)
            _enemySound.Initialize();
        else
            Debug.LogError($"{GetType().FullName} : Failed to find {typeof(EnemySound).FullName}.");

        if (!_playerDetector)
            gameObject.GetComponentInChildren<PlayerDetector>();

        if (_playerDetector)
        {
            _playerDetector.Initialize(_enemyData.PlayerDetectorData);
        }
        else
            Debug.LogError($"{GetType().FullName} : Failed to find {typeof(PlayerDetector).FullName}.");

    }
}
