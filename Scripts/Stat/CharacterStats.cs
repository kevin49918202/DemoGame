using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {

    public string characterName;

    public int maxHealth { get { return m_maxHealth; } }
    public int currentHealth { get { return m_currentHealth; } }
    protected int m_maxHealth = 100;
    protected int m_currentHealth;
    
    public int maxMana { get { return m_maxMana; } }
    public int currentMana { get { return m_currentMana; } }
    protected int m_maxMana = 100;
    protected int m_currentMana;

    public int floatingTextType;
    public Stat damage;
    public Stat armor;
    
    void Awake()
    {
        m_currentHealth = m_maxHealth;
        m_currentMana = m_maxMana;
    }

    public virtual void TakeDamage(int damage)
    {
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        m_currentHealth -= damage;
        //Debug.Log(transform.name + " takes " + damage + " damage.");
        FloatingTextController.instance.CreatFloatingText(damage.ToString(), transform, floatingTextType);
        AvatarManager.instance.UpdateAvatar(this);

        if (m_currentHealth <= 0)
        {
            m_currentHealth = 0;
            Die();
        }
    }

    public virtual void Die()
    {
        GetComponentInChildren<Animator>().SetBool("Death", true);
    }
}
