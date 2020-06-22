using System;
using UnityEngine;
using UnityEngine.UI;

public class ProductPreview : MonoBehaviour
{
    public event Action<Product> OnProductSelect;

    [SerializeField]
    private Transform _pivot;

    [SerializeField]
    private Button _button;

    private Product _product;

    public void Initialize(Product productPrefab)
    {
        _product = productPrefab;

        var productModel = Instantiate(productPrefab, _pivot);

        foreach(var collider in productModel.GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
            collider.gameObject.layer = gameObject.layer;
        }
        
        _button.onClick.AddListener(OnButtonClickHandler);
    }

    private void OnButtonClickHandler()
    {
        OnProductSelect?.Invoke(_product);
    }
}
