using UnityEngine;
using UnityEngine.UI;

public sealed class GhostCubeView : MonoBehaviour
{
    public RectTransform Rect => (RectTransform)transform;
    public RectTransform ParentRectTransform { get; private set; }
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        ParentRectTransform = transform.parent.GetComponent<RectTransform>();
    }

    public void Setup(Sprite sprite)
    {
        _image.sprite = sprite;
    }
}