using UnityEngine;

public class Slab : MonoBehaviour
{
    [SerializeField]
    private MeshFilter _meshFilter;

    [SerializeField]
    private MeshRenderer _meshRenderer;

    public MeshFilter MeshFilter { get => _meshFilter; }
    public MeshRenderer MeshRenderer { get => _meshRenderer; }
}
