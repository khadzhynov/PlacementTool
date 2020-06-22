using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Products Collection", menuName = "ScriptableObjects/Products Collection", order = 0)]
public class ProductsCollection : ScriptableObject
{
    [SerializeField]
    private List<Product> _productsList;

    public IReadOnlyList<Product> ProductsList { get => _productsList; }
}
