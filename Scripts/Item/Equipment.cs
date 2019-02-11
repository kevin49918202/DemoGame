using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equiptment" , menuName = "Inventory/Equipment")]
public class Equipment : Item {

    public EquipmentSlot EquipSlot;
    public SkinnedMeshRenderer mesh;
    public EquipmentMeshRagion[] coveredMeshRagions;

    public int armorModifier;
    public int damageModifier;

    //當在Inventory內被使用
    public override void Use()
    {
        base.Use();
        EquipmentManager.instance.Equip(this);
        RemoveFromInventory();
    }
}

public enum EquipmentSlot { Head, Chest, Legs, Weapon, Shield, Feet, };
public enum EquipmentMeshRagion { Legs, Arms, Torso }
