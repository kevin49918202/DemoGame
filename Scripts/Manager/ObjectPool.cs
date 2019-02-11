using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {

	public class ObjectPoolData
	{
		public GameObject m_go;
		public bool m_bUsing;
	}

	public int[]  m_iCount;
	public static ObjectPool m_Instance;
	public List<ObjectPoolData>[] m_GameObjects;
	public int m_iNumGameObjectInType;

    Dictionary<string, int> dict_KindIndex;

	void Awake () {
		m_Instance = this;
		m_iNumGameObjectInType = 10;
        m_iCount = new int[10];
        m_GameObjects = new List<ObjectPoolData>[10];
        dict_KindIndex = new Dictionary<string, int>();
    }
	//
	int FindEmptySlot()
	{
		int i;
		for(i = 0; i < m_iNumGameObjectInType; i++) {
			if(m_GameObjects[i] == null) {
				break;	
			}
		}
		if(i == m_iNumGameObjectInType) {
			return -1;
		} else {
			return i;
		}
	}

	public void DeInit()
	{
		int i, j;
		int iCount;
		for(i = 0; i < m_iNumGameObjectInType; i++) {
			if(m_GameObjects[i] != null) {
				iCount = m_GameObjects[i].Count;
				for(j = 0; j < iCount; j++) {
					Destroy(m_GameObjects[i][j].m_go);
					Debug.Log("ddd");
				}
			}
		}
	}
	
	
	public void InitObjectsInPool(Object obj, string kind, int iCount)
	{
		int iSlot = FindEmptySlot();
		// Not found, return or resize buffer.
		if(iSlot < 0) {
			return;	
		}
		m_iCount[iSlot] = iCount;
		m_GameObjects[iSlot] = new List<ObjectPoolData>();
		for(int i = 0; i < iCount; i++) {
			GameObject go = Instantiate(obj) as GameObject;
			if(go == null) {
				break;
			}
            //ShowModel(go, false);
            go.SetActive(false);
			ObjectPoolData objData = new ObjectPoolData();
			objData.m_go = go;
			objData.m_bUsing = false;
			m_GameObjects[iSlot].Add(objData);
		}
        dict_KindIndex.Add(kind, iSlot);
	}
	
	public GameObject LoadObjectFromPool(string kind)
	{
        int iSlot = dict_KindIndex[kind];

        if (iSlot < 0 || iSlot >= m_iNumGameObjectInType) {
			return null;	
		}
		GameObject go = null;
		int iCount = m_GameObjects[iSlot].Count;
		for(int i = 0; i < iCount; i++) {
			ObjectPoolData objData = m_GameObjects[iSlot][i];
			if(objData.m_bUsing == false) {
				go = objData.m_go;
                go.SetActive(true);
				objData.m_bUsing = true;
				break;
			}
		}
		return go;
	}
	
	public bool UnLoadObjectToPool(string kind, GameObject go)
	{
        int iSlot = dict_KindIndex[kind];

        if (iSlot < 0 || iSlot >= m_iNumGameObjectInType) {
			return false;	
		}
		bool bRet = false;
		int iCount = m_GameObjects[iSlot].Count;
		for(int i = 0; i < iCount; i++) {
			ObjectPoolData objData = m_GameObjects[iSlot][i];
			if(objData.m_go == go) {
				objData.m_bUsing = false;
                go.SetActive(false);
				bRet = true;
				break;
			}
		}
		return bRet;
	}
	
	public void DestroyPoolSlot(string kind)
	{
        int iSlot = dict_KindIndex[kind];

        if (iSlot < 0 || iSlot >= m_iNumGameObjectInType) {
			return;	
		}
		int iCount = m_GameObjects[iSlot].Count;
		for(int i = 0; i < iCount; i++) {
			ObjectPoolData objData = m_GameObjects[iSlot][i];
			Destroy(objData.m_go);
			m_GameObjects[iSlot][i] = null;
		}
		m_GameObjects[iSlot] = null;
	}
}
