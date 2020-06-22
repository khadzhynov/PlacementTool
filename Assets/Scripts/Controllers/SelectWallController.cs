using System;
using UnityEngine;

public class SelectWallController : MonoBehaviour
{
    public event Action<Wall> OnSelectWall;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private SelectWallModel _model;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _model.MaxRaycastDistance, _model.WallsLayerMask))
            {
                var wall = hit.collider.GetComponent<Wall>();
                if (wall != null)
                {
                    OnSelectWall?.Invoke(wall);
                }
            }
        }
    }
}
