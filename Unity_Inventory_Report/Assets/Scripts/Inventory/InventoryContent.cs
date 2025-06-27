using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryContent : MonoBehaviour
{
    [SerializeField] private List<ItemSlot> _itemSlots;

    public void Initialize(InventoryWindow inventoryWindow)
    {
        for (int i = 0; i < _itemSlots.Count; i++)
        {
            int index = i;
            _itemSlots[index].Initialize(inventoryWindow, index);
        }
    }

    public void SetItemSlots(List<ItemInstance> itemList)
    {
        foreach (var itemSlot in _itemSlots)
        {
            itemSlot.SlotClear();
        }

        foreach (var itemInstance in itemList)
        {
            int slotIndex = itemInstance.SlotIndex;
            _itemSlots[slotIndex].SetItemSlot(itemInstance);
        }
    }

}
