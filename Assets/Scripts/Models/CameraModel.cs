using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Camera Model", menuName = "ScriptableObjects/Camera Model", order = 0)]
public class CameraModel : ScriptableObject
{
    [SerializeField]
    private float _sensetivity = 0.1f;

    private float _verticalRotation;
    private float _horizontalRotation;

    private bool _isDrag;

    public float Sensetivity { get => _sensetivity; }
    public float VerticalRotation { get => _verticalRotation; set => _verticalRotation = value; }
    public float HorizontalRotation { get => _horizontalRotation; set => _horizontalRotation = value; }
    public bool IsDrag { get => _isDrag; set => _isDrag = value; }
}
