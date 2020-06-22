using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RoomPlanPoint : RectTransformBehaviour, IDragHandler
{
    public event Action<int> OnClick;
    public event Action<int> OnDragged;

    [SerializeField]
    private Button _button;

    private int _id;

    private Canvas _canvas;

    public void Initialize(int id, Canvas canvas)
    {
        _id = id;
        _canvas = canvas;
        _button.onClick.AddListener(OnButtonClickHandler);
    }

    void OnButtonClickHandler()
    {
        OnClick?.Invoke(_id);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetPosition(eventData.position);
        OnDragged?.Invoke(_id);
    }

    private void SetPosition(Vector2 position)
    {
        rectTransform.anchoredPosition = position / _canvas.scaleFactor;
    }
}

