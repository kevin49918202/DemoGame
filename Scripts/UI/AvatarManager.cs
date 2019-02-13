using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarManager : MonoBehaviour
{
    public static AvatarManager instance { get { return m_Instance; } }
    static AvatarManager m_Instance;

    [SerializeField] AvatarUI playerAvatar;
    [SerializeField] AvatarUI targetAvatar;
    AvatarUI[] characterAvatars;
    void Awake()
    {
        m_Instance = this;
        characterAvatars = new AvatarUI[] { playerAvatar , targetAvatar };
    }
    void Start()
    {
        PlayerStats playerStats= PlayerManager.instance.player.GetComponent<PlayerStats>();
        playerAvatar.Target(playerStats);
    }

    public void UpdateAvatar(CharacterStats stats)
    {
        foreach(AvatarUI a in characterAvatars)
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
