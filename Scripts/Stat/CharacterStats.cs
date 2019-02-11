using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {

    public string characterName;
    public int maxHealth = 100;
    public int currentHealth { get; protected set; }

    public int floatingTextType;
    public Stat damage;
    public Stat armor;
    
    void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;
        //Debug.Log(transform.name + " takes " + damage + " damage.");
        FloatingTextController.instance.CreatFloatingText(damage.ToString(), transform, floatingTextType);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public virtual void Die()
    {
        GetComponentInChildren<Animator>().SetBool("Death", true);
    }
}
