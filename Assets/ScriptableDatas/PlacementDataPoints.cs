using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlacementsData
{
    public Vector3[] leftDataPoints;
    public Vector3[] centerDataPoints;
    public Vector3[] rightDataPoints;
}

[CreateAssetMenu(fileName = "PlacementDataPoints", menuName = "PlacementDataPoints")]
public class PlacementDataPoints : ScriptableObject
{
    [Header("Reusable reference data for more placements")]
    [SerializeField] private PlacementsData ReferenceData;

    [Header("All Coins Data")]
    [SerializeField] private PlacementsData[] placementsData;

    [SerializeField] private string dataContainerName;

    private int dataIndex = 0;

    public PlacementsData GetDataPoints(ref int dataPointIndex)
    {
        if (dataPointIndex == -1)
        {
            dataIndex = Random.Range(0, placementsData.Count());
            dataPointIndex = dataIndex;
        }
        else
        {
            dataIndex = dataPointIndex;
        }

        //Debug.LogError($":: DataIndex {dataContainerName}: {dataIndex}");
        return placementsData[dataIndex];
    }
}
