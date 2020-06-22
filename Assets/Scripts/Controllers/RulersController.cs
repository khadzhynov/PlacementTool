using System.Collections.Generic;
using UnityEngine;

public class RulersController : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private RulersModel _model;

    public void Start()
    {
        _model.RulersByProducts.Clear();
    }

    public void CreateRuler(Product product, Vector3 staticWorldAnchor)
    {
        var ruler = Instantiate(_model.RulerPrefab, transform);
        ruler.Initialize(product, staticWorldAnchor, _canvas, _camera);

        ruler.OnUpdateByTextInput += OnUpdateRulerByTextInputHandler;

        if (_model.RulersByProducts.ContainsKey(product))
        {
            _model.RulersByProducts[product].Add(ruler);
        }
        else
        {
            var newRulersList = new List<Ruler> { ruler };
            _model.RulersByProducts.Add(product, newRulersList);
        }
    }

    private void OnUpdateRulerByTextInputHandler(Product product)
    {
        if (_model.RulersByProducts.ContainsKey(product))
        {
            foreach (var ruler in _model.RulersByProducts[product])
            {
                ruler.Refresh();
            }
        }
    }

    public void RemoveRulers(Product product)
    {
        if (product != null && _model.RulersByProducts.ContainsKey(product))
        {
            DestroyRulers(product);
            _model.RulersByProducts.Remove(product);
        }
    }

    private void DestroyRulers(Product product)
    {
        foreach (var ruler in _model.RulersByProducts[product])
        {
            ruler.OnUpdateByTextInput -= OnUpdateRulerByTextInputHandler;
            ruler.Dispose();
            Destroy(ruler.gameObject);
        }
    }

    public void RemoveAllRulers()
    {
        foreach(var product in _model.RulersByProducts)
        {
            DestroyRulers(product.Key);
        }
        _model.RulersByProducts.Clear();
    }
}
