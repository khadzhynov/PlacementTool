using UnityEngine;

[CreateAssetMenu(fileName = "Select Wall Model", menuName = "ScriptableObjects/Select Wall  Model", order = 0)]
public class SelectWallModel : ScriptableObject
{
    [SerializeField]
    private LayerMask _wallsLayerMask;

    [SerializeField]
    private float _maxRaycastDistance = 100;

    public LayerMask WallsLayerMask { get => _wallsLayerMask; }
    public float MaxRaycastDistance { get => _maxRaycastDistance; }
}
