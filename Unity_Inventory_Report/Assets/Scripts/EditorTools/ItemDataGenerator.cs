using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ItemDataGenerator : EditorWindow
{
    private string csvPath = "CSV/Items/ItemData";
    private string outputFolder = "Assets/Resources/ScriptableObjects/ItemDatas";

    [MenuItem("Tools/CSV → ItemData 생성기")]
    public static void ShowWindow()
    {
        GetWindow<ItemDataGenerator>("ItemData Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("CSV to ScriptableObject 생성기", EditorStyles.boldLabel);
        csvPath = EditorGUILayout.TextField("CSV 경로", csvPath);
        outputFolder = EditorGUILayout.TextField("생성 폴더", outputFolder);

        if (GUILayout.Button("ItemData 생성"))
        {
            GenerateItemSOs();
        }
    }

    private void GenerateItemSOs()
    {
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        var dataList = CSVService.Read(csvPath);

        if (dataList == null) return;

        foreach (var entry in dataList)
        {
            string id = entry["ItemID"].ToString();
            string name = entry["ItemName"].ToString();
            EItemType type = (EItemType)Enum.Parse(typeof(EItemType), entry["ItemType"].ToString());
            EItemRarity rarity = !string.IsNullOrEmpty(entry["ItemRarity"].ToString()) ? (EItemRarity)Enum.Parse(typeof(EItemRarity), entry["ItemRarity"].ToString()) : EItemRarity.None;
            EItemCode code = !string.IsNullOrEmpty(entry["ItemCode"].ToString()) ? (EItemCode)Enum.Parse(typeof(EItemCode), entry["ItemCode"].ToString()) : EItemCode.None;
            int requiredLevel = !string.IsNullOrEmpty(entry["RequiredLevel"].ToString()) ? int.Parse(entry["RequiredLevel"].ToString()) : 0;
            int maxCount = !string.IsNullOrEmpty(entry["MaxCount"].ToString()) ? int.Parse(entry["MaxCount"].ToString()) : 0;
            string description = entry["Description"].ToString();
            string iconName = entry["ItemIcon"].ToString();

            ItemData item = ScriptableObject.CreateInstance<ItemData>();
            item.ItemID = id;
            item.ItemName = name;
            item.ItemType = type;
            item.ItemRarity = rarity;
            item.ItemCode = code;
            item.RequiredLevel = requiredLevel;
            item.MaxCount = maxCount;
            item.Description = description;

            Sprite icon = Resources.Load<Sprite>($"Sprites/ItemIcon/{iconName}");
            if (icon == null)
            {
                Debug.LogWarning($"아이콘 '{iconName}' 을 찾을 수 없습니다.");
            }
            item.ItemIcon = icon;

            string assetPath = $"{outputFolder}/{id}.asset";
            AssetDatabase.CreateAsset(item, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("ItemData 생성 완료");
    }
}
