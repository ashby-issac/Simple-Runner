using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlatform : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    public Vector3 GetStartPointPosition() => startPoint.position;
    public Vector3 GetEndPointPosition() => endPoint.position;
}
