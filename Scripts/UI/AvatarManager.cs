using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarManager : MonoBehaviour
{
    public static AvatarManager instance { get { return m_Instance; } }
    static AvatarManager m_Instance;

    [SerializeField] CharacterAvatar playerAvatar;
    [SerializeField] CharacterAvatar targetAvatar;
    CharacterAvatar[] characterAvatars;
    void Awake()
    {
        m_Instance = this;
        characterAvatars = new CharacterAvatar[] { playerAvatar , targetAvatar };
    }
    void Start()
    {
        PlayerStats playerStats= PlayerManager.instance.player.GetComponent<PlayerStats>();
        playerAvatar.Target(playerStats);
    }

    public void UpdateAvatar(CharacterStats stats)
    {
        foreach(CharacterAvatar a in characterAvatars)
        {
            if(stats == a.targetStats)
            {
                a.OnUpdate();
            }
        }
    }

    public void Target(GameObject go)
    {
        CharacterStats stats = go.GetComponent<CharacterStats>();
        targetAvatar.Target(stats);
    }
    public void UnTarget(GameObject go)
    {
        CharacterStats stats = go.GetComponent<CharacterStats>();
        targetAvatar.UnTarget(stats);
    }
}
