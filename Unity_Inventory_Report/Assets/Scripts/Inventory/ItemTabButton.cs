using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ItemTabButton : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Image _image;

    public void OnSelected(bool isSelect)
    {
        Vector3 to = (isSelect) ? Vector3.one * 1.15f : Vector3.one;
        _rect.DOScale(to, 0.3f).SetEase(Ease.OutBack);

        _image.color = (isSelect) ? new Color(0.7f, 0.9f, 1f) : Color.white;
    }
}
