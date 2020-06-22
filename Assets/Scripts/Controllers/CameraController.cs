using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CameraController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public event Action OnEndRotation;
    public event Action OnStartRotation;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private CameraModel _model;

    public bool IsDrag { get => _model.IsDrag; }

    private void Start()
    {
        ResetRotation();
    }
    public void ResetRotation()
    {
        _model.HorizontalRotation = 0;
        _model.VerticalRotation = 0;
        _camera.transform.eulerAngles = new Vector3(_model.VerticalRotation, _model.HorizontalRotation, 0.0f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _model.HorizontalRotation -= _model.Sensetivity * eventData.delta.x;
        _model.VerticalRotation += _model.Sensetivity * eventData.delta.y;
        _camera.transform.eulerAngles = new Vector3(_model.VerticalRotation, _model.HorizontalRotation, 0.0f);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _model.IsDrag = true;
        OnStartRotation?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _model.IsDrag = false;
        OnEndRotation?.Invoke();
    }
}
