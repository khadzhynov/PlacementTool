using System;
using UnityEngine;
using UnityEngine.UI;

public class PlacementController : MonoBehaviour
{
    public event Action<Product> OnProductPlacementDone;

    [SerializeField]
    private Button _confirmButton;

    [SerializeField]
    private Button _cancelButton;

    [SerializeField]
    private PlacementModel _model;

    private void OnEnable()
    {
        _confirmButton.onClick.AddListener(OnConfirmButtonClickHandler);
        _cancelButton.onClick.AddListener(OnCancelButtonClickHandler);
    }

    private void Start()
    {
        _model.PlacedProducts.Clear();
        _model.CurrnetProductInstance = null;
    }

    public Product PlaceProduct(Vector3 wallBegin, Vector3 wallEnd)
    {
        SetButtons(true);
        _model.CurrnetProductInstance = Instantiate(_model.SelectedProduct);
        _model.CurrnetProductInstance.transform.position = (wallBegin + wallEnd) / 2f;
        _model.CurrnetProductInstance.transform.forward = Vector3.Cross(wallBegin - wallEnd, Vector3.up);
        _model.CurrnetProductInstance.SetLimits(wallBegin, wallEnd);
        return _model.CurrnetProductInstance;
    }

    public void RemoveAllProducts()
    {
        foreach(var product in _model.PlacedProducts)
        {
            Destroy(product.gameObject);
        }
        _model.PlacedProducts.Clear();
    }

    public void DiscardCurrentPlacement()
    {
        if (_model.CurrnetProductInstance != null)
        {
            Destroy(_model.CurrnetProductInstance.gameObject);
            _model.CurrnetProductInstance = null;
        }
        SetButtons(false);
    }

    private void SetButtons(bool active)
    {
        _confirmButton.gameObject.SetActive(active);
        _cancelButton.gameObject.SetActive(active);
    }

    private void OnConfirmButtonClickHandler()
    {
        OnProductPlacementDone?.Invoke(_model.CurrnetProductInstance);
        _model.PlacedProducts.Add(_model.CurrnetProductInstance);
        _model.CurrnetProductInstance = null;
        SetButtons(false);
    }

    private void OnCancelButtonClickHandler()
    {
        OnProductPlacementDone?.Invoke(_model.CurrnetProductInstance);
        DiscardCurrentPlacement();
    }

    internal void SetSelectedProduct(Product selectedProduct)
    {
        _model.SelectedProduct = selectedProduct;
    }
}
