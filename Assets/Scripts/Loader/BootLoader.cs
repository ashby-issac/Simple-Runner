using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootLoader : MonoBehaviour
{
    [SerializeField] private ObjectPoolManager poolManager;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private LevelGenerator levelGenerator;

    private void OnEnable()
    {
        InitOnGameStart();
    }

    public void InitOnGameStart()
    {
        poolManager.gameObject.SetActive(true);
        player.gameObject.SetActive(true);
        levelGenerator.gameObject.SetActive(true);
    }
}
