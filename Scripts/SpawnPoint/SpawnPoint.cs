using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

    public EnemyKind ek;
    public float respawnTime = 10;
    public float wanderRange = 7;
    public string kind;

    void Awake()
    {
        kind = ek.kind;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, 0.5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, wanderRange);
    }
}
