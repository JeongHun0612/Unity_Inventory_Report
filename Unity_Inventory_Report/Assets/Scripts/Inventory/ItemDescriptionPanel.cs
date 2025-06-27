using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionPanel : MonoBehaviour
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TMP_Text _descriptionText;

    public void Show(ItemInstance itemInstance)
    {
        _itemIcon.sprite = itemInstance.ItemData.ItemIcon;
        _descriptionText.text = itemInstance.ItemData.Description;
        gameObject.SetActive(true);
    }

    public void Hide() => gameObject.SetActive(false);
}
