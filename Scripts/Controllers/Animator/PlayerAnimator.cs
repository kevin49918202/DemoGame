using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : CharacterAnimator {

    public bool bMove, bJump;
    public bool bGround;
    public float fVertical, fHorizontal, fJumpVertical, fJumpHorizontal, fMoveVertical, fMoveHorizontal;
    public int iAnimAngle;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        combat = GetComponent<CharacterCombat>();
        //combat.OnAttack += OnAttack;
    }

    void Update()
    {
        currentAnimStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        nextAnimStateInfo = animator.GetNextAnimatorStateInfo(0);

        animator.SetBool("Move", bMove);
        animator.SetBool("Ground", bGround);
        animator.SetFloat("JumpVertical", fJumpVertical);
        animator.SetFloat("JumpHorizontal", fJumpHorizontal);
        animator.SetInteger("Angle", iAnimAngle);
        animator.SetFloat("Vertical", fVertical);
        animator.SetFloat("Horizontal", fHorizontal);

        if (Input.GetKeyDown(KeyCode.F))
        {
            Attack();
        }

        if (bGround)
        {
            animator.SetFloat("MoveVertical", fMoveVertical);
            animator.SetFloat("MoveHorizontal", fMoveHorizontal);
        }

        if (bJump)
        {
            Jump();
            bJump = false;
        }
    }

    void Jump()
    {
        if (!currentAnimStateInfo.IsName("Attack2H [79]") && !nextAnimStateInfo.IsName("Attack2H [79]"))
        {
            animator.SetTrigger("Jump_Upbody");
        }
        animator.SetTrigger("Jump_Lowerbody");
    }

    void Attack()
    {
        if (!currentAnimStateInfo.IsName("Attack2H [79]") && !nextAnimStateInfo.IsName("Attack2H [79]"))
        {
            animator.SetTrigger("Attack");
        }  
    }
}
