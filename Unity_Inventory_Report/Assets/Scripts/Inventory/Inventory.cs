using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Serializable]
    public class ItemInstance
    {
        public ItemData ItemData;
        public int Count;

        public ItemInstance(ItemData data, int count)
        {
            ItemData = data;
            Count = count;
        }

        public override string ToString()
        {
            return $"{ItemData.ItemName} (x{Count}) - {ItemData.ItemRarity}, {ItemData.ItemCode}";
        }
    }

    [SerializeField] private string _csvPath = "CSV/Items/Inventory";

    private Dictionary<EItemType, List<ItemInstance>> _items = new()
    {
        { EItemType.Equipment, new List<ItemInstance>() },
        { EItemType.Consumable, new List<ItemInstance>() },
    };

    public string SavePath => Path.Combine(Application.dataPath, "Resources/CSV/Items/Inventory.csv");


    private void Start()
    {
        LoadInventoryInItem();

        PrintInventory();

        AddItem("potion_001", 5);
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

        if (_items.TryGetValue(itemData.ItemType, out var list))
        {
            var item = list.Find(i => i.ItemData.ItemID == itemId);

            if (item != null)
            {
                item.Count += count;
            }
            else
            {
                list.Add(new ItemInstance(itemData, count));
            }
        }
    }

    public void PrintInventory()
    {
        Debug.Log("=== [인벤토리 출력] ===");

        foreach (var kvp in _items)
        {
            Debug.Log($"--- {kvp.Key} ---");

            if (kvp.Value.Count == 0)
            {
                Debug.Log("비어 있음");
                continue;
            }

            foreach (var item in kvp.Value)
            {
                Debug.Log(
                    $"{item.ItemData.Print()} \n\n" +
                    $"Count : {item.Count}");
            }
        }
    }


    private void LoadInventoryInItem()
    {
        var dataList = CSVService.Read(_csvPath);

        foreach (var entry in dataList)
        {
            string id = entry["ItemID"].ToString();
            int count = int.Parse(entry["Count"].ToString());
            EItemType type = (EItemType)Enum.Parse(typeof(EItemType), entry["ItemType"].ToString());

            var itemData = ItemDatabase.Instance.GetItemData(id);

            if (itemData == null) return;

            if (_items.TryGetValue(type, out var list))
            {
                list.Add(new ItemInstance(itemData, count));
            }
        }
    }

    private void SaveInventoryToCSV()
    {
        List<string> lines = new() { "ItemID,ItemType,Count" };

        foreach (var kvp in _items)
        {
            var type = kvp.Key;
            var list = kvp.Value;

            foreach (var item in list)
            {
                string line = $"{item.ItemData.ItemID},{item.ItemData.ItemType},{item.Count}";
                lines.Add(line);
            }
        }

        File.WriteAllLines(SavePath, lines);
        Debug.Log($"[Inventory] 저장 완료: {SavePath}");
    }


    private void OnApplicationQuit()
    {
        SaveInventoryToCSV();
    }
}