using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "装备数据", menuName = "Data/Equipment")]

public class ItemDataEquipment : ItemData
{
    public EquipmentType equpmentType;

    public float itemCooldown;

    public ItemEffect[] itemEffects;

    [Header("玩家属性")]
    public int strength;//力量,影响物理伤害
    public int agility;//敏捷,影响暴击率和闪避率
    public int intelligence;//智力,影响法术伤害
    public int vitality;//生命值,影响最大生命

    [Header("武器属性")]
    public int damage;
    public int critChance;
    public int critPower;


    [Header("护甲属性")]
    public int maxHealth;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("元素属性")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;

    private int descriptionLength;//物品描述长度
    public void Effect(Transform _enemyPosition)
    {
        foreach(var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }
    public void AddModfiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(maxHealth);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);
    }

    public void RemoveModfiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(maxHealth);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;

        AddItemDescription(strength, "力量");
        AddItemDescription(agility, "敏捷");
        AddItemDescription(intelligence, "智力");
        AddItemDescription(vitality, "生命值");
        AddItemDescription(damage,"伤害");
        AddItemDescription(critChance,"暴击率");
        AddItemDescription(critPower,"暴击伤害");
        AddItemDescription(maxHealth,"最大生命值");
        AddItemDescription(armor,"护甲");
        AddItemDescription(evasion,"闪避率");
        AddItemDescription(magicResistance,"魔法抗性");
        AddItemDescription(fireDamage,"火焰伤害");
        AddItemDescription(iceDamage,"冰霜伤害");
        AddItemDescription(lightingDamage,"雷电伤害 ");

        if(descriptionLength < 5)
        {
            for(int i = 0; i < 5 - descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }
                                                   
        return sb.ToString();
    }

    public void AddItemDescription(int _value, string _name)
    {
        if(_value != 0)
        {
            if(sb.Length > 0)
            {
                sb.AppendLine();
            }
            if(_value > 0)
            {
                sb.Append(_name + ":" + _value);
            }

            descriptionLength++;
        }
    }
}
