using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolInfo {
    public GameObject prefab;
    public int poolSize;
}

public class PoolSpawner : MonoBehaviour {
    static PoolSpawner _instance;
    
    public PoolInfo daggersPoolInfo;
    
    public static PoolSpawner GetInstance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<PoolSpawner>();

                if (_instance == null) {
                    GameObject go_PoolSpawner = new GameObject("PoolSpawner");
                    go_PoolSpawner.AddComponent<PoolSpawner>();
                    DontDestroyOnLoad(go_PoolSpawner);
                }
            }
            return _instance;
        }
    }

    private void OnEnable() {
        GameObject.FindWithTag("Player").GetComponent<PlayerTackleControl>().PickUpDaggerSkill += CreateDaggersPool;
    }
    
    public void CreateDaggersPool() {
        Debug.Log("Create Daggers Pool");
        PoolManager.GetInstance.CreatePool(daggersPoolInfo.prefab, daggersPoolInfo.poolSize);
        GameObject.FindWithTag("Player").GetComponent<PlayerTackleControl>().PickUpDaggerSkill -= CreateDaggersPool;
    }
}
