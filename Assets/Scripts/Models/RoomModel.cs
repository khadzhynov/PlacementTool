using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room Model", menuName = "ScriptableObjects/Room Model", order = 0)]
public class RoomModel : ScriptableObject
{
    public List<Vector3> DefaultRoom;

    public float Height;

    public float WallsWidth;

    public List<Wall> Walls;

    public List<Slab> Slabs;

    public Wall WallPrefab;

    public Slab SlabPrefab;

    public Material WallMaterial;

    public Material FloorMaterial;

    public Material CeilingMaterial;
}
