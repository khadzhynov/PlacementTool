using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlacementModel", menuName = "ScriptableObjects/Placement Model", order = 0)]
public class PlacementModel : ScriptableObject
{
    public Product SelectedProduct;
    
    public Product CurrnetProductInstance;

    public List<Product> PlacedProducts = new List<Product>();
}