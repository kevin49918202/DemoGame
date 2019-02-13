using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AvatarUI : MonoBehaviour {

    [SerializeField]GameObject targetAvatar;
    CharacterStats m_targetStats;
    public CharacterStats targetStats { get { return m_targetStats; } }

    [SerializeField]TextMeshProUGUI text_HP;
    [SerializeField]Image image_HP;
    float m_maxHp;
    float m_currentHP;

    [SerializeField] TextMeshProUGUI text_PB;
    [SerializeField] Image image_PB;
    float m_maxMp;
    float m_currentMp;

    public TextMeshProUGUI text_Name;

    public void OnUpdate()
    {
        m_maxHp = m_targetStats.maxHealth;
        m_currentHP = m_targetStats.currentHealth;
        m_maxMp = m_targetStats.maxMana;
        m_currentMp = m_targetStats.currentMana;

        image_HP.fillAmount = m_currentHP / m_maxHp;
        text_HP.text = (int)(m_currentHP / m_maxHp * 100) + "%";

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
