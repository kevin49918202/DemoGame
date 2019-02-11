using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAvatar : GameAvatar {

    public GameObject player;

    void Start()
    {
        targetStats = player.GetComponent<PlayerStats>();
        targetName.text = targetStats.characterName;
    }

    void Update () {
        UpdateHP();
    }
}
