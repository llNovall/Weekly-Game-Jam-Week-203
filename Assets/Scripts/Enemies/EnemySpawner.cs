using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    /// <summary>
    /// Singleton Accessor
    /// </summary>
    public static EnemySpawner Current;

    [SerializeField]
    private float _minSpawnerFrequency, _maxSpawnerFrequency, _spawnerFrequency;

    [SerializeField]
    private float _spawnRadius;

    /// <summary>
    /// Keeps track of time since last spawn
    /// </summary>
    [SerializeField]
    private float _timePassedSinceLastSpawn;

    private Queue<GameObject> _objectPool = new Queue<GameObject>();

    [SerializeField]
    private GameObject _prefabEnemy;

    [SerializeField]
    private bool _isSpawningEnabled;

    [SerializeField]
    private float _maxEnemySpawn, _currentEnemySpawn;

    private List<Vector3> _spawnLocations = new List<Vector3>();
    private void Awake()
    {
        Current = this;
    }
    private void Start()
    {
        _spawnerFrequency = _maxSpawnerFrequency;

        for (int i = 0; i < _maxEnemySpawn; i++)
        {
            if (!CreateObjectFromPrefab())
                i--;
        }
    }

    private void Update()
    {
        if (_isSpawningEnabled)
        {
            AttemptToSpawnObject();
        }
    }

    #region Public Methods

    public void ResetEnemySpawner()
    {
        _objectPool.Clear();
    }

    /// <summary>
    /// This function can be used to add a game object to the object pool.
    /// </summary>
    /// <param name="objectToAdd"></param>
    public void AddObjectToObjectPool(GameObject objectToAdd)
    {
        if (!_objectPool.Contains(objectToAdd))
        {
            _objectPool.Enqueue(objectToAdd);
            _currentEnemySpawn--;
        }
            
    }

    /// <summary>
    /// This function is used to activate spawning.
    /// </summary>
    /// <param name="isEnabled"></param>
    public void EnableSpawning(bool isEnabled) => _isSpawningEnabled = isEnabled;
    #endregion

    #region Private Methods
    /// <summary>
    /// This function attempts to spawn object if sufficient time has passed
    /// </summary>
    private void AttemptToSpawnObject()
    {
        if (_timePassedSinceLastSpawn >= _spawnerFrequency)
        {
            _timePassedSinceLastSpawn -= _spawnerFrequency;

            if (_objectPool.Count > 0)
                UseObjEnemyFromObjectPool();
            else
                CreateObjectFromPrefab();

            UpdateSpawnerFrequency();
        }
        else
            _timePassedSinceLastSpawn += Time.deltaTime;
    }

    /// <summary>
    /// This function updates spawner frequency until max frequency is met
    /// </summary>
    private void UpdateSpawnerFrequency() => _spawnerFrequency = UnityEngine.Random.Range(_minSpawnerFrequency, _maxSpawnerFrequency);
    /// <summary>
    /// This function takes an object from object pool and moves it to a new origin location
    /// </summary>
    private void UseObjEnemyFromObjectPool()
    {
        if(_objectPool.Count > 0 && _currentEnemySpawn < _maxEnemySpawn)
        {
            Vector3 spawnPosition = GenerateStartPosition();

            if(spawnPosition != Vector3.one * -1)
            {
                _currentEnemySpawn++;
                GameObject objEnemyFromObjectPool = _objectPool.Dequeue();
                //EnemyStartData EnemyStartData = new EnemyStartData(spawnPosition);
                objEnemyFromObjectPool.SetActive(true);

                EnemyInitializer EnemyInitializer = objEnemyFromObjectPool.GetComponent<EnemyInitializer>();
                if (EnemyInitializer)
                    EnemyInitializer.Initialize();
                else
                    Debug.LogError($"{GetType().FullName} : Failed to find EnemyInitializer on EnemyObject.");
            }
        }
    }

    private Vector3 GenerateStartPosition()
    {
        Vector3 center = new Vector3(Random.Range(-10, 390), 0, Random.Range(10, -10));
        Vector3 randomPoint = UnityEngine.Random.insideUnitSphere * _spawnRadius + center;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, 20, 3))
        {
            _spawnLocations.Add(hit.position);
            return hit.position;
        }
            
        else
            return Vector3.one * -1;
    }
    /// <summary>
    /// This method creates an object using the prefab at a location and initializes it.
    /// </summary>
    private bool CreateObjectFromPrefab()
    {
        if (_prefabEnemy && _currentEnemySpawn < _maxEnemySpawn)
        {
            Vector3 spawnPosition = GenerateStartPosition();

            if(spawnPosition != Vector3.one * -1)
            {
                _currentEnemySpawn++;
                //EnemyStartData EnemyStartData = new EnemyStartData(spawnPosition);

                GameObject objEnemy = Instantiate(_prefabEnemy, spawnPosition, Quaternion.identity);

                EnemyInitializer EnemyInitializer = objEnemy.GetComponent<EnemyInitializer>();
                if (EnemyInitializer)
                {
                    EnemyInitializer.Initialize();
                    EnemySpawnerInfo EnemySpawnerInfo = objEnemy.GetComponent<EnemySpawnerInfo>();
                    EnemySpawnerInfo.Initialize(this);

                    return true;
                }
                else
                    Debug.LogError($"{GetType().FullName} : Failed to find EnemyInitializer on EnemyObject.");
            }
        }
        else
            Debug.LogError($"{GetType().FullName} : Failed to find Prefab to create Game Object.");

        return false;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < _spawnLocations.Count; i++)
        {
            Gizmos.DrawCube(_spawnLocations[i], Vector3.one);
        }
    }
}
