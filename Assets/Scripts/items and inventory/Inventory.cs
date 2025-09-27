using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<ItemData> startingItems;

    public List<InventoryItem> equipment;
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDictiatiory;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictiatiory;

    public List<InventoryItem> stash;
    public Dictionary<ItemData,InventoryItem> stashDictiatiory;

    [Header("库存UI")]

    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSLot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] statSlot;

    [Header("物品使用cd")]
    [SerializeField] private float lastTimeUseFlask;
    [SerializeField] private float lastTimeUseAmor;

    private float flaskCooldown;
    private float amorCooldown;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Start()
    {


        equipment = new List<InventoryItem>();
        equipmentDictiatiory = new Dictionary<ItemDataEquipment, InventoryItem>();

        inventory = new List<InventoryItem>();
        inventoryDictiatiory = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictiatiory = new Dictionary<ItemData, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSLot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();

        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartingItems();
    }

    private void AddStartingItems()
    {
        for (int i = 0; i < startingItems.Count; i++)
        {
            AddItem(startingItems[i]);
        }
    }

    //提供装备物品的逻辑

    public void EquipItem(ItemData _item)
    {
        if (_item == null)
            return;

        ItemDataEquipment newEquipment = _item as ItemDataEquipment;
        if (newEquipment == null)
            return;

        ItemDataEquipment oldEquipment = null;

        // 查找是否有同类型的旧装备
        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictiatiory)
        {
            if (item.Key.equpmentType == newEquipment.equpmentType)
            {
                oldEquipment = item.Key;
                break; // 找到后即可退出循环
            }
        }

        // 如果有旧装备，先将其放回背包
        if (oldEquipment != null)
        {
            // 从装备字典和列表中移除旧装备
            equipmentDictiatiory.Remove(oldEquipment);
            equipment.Remove(equipment.Find(x => x.data == oldEquipment));
            oldEquipment.RemoveModfiers();

            // 将旧装备添加回背包
            AddToInventory(oldEquipment);
        }

        // 装备新物品
        InventoryItem newItem = new InventoryItem(newEquipment);
        equipment.Add(newItem);
        equipmentDictiatiory.Add(newEquipment, newItem);
        newEquipment.AddModfiers();

        // 从背包中移除新装备
        RemoveItem(_item);

        // 刷新UI
        UpdateSlotUI();

    }

    //提供取消装备物品的逻辑

    public void UnequipItem(ItemDataEquipment _itemToRemove)
    {
        if (_itemToRemove == null || !equipmentDictiatiory.ContainsKey(_itemToRemove))
            return;

        // 1. 从装备字典和列表中移除
        InventoryItem itemToUnequip = equipmentDictiatiory[_itemToRemove];
        equipment.Remove(itemToUnequip);
        equipmentDictiatiory.Remove(_itemToRemove);
        _itemToRemove.RemoveModfiers();

        // 2. 将物品添加回背包
        AddToInventory(_itemToRemove);

        // 3. 刷新UI
        UpdateSlotUI();
    }

    private void UpdateSlotUI()
    {
        // 更新装备槽UI
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            equipmentSlot[i].CleanUpSlot(); // 在更新前，先清空所有装备槽

            foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictiatiory)
            {
                if (item.Key.equpmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].UpdateSlot(item.Value);
                    break; // 找到匹配的就更新并跳出内层循环，提高效率
                }
            }
        }

        // 更新背包UI
        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        // 更新仓库UI
        for (int i = 0; i < stashItemSLot.Length; i++)
        {
            stashItemSLot[i].CleanUpSlot();
        }
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSLot[i].UpdateSlot(stash[i]);
        }

        // 更新角色属性UI
        for (int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdateStatValueUI();
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAddItem())
        {
            AddToInventory(_item);
        }
        else if(_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }
        UpdateSlotUI();
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictiatiory.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictiatiory.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        InventoryItem newItem = new InventoryItem(_item);
        inventory.Add(newItem);
    }

    public void RemoveItem(ItemData _item)
    {
        // 如果是装备类型，则直接在背包列表中查找并移除
        if (_item.itemType == ItemType.Equipment)
        {
            // 从后往前遍历以安全地移除列表项
            for (int i = inventory.Count - 1; i >= 0; i--)
            {
                if (inventory[i].data == _item)
                {
                    inventory.RemoveAt(i);
                    // 找到并移除一个后即可退出，因为装备是唯一的
                    break; 
                }
            }
        }
        if (inventoryDictiatiory.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictiatiory.Remove(_item);
            }
            else
                value.RemoveStack();
        }

        if(stashDictiatiory.TryGetValue(_item, out InventoryItem stashValue))
        {
            if(stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue); 
                stashDictiatiory.Remove(_item);
            }
            else
                stashValue.RemoveStack();
        }
        UpdateSlotUI();
    }

    public bool CanAddItem()
    {
        if(inventory.Count >= inventoryItemSlot.Length)
        {
            Debug.Log("背包已满");
            return false;
        }
        return true;
    }

    public bool CanCraft(ItemDataEquipment _itemToCraft,List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for(int i =0; i < _requiredMaterials.Count; i++)
        {
            if (stashDictiatiory.TryGetValue(_requiredMaterials[i].data,out InventoryItem stashValue))
            {
                if (stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    Debug.Log("材料不足");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("材料不足");
                return false;
            }
        }

        for(int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }
        AddItem(_itemToCraft);
        Debug.Log("合成物品: " + _itemToCraft.name);

        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stash;

    public ItemDataEquipment GetEquipment(EquipmentType _type)
    {
        ItemDataEquipment equipedItem = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictiatiory)
        {
            if (item.Key.equpmentType == _type)
                equipedItem = item.Key;
        }

        return equipedItem;
    }

    public void UseFlask()
    {
        ItemDataEquipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null)
            return;

        bool canUseFlask = Time.time > lastTimeUseFlask + flaskCooldown;

        if (canUseFlask)
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.Effect(null);
            lastTimeUseFlask = Time.time;
        }
        else
        {
            Debug.Log("药水冷却中");
        }
    }

    public bool CanUseAmor()
    {
        ItemDataEquipment currentAmor = GetEquipment(EquipmentType.Armor);

        if (Time.time > lastTimeUseAmor + amorCooldown)
        {
            amorCooldown = currentAmor.itemCooldown;
            lastTimeUseAmor = Time.time;
            return true;
        }
        Debug.Log("护甲冷却中");
        return false;
    }

}
