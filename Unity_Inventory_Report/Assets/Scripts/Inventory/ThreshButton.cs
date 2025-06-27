using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ThreshButton : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform _rect;

    private InventoryWindow _inventoryWindow;

    public void Initialize(InventoryWindow inventoryWindow)
    {
        _inventoryWindow = inventoryWindow;
    }

    public void Show()
    {
        _rect.localScale = Vector3.one;
        gameObject.SetActive(true);
    }

    public void Hide() => gameObject.SetActive(false);


    public void OnDrop(PointerEventData eventData)
    {
        _inventoryWindow.RemoveDraggingItem();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        _rect.DOScale(Vector3.one * 1.2f, 0.3f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _rect.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }
}
