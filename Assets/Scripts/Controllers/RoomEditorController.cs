using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class RoomEditorController : MonoBehaviour, IPointerDownHandler
{
    public event Action<List<Vector3>> OnDone;
    public event Action OnCancel;

    [SerializeField]
    private RoomEditorModel _model;

    [SerializeField]
    private Button _confirm;

    [SerializeField]
    private Button _cancel;

    [SerializeField]
    private Canvas _canvas;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private LineRenderer _lineRenderer;

    private void Start()
    {
        _confirm.onClick.AddListener(OnCompleteDrawingHandler);
        _cancel.onClick.AddListener(OnCancelDrawingHandler);
        _model.Points.Clear();
    }

    private void OnEnable()
    {
        foreach (var point in _model.Points)
        {
            point.OnClick += OnPlanPointClickHandler;
            point.OnDragged += OnPlanPointDraggedHandler;
        }
    }

    private void OnDisable()
    {
        Clear();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var planPoint = Instantiate(_model.PlanPointPrefab, transform);
        planPoint.Initialize(_model.Points.Count, _canvas);
        planPoint.OnDrag(eventData);
        planPoint.OnClick += OnPlanPointClickHandler;
        planPoint.OnDragged += OnPlanPointDraggedHandler;
        _model.Points.Add(planPoint);
        planPoint.transform.position += Vector3.back * 2;
        UpdateLine(planPoint);
    }

    private void UpdateLine(RoomPlanPoint planPoint)
    {
        _lineRenderer.enabled = _model.Points.Count > 1;
        _lineRenderer.positionCount = _model.Points.Count;
        _lineRenderer.SetPosition(_model.Points.Count - 1, planPoint.transform.position + Vector3.forward);
    }

    private void OnPlanPointClickHandler(int pointIndex)
    {
        if (pointIndex == 0)
        {
            OnCompleteDrawingHandler();
        }
    }

    private void OnPlanPointDraggedHandler(int pointIndex)
    {
        _lineRenderer.SetPosition(pointIndex, _model.Points[pointIndex].transform.position + Vector3.forward);
    }

    private void OnCompleteDrawingHandler()
    {
        if (_model.Points.Count > 2)
        {
            List<Vector3> worldPoints = new List<Vector3>();
            foreach (var point in _model.Points)
            {
                Vector3 worldPosition = ToWorldPosition(point);

                worldPoints.Add(worldPosition);
            }
            Clear();
            gameObject.SetActive(false);
            OnDone?.Invoke(worldPoints);
        }
    }

    private Vector3 ToWorldPosition(RoomPlanPoint point)
    {
        Vector2 viewportPosition = (point.transform as RectTransform).anchoredPosition;
        viewportPosition.x /= Screen.width;
        viewportPosition.y /= Screen.height;

        viewportPosition -= Vector2.one * 0.5f;

        float horizontalSize = (float)Screen.width / (float)Screen.height * _model.VerticalSize;

        Vector3 worldPosition = new Vector3(
            viewportPosition.x * horizontalSize,
            0,
            viewportPosition.y * _model.VerticalSize
            );
        return worldPosition;
    }

    private void OnCancelDrawingHandler()
    {
        Clear();
        gameObject.SetActive(false);
        OnCancel?.Invoke();
    }
   
    public void Clear()
    {
        _lineRenderer.positionCount = 0;
        _lineRenderer.enabled = false;
        foreach (var point in _model.Points)
        {
            point.OnClick -= OnPlanPointClickHandler;
            point.OnDragged -= OnPlanPointDraggedHandler;
            Destroy(point.gameObject);
        }
        _model.Points.Clear();
    }
}

