using UnityEngine;

public abstract class RectTransformBehaviour : MonoBehaviour
{
    private RectTransform _rectTransform;
    protected RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = transform as RectTransform;
            }
            return _rectTransform;
        }
    }
}
