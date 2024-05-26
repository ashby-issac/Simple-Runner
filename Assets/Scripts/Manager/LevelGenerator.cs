using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private ObjectPoolManager poolManager;
    [SerializeField] private BasePlatform basePlatform; // Move
    [SerializeField] private PlayerMovement player;
    [SerializeField] private int initialSpawnCount;
    [SerializeField] private PlacementDataPoints coinDataPoints;
    [SerializeField] private PlacementDataPoints obstacleDataPoints;

    private float zPosition = 0f;
    private float platformZLength = 0f;
    private bool isEnvironmentLoaded;
    private int dataPointIndex = -1;

    public bool IsEnvironmentLoaded => isEnvironmentLoaded;

    public static LevelGenerator Instance;

    // TODO :: Add a better logic instead of using GetComponent inside OnTrigger
    //public BasePlatform GetPlatformOverlapper(string overlappingPlatformTag)
    //{
    //    return platformsPool.Peek();
    //}

    private void LoadEnvironment()
    {
        var spawnedPlatform = poolManager.SpawnPlatform(new Vector3(0, 0, zPosition));
        zPosition += platformZLength;

        dataPointIndex = -1;
        UpdateObstacles(spawnedPlatform);
        UpdateCoins(spawnedPlatform);
    }

    private void UpdateObstacles(BasePlatform spawnedPlatform)
    {
        PlacementsData placementsData = obstacleDataPoints.GetDataPoints(ref dataPointIndex);

        foreach (var dataPoint in placementsData.leftDataPoints)
        {
            poolManager.SpawnObstacles(spawnedPlatform, dataPoint);
        }
        foreach (var dataPoint in placementsData.centerDataPoints)
        {
            poolManager.SpawnObstacles(spawnedPlatform, dataPoint);
        }
        foreach (var dataPoint in placementsData.rightDataPoints)
        {
            poolManager.SpawnObstacles(spawnedPlatform, dataPoint);
        }
    }

    private void UpdateCoins(BasePlatform spawnedPlatform)
    {
        PlacementsData placementsData = coinDataPoints.GetDataPoints(ref dataPointIndex);

        foreach (var dataPoint in placementsData.leftDataPoints)
            poolManager.SpawnCoins(spawnedPlatform, dataPoint);

        foreach (var dataPoint in placementsData.centerDataPoints)
            poolManager.SpawnCoins(spawnedPlatform, dataPoint);

        foreach (var dataPoint in placementsData.rightDataPoints)
            poolManager.SpawnCoins(spawnedPlatform, dataPoint);
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

        isEnvironmentLoaded = false;
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
            BasePlatform spawnedPlatform = poolManager.SpawnPlatform(new Vector3(0, 0, zPosition));
            zPosition += platformZLength;

            dataPointIndex = -1;
            UpdateObstacles(spawnedPlatform);
            UpdateCoins(spawnedPlatform);

            player.PreviousOverlappingPlatform = null;
        }
    }
}
