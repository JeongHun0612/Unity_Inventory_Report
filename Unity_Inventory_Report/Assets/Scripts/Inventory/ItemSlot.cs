using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _countText;

    private InventoryWindow _inventoryWindow;
    private int _slotIndex;

    public ItemInstance ItemInstance { get; private set; }

    public void Initialize(InventoryWindow inventoryWindow, int index)
    {
        _inventoryWindow = inventoryWindow;
        _slotIndex = index;
    }

    public void SetItemSlot(ItemInstance itemInstance)
    {
        if (itemInstance == null)
        {
            SlotClear();
            return;
        }

        ItemInstance = itemInstance;
        itemInstance.SlotIndex = _slotIndex;

        _countText.text = itemInstance.Count == 1 ? string.Empty : $"{itemInstance.Count}";
        _iconImage.sprite = itemInstance.ItemData.ItemIcon;
        _iconImage.color = Color.white;
    }

    public void SlotClear()
    {
        ItemInstance = null;

        _countText.text = string.Empty;

        _iconImage.sprite = null;
        _iconImage.color = Color.clear;
    }

    public void OnClickItemSlot()
    {
        if (ItemInstance == null)
            return;

        _inventoryWindow.ShowDescrptionPanel(ItemInstance);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ItemInstance == null)
            return;

        _inventoryWindow.ShowDragIcon(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _inventoryWindow.UpdateDragIconPosition(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _inventoryWindow.HideDragIcon();
    }

    public void OnDrop(PointerEventData eventData)
    {
        _inventoryWindow.SwapItems(this);
    }
}
