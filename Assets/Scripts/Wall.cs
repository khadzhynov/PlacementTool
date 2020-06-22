using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _renderer;

    private Vector3 _cornerBegin;
    private Vector3 _cornerEnd;

    public Vector3 CornerBegin { get => _cornerBegin; }
    public Vector3 CornerEnd { get => _cornerEnd; }

    public void Setup(Vector3 cornerBegin, Vector3 cornerEnd, float height, float width, Material material)
    {
        _cornerBegin = cornerBegin;
        _cornerEnd = cornerEnd;
        SetPosition(cornerBegin, cornerEnd, height, width);
        SetSize(cornerBegin, cornerEnd, height, width);
        SetOrientation(cornerBegin, cornerEnd);
        _renderer.material = material;
    }
    
    private void SetPosition(Vector3 cornerBegin, Vector3 cornerEnd, float height, float width)
    {
        Vector3 position = (cornerBegin + cornerEnd) / 2f;
        position += Vector3.Cross(cornerEnd - cornerBegin, Vector3.up).normalized * width / 2f;
        position += Vector3.up * height / 2f;
        transform.position = position;
    }

    private void SetSize(Vector3 cornerBegin, Vector3 cornerEnd, float height, float width)
    {
        transform.localScale = new Vector3(
            (cornerBegin - cornerEnd).magnitude,
            height,
            width);
    }

    private void SetOrientation(Vector3 cornerBegin, Vector3 cornerEnd)
    {
        transform.right = cornerBegin - cornerEnd;
    }
}
