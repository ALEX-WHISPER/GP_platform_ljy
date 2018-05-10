using System.Collections;
using UnityEngine;

public class TestManager : MonoBehaviour {
    public GameObject prefab;
    public int poolSize = 5;

    private void Start() {
        PoolManager.GetInstance.CreatePool(prefab, poolSize);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            PoolManager.GetInstance.ReuseObject(prefab, Vector3.zero, Quaternion.identity);
        }
    }
}
