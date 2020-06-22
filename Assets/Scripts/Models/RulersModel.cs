using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rulers Model", menuName = "ScriptableObjects/Rulers Model", order = 0)]
public class RulersModel : ScriptableObject
{
    [SerializeField]
    private Ruler _rulerPrefab;
    public Ruler RulerPrefab { get => _rulerPrefab; }
    public Dictionary<Product, List<Ruler>> RulersByProducts { get; } = new Dictionary<Product, List<Ruler>>();
}
