using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Environment")]
    [SerializeField] private BasePlatform basePlatform; // Move
    [SerializeField] private GameObject coinsPrefab; // Move
    [SerializeField] private GameObject obstaclesPrefab; // Move

    [SerializeField] private PlayerMovement player;
    [SerializeField] private int initialSpawnCount;
    [SerializeField] private PlacementDataPoints coinDataPoints;
    [SerializeField] private PlacementDataPoints obstacleDataPoints;

    private float zPosition = 0f;
    private float platformZLength = 0f;

    private Queue<BasePlatform> platformsPool = new Queue<BasePlatform>();  // Move

    private Queue<GameObject> coinsPool = new Queue<GameObject>();  // Move
    private Queue<GameObject> obstaclesPool = new Queue<GameObject>();  // Move

    private bool isEnvironmentLoaded;
    private int dataPointIndex = -1;

    public static LevelGenerator Instance;

    // TODO :: Add a better logic instead of using GetComponent inside OnTrigger
    public BasePlatform GetPlatformOverlapper(string overlappingPlatformTag)
    {
        return platformsPool.Peek();
    }

    private void LoadEnvironment()
    {
        var spawnedPlatform = SpawnPlatform(); // Move
        UpdateObstacles(spawnedPlatform); 
        UpdateCoins(spawnedPlatform);
    }

    private BasePlatform SpawnPlatform() // Move
    {
        BasePlatform instance = null;
        if (platformsPool.Count > 0 && isEnvironmentLoaded)
        {
            instance = platformsPool.Dequeue();
        }
        else
        {
            instance = Instantiate(basePlatform);
        }

        instance.gameObject.SetActive(false);
        instance.transform.position = new Vector3(0, 0, zPosition);
        instance?.gameObject.SetActive(true);
        zPosition += platformZLength;

        PoolPlatform(instance);

        return instance;
    }

    private void UpdateObstacles(BasePlatform spawnedPlatform)
    {
        //return;
        PlacementsData placementsData = obstacleDataPoints.GetDataPoints(ref dataPointIndex);

        foreach (var dataPoint in placementsData.leftDataPoints)
            SpawnObstacles(spawnedPlatform, dataPoint);

        foreach (var dataPoint in placementsData.centerDataPoints)
            SpawnObstacles(spawnedPlatform, dataPoint);

        foreach (var dataPoint in placementsData.rightDataPoints)
            SpawnObstacles(spawnedPlatform, dataPoint);
    }

    private void UpdateCoins(BasePlatform spawnedPlatform)
    {
        //return;
        PlacementsData placementsData = coinDataPoints.GetDataPoints(ref dataPointIndex);

        foreach (var dataPoint in placementsData.leftDataPoints)
            SpawnCoins(spawnedPlatform, dataPoint);

        foreach (var dataPoint in placementsData.centerDataPoints)
            SpawnCoins(spawnedPlatform, dataPoint);

        foreach (var dataPoint in placementsData.rightDataPoints)
            SpawnCoins(spawnedPlatform, dataPoint);
    }


    private void SpawnObstacles(BasePlatform spawnedPlatform, Vector3 position) // Move
    {
        GameObject instance = null;
        if (obstaclesPool.Count > 0 && isEnvironmentLoaded)
        {
            instance = obstaclesPool.Dequeue();
        }
        else
        {
            instance = Instantiate(obstaclesPrefab, spawnedPlatform.transform);
        }

        instance.transform.SetParent(spawnedPlatform.transform);
        instance.transform.localPosition = position;
        instance?.gameObject.SetActive(true);
        PoolObstacle(instance);
    }

    private void SpawnCoins(BasePlatform spawnedPlatform, Vector3 position) // Move
    {
        GameObject instance = null;
        if (coinsPool.Count > 0 && isEnvironmentLoaded)
        {
            instance = coinsPool.Dequeue();
        }
        else
        {
            instance = Instantiate(coinsPrefab, spawnedPlatform.transform);
        }

        instance.transform.SetParent(spawnedPlatform.transform);
        instance.transform.localPosition = position;
        instance?.gameObject.SetActive(true);
        PoolCoin(instance);
    }

    private void PoolPlatform(BasePlatform basePlatform)
    {
        platformsPool.Enqueue(basePlatform);
    }

    private void PoolObstacle(GameObject obstacle)
    {
        obstaclesPool.Enqueue(obstacle);
    }

    private void PoolCoin(GameObject coin)
    {
        coinsPool.Enqueue(coin);
    }

    private bool HasPlayerExceededPreviousPlatform()
    {
        return player.transform.position.z > player.PreviousOverlappingPlatform.GetEndPointPosition().z;
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        basePlatform.gameObject.SetActive(false);
        platformZLength = (basePlatform.GetEndPointPosition() - basePlatform.GetStartPointPosition()).magnitude;

        for (int i = 0; i < initialSpawnCount; i++)
        {
            LoadEnvironment();
        }

        isEnvironmentLoaded = true;
    }

    private void Update()
    {
        if (player.PreviousOverlappingPlatform && HasPlayerExceededPreviousPlatform()) // when player moves and only 2 are left
        {
            BasePlatform spawnedPlatform = SpawnPlatform();

            // TODO :: Dry Run
            UpdateObstacles(spawnedPlatform);
            UpdateCoins(spawnedPlatform);

            player.PreviousOverlappingPlatform = null;
        }
    }
}
