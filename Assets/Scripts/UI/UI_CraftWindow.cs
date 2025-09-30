using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private Image itemIcon;
    [SerializeReference] private Button craftButton;

    [SerializeField] private Image[] materialIcons;
    public void SetupCraftWindow(ItemDataEquipment _data)
    {
        craftButton.onClick.RemoveAllListeners();


        for (int i = 0; i < materialIcons.Length; i++)
        {
            materialIcons[i].color = Color.clear;
            materialIcons[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i = 0; i < _data.craftingMaterials.Count; i++)
        {
            if (_data.craftingMaterials.Count > materialIcons.Length)
                Debug.LogWarning("材料图标数量不足");

            materialIcons[i].sprite = _data.craftingMaterials[i].data.icon;
            materialIcons[i].color = Color.white;

            TextMeshProUGUI textMeshProUGUI = materialIcons[i].GetComponentInChildren<TextMeshProUGUI>();

            materialIcons[i].GetComponentInChildren<TextMeshProUGUI>().text = _data.craftingMaterials[i].stackSize.ToString();
            materialIcons[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }

        itemIcon.sprite = _data.icon;
        itemDescriptionText.text = _data.GetDescription();
        titleText.text = _data.name;

        craftButton.onClick.AddListener(() =>
        {
            Inventory.Instance.CanCraft(_data, _data.craftingMaterials);
        });
    }
}
