using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats {

    public override void Die()
    {
        base.Die();

        enemyObjectManage.instance.StartEnemyDead(this.gameObject, 2, 2);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    public void ResetStats()
    {
        currentHealth = maxHealth;
    }
}
