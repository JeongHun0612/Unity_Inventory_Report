using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWindow : MonoBehaviour
{
    [Header("## Inventory Panel")]
    [SerializeField] private RectTransform _inventoryPanelRect;
    public Ease _debugEase = Ease.OutExpo;

    [Header("## Content")]
    [SerializeField] private InventoryContent _content;

    [Header("## Description Panel")]
    [SerializeField] private ItemDescriptionPanel _itemDescriptionPanel;

    [Header("## Drag Icon")]
    [SerializeField] private Image _dragIcon;

    [Header("## TabButtons")]
    [SerializeField] private ItemTabButton[] _tabButtons;

    [Header("## TabButtons")]
    [SerializeField] private ThreshButton _threashButton;


    private ItemTabButton _selectedTab;
    private ItemSlot _draggingSlot;
    private EItemType _selectedItemType;

    public void Initialize()
    {
        _selectedItemType = EItemType.None;

        _content.Initialize(this);
        _threashButton.Initialize(this);

        gameObject.SetActive(false);
    }

    public void OnShow()
    {
        // X : -1660 , Y : 940

        // MoveX
        //_inventoryPanelRect.localPosition = new Vector3(-1660f, _inventoryPanelRect.localPosition.y, _inventoryPanelRect.localPosition.z);
        //_inventoryPanelRect.DOLocalMoveX(0f, 0.5f).SetEase(_debugEase);

        // MoveY
        _inventoryPanelRect.localPosition = new Vector3(_inventoryPanelRect.localPosition.x, 940f, _inventoryPanelRect.localPosition.z);
        _inventoryPanelRect.DOLocalMoveY(0f, 0.5f).SetEase(_debugEase);

        gameObject.SetActive(true);

        _itemDescriptionPanel.Hide();
        _threashButton.Hide();

        SelectedContent(EItemType.Equipment);
    }

    public void OnHide()
    {
        gameObject.SetActive(false);
    }

    public void ShowDragIcon(ItemSlot itemSlot)
    {
        _draggingSlot = itemSlot;
        _dragIcon.sprite = itemSlot.ItemInstance.ItemData.ItemIcon;
        _dragIcon.gameObject.SetActive(true);

        _threashButton.Show();
    }

    public void UpdateDragIconPosition(Vector2 pos)
    {
        _dragIcon.transform.position = pos;
    }

    public void HideDragIcon()
    {
        _dragIcon.gameObject.SetActive(false);
        _draggingSlot = null;

        _threashButton.Hide();
    }

    public void SwapItems(ItemSlot targetSlot)
    {
        if (_draggingSlot == null || _draggingSlot == targetSlot)
            return;

        var tempItemInstance = _draggingSlot.ItemInstance;
        _draggingSlot.SetItemSlot(targetSlot.ItemInstance);
        targetSlot.SetItemSlot(tempItemInstance);
    }

    public void RemoveDraggingItem()
    {
        if (_draggingSlot == null)
            return;

        Inventory.Instance.RemoveItem(_draggingSlot.ItemInstance);
        _draggingSlot.SlotClear();
    }

    public void ShowDescrptionPanel(ItemInstance itemInstance)
    {
        _itemDescriptionPanel.Show(itemInstance);
    }

    private void SelectedContent(EItemType itemType)
    {
        if (_selectedItemType == itemType)
            return;

        if (_selectedTab != null)
        {
            _selectedTab.OnSelected(false);
        }

        _selectedTab = _tabButtons[(int)itemType];
        _selectedTab.OnSelected(true);

        _selectedItemType = itemType;

        // 아이템 슬롯 할당
        SetContentItems(itemType);

        // 아이템 설명 패널 Hide
        _itemDescriptionPanel.Hide();
    }

    private void SetContentItems(EItemType itemType)
    {
        var itemList = Inventory.Instance.GetItemInstances(itemType);
        _content.SetItemSlots(itemList);
    }

    public void OnClickCompressButton()
    {
        if (_selectedItemType == EItemType.None)
            return;

        Inventory.Instance.CompressSlotIndices(_selectedItemType);
        SetContentItems(_selectedItemType);
    }

    [VisibleEnum(typeof(EItemType))]
    public void OnClickItemTab(int itemType)
    {
        if (itemType < 0 || itemType > _tabButtons.Length - 1)
            return;

        EItemType eitemType = (EItemType)itemType;
        SelectedContent(eitemType);
    }



    #region Debug AddItem Button Event
    public void OnClickAddChest()
    {
        Inventory.Instance.AddItem("chest_001", 1);
        SetContentItems(EItemType.Equipment);
    }
    public void OnClickAddWeapon1()
    {
        Inventory.Instance.AddItem("weapon_001", 1);
        SetContentItems(EItemType.Equipment);
    }
    public void OnClickAddWeapon2()
    {
        Inventory.Instance.AddItem("weapon_002", 3);
        SetContentItems(EItemType.Equipment);
    }
    public void OnClickAddPotion1()
    {
        Inventory.Instance.AddItem("potion_001", 15);
        SetContentItems(EItemType.Consumable);
    }
    public void OnClickAddPotion4()
    {
        Inventory.Instance.AddItem("potion_004", 20);
        SetContentItems(EItemType.Consumable);
    }

    #endregion
}
