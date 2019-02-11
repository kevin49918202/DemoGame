using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameAvatar : MonoBehaviour {

    public Text targetName;
    public Slider hp;
    public float maxHealth;
    [SerializeField]protected float currentHealth;
    protected CharacterStats targetStats;

    protected void UpdateHP()
    {
        if(targetStats != null)
        {
            maxHealth = targetStats.maxHealth;
            currentHealth = targetStats.currentHealth;
            hp.value = Mathf.Lerp(hp.value, currentHealth / maxHealth, 0.3f);
        }
    }
}
