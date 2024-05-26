using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (!player) return;

        transform.position = player.position + offset;
    }
}
