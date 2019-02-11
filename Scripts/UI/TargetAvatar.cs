using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetAvatar : GameAvatar {

    public static TargetAvatar instance;
    public GameObject targetAvatar;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        UpdateHP();
    }

    public void Target(CharacterStats target)
    {
        if (target != null)
        {
            targetStats = target;
            targetName.text = targetStats.characterName;
            targetAvatar.SetActive(true);
        }
    }

    public void UnTarget()
    {
        targetStats = null;
        targetAvatar.SetActive(false);
    }
}
