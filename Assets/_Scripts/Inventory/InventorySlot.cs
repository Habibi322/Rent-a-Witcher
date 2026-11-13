using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text amountText;

    public bool IsEmpty => !itemIcon.gameObject.activeSelf;

    public void SetItem(Sprite icon, int amount)
    {
        itemIcon.gameObject.SetActive(true);
        amountText.gameObject.SetActive(true);

        itemIcon.sprite = icon;
        amountText.text = amount.ToString();
    }

    public void ClearSlot()
    {
        itemIcon.gameObject.SetActive(false);
        amountText.gameObject.SetActive(false);
    }
}