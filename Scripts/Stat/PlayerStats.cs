using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats {

    void Start () {
        //EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
	}

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if(newItem != null)
        {
            damage.AddModifier(newItem.damageModifier);
            armor.AddModifier(newItem.armorModifier);
        }

        if(oldItem != null)
        {
            damage.RemoveModifier(oldItem.damageModifier);
            armor.RemoveModifier(oldItem.armorModifier);
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        base.Die();
        PlayerMotor.instance.inputLock = true;
    }
}
