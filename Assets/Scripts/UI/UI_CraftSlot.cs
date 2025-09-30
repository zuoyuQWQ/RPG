using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    protected override void Start()
    {
        base.Start();
    }
    public void SetupCraftSlot(ItemDataEquipment _data)
    {
        if (_data == null) return;
        
        item = new InventoryItem(_data);
        itemImage.sprite = _data.icon;
        itemText.text = _data.itemName;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.data == null)
            return;
        
        ui.craftWindow.SetupCraftWindow(item.data as ItemDataEquipment);
    }
}
