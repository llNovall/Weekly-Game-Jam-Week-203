using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerInfo : MonoBehaviour
{
    [SerializeField]
    private EnemySpawner _parentSpawner;

    [SerializeField]
    private EnemyHealth _enemyHealth;

    private void Awake()
    {
        //_enemyHealth = gameObject.GetComponent<EnemyHealth>();
        //if (!enemyHealth)
        //    gameObject.GetComponentInChildren<EnemyHealth>();

        if (_enemyHealth)
        {
            _enemyHealth.SubscribeToOnDeath(EnemyHealth_OnDeath);
        }
        else
            Debug.LogError($"{GetType().FullName} : Failed to find EnemyHealth");
    }

    public void Initialize(EnemySpawner enemySpawner)
    {
        _parentSpawner = enemySpawner;
    }

    /// <summary>
    /// If Enemy is dead, it disables gameobject. If not, it enables the gameobject.
    /// </summary>
    /// <param name="isDead"></param>
    private void EnemyHealth_OnDeath(bool isDead)
    {
        gameObject.SetActive(!isDead);

        if (isDead)
            _parentSpawner.AddObjectToObjectPool(gameObject);
    }

    private void OnBecameInvisible()
    {
        _parentSpawner.AddObjectToObjectPool(gameObject);
    }
}
