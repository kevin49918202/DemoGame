using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

    #region Singleton

    public static PlayerManager instance;

    void Awake()
    {
        StartCoroutine(LoadAssetAsync());
        instance = this;
    }

    #endregion

    public GameObject player;
    public Dictionary<int, GameObject> dictOtherPlayer;
    public Dictionary<int, OtherPlayerMotor> dictOtherPlayerMotor;

    UnityEngine.Object heroPrefabSource;

    void Start()
    {
        dictOtherPlayer = new Dictionary<int, GameObject>();
        dictOtherPlayerMotor = new Dictionary<int, OtherPlayerMotor>();
    }

    public void KillPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddPlayer(int index ,string name)
    {
        GameObject heroPrefab = Instantiate(heroPrefabSource) as GameObject;
        dictOtherPlayer.Add(index, heroPrefab);
        dictOtherPlayerMotor.Add(index, heroPrefab.GetComponent<OtherPlayerMotor>());
    }

    public void RemovePlayer(int index)
    {
        Destroy(dictOtherPlayer[index].gameObject);
        dictOtherPlayer.Remove(index);
        dictOtherPlayerMotor.Remove(index);
    }

    IEnumerator LoadAssetAsync()
    {
        ResourceRequest rr = Resources.LoadAsync("HeroPrefab");
        yield return rr;
        if (rr != null && rr.isDone)
        {
            heroPrefabSource = rr.asset;
        }
    }
}
