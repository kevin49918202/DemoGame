using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Interactable{

    public PlayerManager playerManager;
    CharacterCombat playerCombat;

    void Start()
    {
        playerManager = PlayerManager.instance;
        playerCombat = playerManager.player.GetComponent<CharacterCombat>();
    }

    public override void Interact()
    {
        base.Interact();
        if(Close())
            playerCombat.Attack();
    }

    public override void OnFocused(Transform playerTransform)
    {
        base.OnFocused(playerTransform);
        playerCombat.Target(transform);
    }

    public override void OnDefocused()
    {
        base.OnDefocused();
        playerCombat.UnTarget();
    }
}
