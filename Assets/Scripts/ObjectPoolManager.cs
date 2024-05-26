using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [Header("Environment")]
    [SerializeField] private BasePlatform basePlatform; 
    [SerializeField] private Collectable coinsPrefab; 
    [SerializeField] private Obstacle obstaclesPrefab; 
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private PlayerMovement player;

    private Queue<BasePlatform> platformsPool = new Queue<BasePlatform>();  

    private Queue<Collectable> coinsPool = new Queue<Collectable>();  
    private Queue<Obstacle> obstaclesPool = new Queue<Obstacle>();  

    public BasePlatform SpawnPlatform(Vector3 position) 
    {
        BasePlatform instance = null;
        if (platformsPool.Count > 0 && levelGenerator.IsEnvironmentLoaded)
        {
            instance = platformsPool.Dequeue();
        }
        else
        {
            instance = Instantiate(basePlatform);
        }

        instance?.gameObject.SetActive(false);
        instance.transform.position = position;
        instance?.gameObject.SetActive(true);

        PoolPlatform(instance);

        return instance;
    }

    public void SpawnObstacles(BasePlatform spawnedPlatform, Vector3 position) // Move
    {
        Obstacle instance = null;
        if (obstaclesPool.Count > 0 && levelGenerator.IsEnvironmentLoaded)
        {
            instance = obstaclesPool.Dequeue();
        }
        else
        {
            instance = Instantiate(obstaclesPrefab);
        }

        instance.transform.SetParent(spawnedPlatform.transform);
        instance.transform.localPosition = position;
        instance?.gameObject.SetActive(true);
        PoolObstacle(instance);
    }

    public void SpawnCoins(BasePlatform spawnedPlatform, Vector3 position) // Move
    {
        Collectable instance = null;
        if (coinsPool.Count > 0 && levelGenerator.IsEnvironmentLoaded)
        {
            instance = coinsPool.Dequeue();
        }
        else
        {
            instance = Instantiate(coinsPrefab);
        }

        instance.transform.SetParent(spawnedPlatform.transform);
        instance.transform.localPosition = position;
        instance?.gameObject.SetActive(true);
        PoolCoin(instance);
    }

    public void PoolPlatform(BasePlatform basePlatform)
    {
        platformsPool.Enqueue(basePlatform);
    }

    public void PoolObstacle(Obstacle obstacle)
    {
        obstaclesPool.Enqueue(obstacle);
    }

    public void PoolCoin(Collectable coin)
    {
        coinsPool.Enqueue(coin);
    }
}
