using System;
using UnityEngine;

public class SelectProductView : MonoBehaviour
{
    public event Action<Product> OnProductSelected;

    [SerializeField]
    private ProductPreview _productPreviewPrefab;

    [SerializeField]
    private Transform _productContainer;

    [SerializeField]
    private GameObject _selectProductRootObject;

    public void Initialize(ProductsCollection productsCollection)
    {
        foreach(var product in productsCollection.ProductsList)
        {
            var productPreview = Instantiate(_productPreviewPrefab, _productContainer);
            productPreview.Initialize(product);
            productPreview.OnProductSelect += OnProductSelectHandler;
        }
    }

    public void Show()
    {
        _selectProductRootObject.SetActive(true);
    }

    public void Hide()
    {
        _selectProductRootObject.SetActive(false);
    }

    private void OnProductSelectHandler(Product selectedProduct)
    {
        OnProductSelected?.Invoke(selectedProduct);
    }
}
