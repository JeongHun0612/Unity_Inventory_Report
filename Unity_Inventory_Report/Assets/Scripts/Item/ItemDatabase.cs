using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDatabase : MonoSingleton<ItemDatabase>
{
    private Dictionary<string, ItemData> _itemDict = new();
    private bool _isInitialize;

    public void Initialize()
    {
        if (_isInitialize)
            return;

        ItemData[] items = Resources.LoadAll<ItemData>("ScriptableObjects/ItemDatas");
        foreach (var item in items)
        {
            if (!_itemDict.ContainsKey(item.ItemID))
            {
                _itemDict.Add(item.ItemID, item);
            }
            else
            {
                Debug.LogWarning($"이미 로드된 데이터 : {item.ItemID}");
            }
        }

        _isInitialize = true;
        Debug.Log($"[ItemDatabase] 총 {_itemDict.Count}개 아이템 로드됨");
    }

    public ItemData GetItemData(string itemID)
    {
        if (_itemDict.TryGetValue(itemID, out var item))
        {
            return item;
        }

        Debug.LogWarning($"ItemDatabase에 해당 ID({itemID})의 아이템이 존재하지 않습니다.");
        return null;
    }

    public List<ItemData> GetAllItemDatas() => _itemDict.Values.ToList();
}
