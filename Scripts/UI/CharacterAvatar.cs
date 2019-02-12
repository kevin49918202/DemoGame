using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterAvatar : MonoBehaviour {

    public GameObject targetAvatar;
    CharacterStats m_targetStats;
    public CharacterStats targetStats { get { return m_targetStats; } }

    public TextMeshProUGUI text_HP;
    public Image image_HP;
    protected float m_MaxHp;
    protected float m_CurrentHP;

    public TextMeshProUGUI text_PB;
    public Image image_PB;
    float m_maxMp;
    float m_currentMp;

    public TextMeshProUGUI text_Name;

    public void OnUpdate()
    {
        m_MaxHp = m_targetStats.maxHealth;
        m_CurrentHP = m_targetStats.currentHealth;
        m_maxMp = m_targetStats.maxMana;
        m_currentMp = m_targetStats.currentMana;

        image_HP.fillAmount = m_CurrentHP / m_MaxHp;
        text_HP.text = (int)(m_CurrentHP / m_MaxHp * 100) + "%";

        image_PB.fillAmount = m_currentMp / m_maxMp;
        text_PB.text = (int)(m_currentMp / m_maxMp * 100) + "%";
    }

    public void Target(CharacterStats stats)
    {
        if (stats != null && stats != m_targetStats)
        {
            m_targetStats = stats;
            text_Name.text = m_targetStats.characterName;
            OnUpdate();
            targetAvatar.SetActive(true);
        }
    }

    public void UnTarget(CharacterStats stats)
    {
        if(stats == m_targetStats)
        {
            m_targetStats = null;
            targetAvatar.SetActive(false);
        }
    }
}
