using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ItemInstance
{
    public ItemData ItemData;
    public int SlotIndex;
    public int Count;

    public ItemInstance(ItemData data, int slotIndex, int count)
    {
        ItemData = data;
        SlotIndex = slotIndex;
        Count = count;
    }
}

public class Inventory : MonoSingleton<Inventory>
{
    private const string CSVPath = "CSV/Items/Inventory";
    private const int SLOT_MAX_COUNT = 15;

    private Dictionary<EItemType, List<ItemInstance>> _items = new()
    {
        { EItemType.Equipment, new List<ItemInstance>() },
        { EItemType.Consumable, new List<ItemInstance>() },
    };

    public string SavePath => Path.Combine(Application.dataPath, "Resources/CSV/Items/Inventory.csv");

    public void Initialize()
    {
        LoadInventoryInItem();
    }

    public void AddItem(string itemId, int count)
    {
        if (count <= 0)
        {
            Debug.LogWarning("Count가 0 이하의 숫자입니다.");
            return;
        }

        var itemData = ItemDatabase.Instance.GetItemData(itemId);
        if (itemData == null) return;

        if (_items.TryGetValue(itemData.ItemType, out var itemList))
        {
            int remaining = count;

            // 기존 데이터에 추가할 수 있는 공간이 있다면 먼저 채움
            foreach (var item in itemList)
            {
                if (item.ItemData.ItemID != itemId)
                    continue;

                int canAdd = itemData.MaxCount - item.Count;
                if (canAdd <= 0)
                    continue;

                int add = Mathf.Min(remaining, canAdd);
                item.Count += add;
                remaining -= add;

                if (remaining <= 0)
                    return;
            }

            // 남은 수량이 있다면 새로운 데이터를 만들어서 나눠 넣음
            while (remaining > 0)
            {
                if (itemList.Count == SLOT_MAX_COUNT)
                {
                    Debug.LogWarning($"{itemData.ItemType} 인벤토리의 자리가 부족합니다.");
                    return;
                }

                int add = Mathf.Min(remaining, itemData.MaxCount);
                int nextIndex = GetAvailableIndex(itemList);

                itemList.Add(new ItemInstance(itemData, nextIndex, add));
                remaining -= add;
            }
        }
    }

    public bool RemoveItem(ItemInstance targetItem)
    {
        if (targetItem == null)
            return false;

        EItemType type = targetItem.ItemData.ItemType;

        if (!_items.TryGetValue(type, out var itemList))
            return false;

        return itemList.Remove(targetItem);
    }

    public List<ItemInstance> GetItemInstances(EItemType itemType)
    {
        if (_items.TryGetValue(itemType, out var itemList))
        {
            return itemList;
        }

        return null;
    }

    public int GetAvailableIndex(List<ItemInstance> itemList)
    {
        HashSet<int> usedIndexes = new();

        // 현재 아이템들의 인덱스를 수집
        foreach (var item in itemList)
        {
            usedIndexes.Add(item.SlotIndex);
        }

        // 비어있는 가장 작은 인덱스를 찾음
        int index = 0;
        while (true)
        {
            if (!usedIndexes.Contains(index))
                return index;
            index++;
        }
    }

    public void CompressSlotIndices(EItemType itemType)
    {
        if (!_items.TryGetValue(itemType, out var itemList))
            return;

        // SlotIndex 기준으로 정렬
        itemList.Sort((a, b) => a.SlotIndex.CompareTo(b.SlotIndex));

        // SlotIndex 0부터 재할당
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].SlotIndex = i;
        }
    }

    private void LoadInventoryInItem()
    {
        var dataList = CSVService.Read(CSVPath);

        if (dataList == null)
            return;

        foreach (var entry in dataList)
        {
            string id = entry["ItemID"].ToString();
            int slotIndex = int.Parse(entry["SlotIndex"].ToString());
            int count = int.Parse(entry["Count"].ToString());
            EItemType type = (EItemType)Enum.Parse(typeof(EItemType), entry["ItemType"].ToString());

            var itemData = ItemDatabase.Instance.GetItemData(id);

            if (itemData == null) return;

            if (_items.TryGetValue(type, out var list))
            {
                list.Add(new ItemInstance(itemData, slotIndex, count));
            }
        }
    }

    private void SaveInventoryToCSV()
    {
        List<string> lines = new() { "ItemID,ItemType,SlotIndex,Count" };

        foreach (var kvp in _items)
        {
            var type = kvp.Key;
            var list = kvp.Value;

            foreach (var item in list)
            {
                string line = $"{item.ItemData.ItemID},{item.ItemData.ItemType},{item.SlotIndex},{item.Count}";
                lines.Add(line);
            }
        }

        File.WriteAllLines(SavePath, lines);
        Debug.Log($"[Inventory] 저장 완료: {SavePath}");
    }

    private void OnDestroy()
    {
        SaveInventoryToCSV();
    }
}