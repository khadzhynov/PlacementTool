using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room Editor Model", menuName = "ScriptableObjects/Room Editor Model", order = 0)]
public class RoomEditorModel : ScriptableObject
{
    public float VerticalSize = 8;

    public RoomPlanPoint PlanPointPrefab;

    public List<RoomPlanPoint> Points = new List<RoomPlanPoint>();
}
