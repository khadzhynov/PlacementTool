using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Product : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public event Action<Vector3> OnPositionUpdate;

    [SerializeField]
    private float _width;

    private Vector3 _minPoint;
    private Vector3 _maxPoint;

    private Vector3 _dragOffset;

    public float Width { get => _width; }

    public void SetLimits(Vector3 minPoint, Vector3 maxPoint)
    {
        Vector3 minMax = maxPoint - minPoint;
        _minPoint = minPoint + minMax.normalized * Width / 2f;
        _maxPoint = maxPoint - minMax.normalized * Width / 2f;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 hitPoint;
        if (Raycast(eventData.pressEventCamera, eventData.position, out hitPoint))
        {
            _dragOffset = hitPoint - transform.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 hitPoint;
        if (Raycast(eventData.pressEventCamera, eventData.position, out hitPoint))
        {
            Vector3 desiredPosition = hitPoint - _dragOffset;
            desiredPosition.y = 0;
            MoveWithLimits(desiredPosition);
        }
    }

    private bool Raycast(Camera camera, Vector2 position, out Vector3 hitPoint)
    {
        Plane plane = new Plane(transform.forward, transform.position);
        Ray ray = camera.ScreenPointToRay(position);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            hitPoint = ray.GetPoint(distance);
            return true;
        }
        hitPoint = default;
        return false;
    }

    private void MoveWithLimits(Vector3 desiredPosition)
    {
        Vector3 limitsVector = _maxPoint - _minPoint;
        if (Vector3.Dot(limitsVector, desiredPosition - _maxPoint) < 0)
        {
            if (Vector3.Dot(-limitsVector, desiredPosition - _minPoint) < 0)
            {
                transform.position = desiredPosition;
            }
            else
            {
                transform.position = _minPoint;
            }
        }
        else
        {
            transform.position = _maxPoint;
        }
        OnPositionUpdate?.Invoke(transform.position);
    }
}
