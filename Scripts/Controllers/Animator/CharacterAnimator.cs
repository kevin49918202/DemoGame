using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour {

    protected CharacterCombat combat;
    protected Animator animator;
    protected AnimatorStateInfo currentAnimStateInfo;
    protected AnimatorStateInfo nextAnimStateInfo;

    void Start () {
        animator = GetComponentInChildren<Animator>();
        combat = GetComponent<CharacterCombat>();
        combat.OnAttack += OnAttack;
	}
	
	void Update () {
        
    }

    protected virtual void OnAttack()
    {
        animator.SetTrigger("Attack");
    }
}
