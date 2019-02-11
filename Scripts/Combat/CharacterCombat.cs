using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour {

    public CharacterStats targetStats;
    protected CharacterStats myStats;

    public event System.Action OnAttack;

    public float attackSpeed = 1f;
    public float attackDelay = .6f;
    protected float attackCooldown = 0f;
    protected float attackRadius = 2f;

    protected float leaveCombatTime = 0;
    protected bool inCombat = false;

    void Start()
    {
        myStats = GetComponent<CharacterStats>();
    }

    void Update()
    {
        if (targetStats != null)
        {
            if(Distance() < attackRadius)
                Attack();
            attackCooldown -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        if (attackCooldown <= 0)
        {
            if (targetStats.currentHealth != 0 && myStats.currentHealth != 0)
            {
                attackCooldown = attackSpeed;
                StartCoroutine(DoDamage(targetStats, attackDelay, myStats.damage.GetValue()));
                inCombat = true;

                if (OnAttack != null)
                    OnAttack();
            }
            else
            {
                UnTarget();
            }  
        }
    }

    public void Target(Transform transform)
    {
        targetStats = transform.GetComponent<CharacterStats>();
    }

    public void UnTarget()
    {
        targetStats = null;
        inCombat = false;
    }

    protected virtual IEnumerator DoDamage (CharacterStats targetStats, float delay, int damage)
    {
        yield return new WaitForSeconds(delay);
        if(targetStats.currentHealth != 0 && myStats.currentHealth != 0)
        {
            targetStats.TakeDamage(damage);
        }
        else
        {
            UnTarget();
        }
    }

    protected float Distance()
    {
        return Vector3.Distance(targetStats.transform.position, transform.position);
    }
}
