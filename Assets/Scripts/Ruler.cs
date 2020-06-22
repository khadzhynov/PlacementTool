using UnityEngine;
using TMPro;
using System;

public class Ruler : RectTransformBehaviour
{
    public event Action<Product> OnUpdateByTextInput;

    private const string MEASUREMENT = "{0:0.00}m";

    private Vector3 _staticWorldAnchor;

    private Vector3 _widthOffset;

    private Product _product;

    private Vector3 _screenCenterOffset = Vector3.zero;

    private Canvas _canvas;

    [SerializeField]
    private TMP_InputField _measurementInput;

    [SerializeField]
    private CanvasGroup _canvasGroup;

    private Camera _camera;

    public void Initialize(Product product, Vector3 staticWorldAnchor, Canvas canvas, Camera camera)
    {
        _product = product;
        _product.OnPositionUpdate += OnPositionUpdateHandler;
        _canvas = canvas;
        _camera = camera;

        _measurementInput.onEndEdit.AddListener(OnMeasurementInputHandler);

        _staticWorldAnchor = staticWorldAnchor;
        _widthOffset = (staticWorldAnchor - _product.transform.position).normalized * _product.Width / 2f;

        Refresh();
    }

    public void Refresh()
    {
        OnPositionUpdateHandler(_product.transform.position);
    }

    private void OnPositionUpdateHandler(Vector3 newPosition)
    {
        Vector3 dynamicWorldAnchor = newPosition + _widthOffset;

        Vector3 dynamicAnchor;
        if (WorldToScreen(dynamicWorldAnchor, out dynamicAnchor))
        {
            SetRulerVisible(true);
            UpdateTransform(dynamicAnchor);
            _measurementInput.text = string.Format(MEASUREMENT, Vector3.Distance(_staticWorldAnchor, dynamicWorldAnchor));
        }
        else
        {
            SetRulerVisible(false);
        }
    }

    private void SetRulerVisible(bool isVisible)
    {
        _canvasGroup.alpha = isVisible ? 1f : 0f;
        _canvasGroup.interactable = isVisible;
    }

    private void OnMeasurementInputHandler(string input)
    {
        float inputValue;

        if (float.TryParse(input, out inputValue))
        {
            Vector3 moveDirection = (_product.transform.position - _staticWorldAnchor).normalized;
            Vector3 movement = moveDirection * inputValue - _widthOffset;
            _product.transform.position = _staticWorldAnchor + movement;

            OnUpdateByTextInput(_product);
        }
        else
        {
            Debug.LogError(string.Format(RulerErrors.PARSING_ERROR, input));
        }
    }

    private void UpdateTransform(Vector3 dynamicAnchor)
    {
        Vector3 staticAnchor;
        if (WorldToScreen(_staticWorldAnchor, out staticAnchor))
        {
            SetRulerVisible(true);
            rectTransform.anchoredPosition = (staticAnchor + dynamicAnchor) / 2f;
            rectTransform.right = staticAnchor - dynamicAnchor;

            if (Vector3.Dot(rectTransform.up, Vector3.up) < 0)
            {
                rectTransform.right = -rectTransform.right;
            }

            rectTransform.sizeDelta = new Vector2((staticAnchor - dynamicAnchor).magnitude, rectTransform.sizeDelta.y);
        }
        else
        {
            SetRulerVisible(false);
        }
    }

    private bool WorldToScreen(Vector3 worldPosition, out Vector3 screenPosition)
    {
        if (_screenCenterOffset == Vector3.zero)
        {
            _screenCenterOffset = Vector3.down * Screen.height / 2f + Vector3.left * Screen.width / 2f;
        }

        screenPosition = _camera.WorldToScreenPoint(worldPosition) + _screenCenterOffset;
        screenPosition /= _canvas.scaleFactor;

        return screenPosition.z > 0;
    }

    public void Dispose()
    {
        _product.OnPositionUpdate -= OnPositionUpdateHandler;
    }
}
