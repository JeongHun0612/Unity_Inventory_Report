using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private InventoryWindow _inventoryWindow;

    private bool _isInventory;


    protected override void Awake()
    {
        base.Awake();

        ItemDatabase.Instance.Initialize();
        Inventory.Instance.Initialize();
    }
    private void Start()
    {
        // UI
        _inventoryWindow.Initialize();

        _isInventory = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            _isInventory = !_isInventory;

            if (_isInventory)
            {
                _inventoryWindow.OnShow();
            }
            else
            {
                _inventoryWindow.OnHide();
            }
        }
    }
}
