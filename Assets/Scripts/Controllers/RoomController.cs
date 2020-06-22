using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

public class RoomController : MonoBehaviour
{
    public event Action<Vector3, Vector3> OnWallClick;

    [SerializeField]
    private RoomModel _model;

    private void Start()
    {
        Build(_model.DefaultRoom);
    }

    private void OnDisable()
    {
        DisposeGeometry();
    }

    public void Build(List<Vector3> cornerPoints)
    {
        Assert.IsNotNull(cornerPoints, RoomErrors.CORNER_POINTS_IS_NULL);
        Assert.IsTrue(cornerPoints.Count > 1, RoomErrors.NOT_ENOUGH_CORNER_POINTS);

        DisposeGeometry();

        for (int i = 0; i < cornerPoints.Count; ++i)
        {
            int previousPointIndex = i == 0 ? cornerPoints.Count - 1 : i - 1;
            BuildWall(cornerPoints[previousPointIndex], cornerPoints[i]);
        }

        BuildSlab(cornerPoints.ToArray(), false, 0, _model.FloorMaterial);

        BuildSlab(cornerPoints.ToArray(), true, _model.Height, _model.CeilingMaterial);
    }

    private void BuildWall(Vector3 cornerBegin, Vector3 cornerEnd)
    {
        var newWall = Instantiate(_model.WallPrefab);
        newWall.Setup(cornerBegin, cornerEnd, _model.Height, _model.WallsWidth, _model.WallMaterial);
        _model.Walls.Add(newWall);
    }

    private void BuildSlab(Vector3[] Points, bool invertNormals, float height, Material material)
    {
        var slab = Instantiate(_model.SlabPrefab);

        List<Vector2> plan = new List<Vector2>();
        for (int i = 0; i < Points.Length; i++)
        {
            plan.Add(new Vector2(Points[i].x, Points[i].z));
        }

        slab.MeshFilter.mesh = MeshGenerator.BuildFlatMesh(plan, Vector3.up, invertNormals);
        slab.transform.position += Vector3.up * height;
        slab.MeshRenderer.material = material;

        _model.Slabs.Add(slab);
    }

    private void DisposeGeometry()
    {
        DisposeWalls();
        DisposeSlabs();
    }

    private void DisposeSlabs()
    {
        foreach (var slab in _model.Slabs)
        {
            if (slab != null)
            {
                if (slab.gameObject != null)
                {
                    Destroy(slab.gameObject);
                }
            }
        }
        _model.Slabs.Clear();
    }

    private void DisposeWalls()
    {
        foreach (var wall in _model.Walls)
        {
            if (wall != null)
            {
                if (wall.gameObject != null)
                {
                    Destroy(wall.gameObject);
                }
            }
        }
        _model.Walls.Clear();
    }
}
