using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : CharacterCombat {

    private EnemyController controller;

    void Start()
    {
        myStats = GetComponent<CharacterStats>();
        controller = GetComponent<EnemyController>();
    }

    void Update()
    {
        if (targetStats != null)
        {
            if (Distance() < attackRadius)
                Attack();
            attackCooldown -= Time.deltaTime;
        }
    }

    protected override IEnumerator DoDamage(CharacterStats targetStats, float delay, int damage)
    {
        controller.agentLock = true;
        yield return new WaitForSeconds(delay);
        if (targetStats.currentHealth != 0 && myStats.currentHealth != 0)
        {
            targetStats.TakeDamage(damage);
        }
        else
        {
            UnTarget();
        }
        yield return new WaitForSeconds(delay);
        controller.agentLock = false;
    }
}
