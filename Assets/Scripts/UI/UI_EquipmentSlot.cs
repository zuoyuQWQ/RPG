using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "装备类型"+ slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        // 如果槽位是空的，不执行任何操作
        if (item == null || item.data == null)
            return;

        // 调用Inventory的UnequipItem方法，将装备卸下并放回背包
        Inventory.Instance.UnequipItem(item.data as ItemDataEquipment);

        ui.itemToolTip.HideToolTip();
        
    }
}
