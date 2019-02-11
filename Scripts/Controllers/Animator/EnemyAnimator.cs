using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : CharacterAnimator
{

    NavMeshAgent agent;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        combat = GetComponent<CharacterCombat>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start () {
        combat.OnAttack += OnAttack;
    }
	
	void Update () {
        animator.SetFloat("Speed", agent.velocity.magnitude);
	}
}
