using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyObjectManage : MonoBehaviour {

    class spawnPointInfo
    {
        public string kind;
        public SpawnPoint sp;
    }

    public static enemyObjectManage instance;
    public SpawnPoint[] spawnPoints;
    public EnemyKind[] enemyKinds;
    Dictionary<GameObject, spawnPointInfo> dict_SpawnPointOfGameObject;

    void Awake()
    {
        instance = this;
        dict_SpawnPointOfGameObject = new Dictionary<GameObject, spawnPointInfo>();
    }

    void Start () {
        InitEnemyObject(enemyKinds);
        EnemySpawn(spawnPoints);
    }

    void InitEnemyObject(EnemyKind[] enemyKinds)
    {
        foreach (EnemyKind ek in enemyKinds)
        {
            Object m_NPC = Resources.Load(ek.kind);
            ObjectPool.m_Instance.InitObjectsInPool(m_NPC, ek.kind, 10);
        }
    }

    void EnemySpawn(SpawnPoint[] spawnPoints)
    {
        foreach (SpawnPoint sp in spawnPoints)
        {
            EnemyRespawn(sp);
        }
    }
	
    void EnemyRespawn(SpawnPoint sp)
    {
        GameObject go = ObjectPool.m_Instance.LoadObjectFromPool(sp.kind);
        go.GetComponent<EnemyController>().Respawn(sp);

        spawnPointInfo newSp = new spawnPointInfo();
        newSp.sp = sp;
        newSp.kind = sp.kind;
        dict_SpawnPointOfGameObject.Add(go, newSp);
    }

    public void StartEnemyDead(GameObject go, float disappearTime, float respawnTime)
    {
        if (dict_SpawnPointOfGameObject.ContainsKey(go))
        {
            string kind = dict_SpawnPointOfGameObject[go].kind;
            StartCoroutine(EnemyDead(kind, go, disappearTime, respawnTime));
        }
        else
        {
            Debug.Log("Not found go in dictionary");
        }
    }

    IEnumerator EnemyDead(string kind, GameObject go, float disappearTime, float respawnTime)
    {
        SpawnPoint sp = dict_SpawnPointOfGameObject[go].sp;
        dict_SpawnPointOfGameObject.Remove(go);

        yield return new WaitForSeconds(disappearTime);
        ObjectPool.m_Instance.UnLoadObjectToPool(kind, go);
        AvatarManager.instance.UnTarget(go);

        yield return new WaitForSeconds(respawnTime);
        EnemyRespawn(sp);
    }
}
