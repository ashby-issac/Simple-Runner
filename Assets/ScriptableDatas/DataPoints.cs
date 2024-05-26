using System.Linq;
using UnityEngine;

[System.Serializable]
public class CoinsData
{
    public Vector3[] leftDataPoints;
    public Vector3[] centerDataPoints;
    public Vector3[] rightDataPoints;
}

[CreateAssetMenu(fileName = "DataPoints", menuName = "DataPoints")]
public class DataPoints : ScriptableObject
{
    [Header("Reusable reference data for more coin placements")]
    [SerializeField] private CoinsData ReferenceData;

    [Header("All Coins Data")]
    [SerializeField] private CoinsData[] coinsDatas;

    public CoinsData GetDataPoints()
    {
        var dataIndex = Random.Range(0, coinsDatas.Count());
        return coinsDatas[dataIndex];
    }
}
