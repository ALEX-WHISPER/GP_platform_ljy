using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour {
    static PoolManager _instance;

    //  Singleton
    public static PoolManager GetInstance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<PoolManager>();
            }
            return _instance;
        }
    }

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    //  stores all the object pools
    Dictionary<int, Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();

    //  create a new pool
    public void CreatePool(GameObject prefab, int poolSize) {
        int poolKey = prefab.GetInstanceID();

        GameObject poolHolder = new GameObject(prefab.name + " Pool");
        poolHolder.transform.parent = transform;

        if (!poolDictionary.ContainsKey(poolKey)) {
            poolDictionary.Add(poolKey, new Queue<ObjectInstance>());

            for (int i = 0; i < poolSize; i++) {
                ObjectInstance objInstance = new ObjectInstance(Instantiate(prefab) as GameObject);
                poolDictionary[poolKey].Enqueue(objInstance);
                objInstance.SetParent(poolHolder.transform);
            }
        }
    }

    //  when you still need more objects while all the objects have been used currently
    public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation) {
        int poolKey = prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(poolKey)) {
            //  put the first element to the end of the queue
            ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
            poolDictionary[poolKey].Enqueue(objectToReuse);

            objectToReuse.Reuse(position, rotation);
        }
    }

    public class ObjectInstance {
        GameObject go;
        Transform xform;

        bool hasPoolObjectComponent;
        PoolObject poolObject;

        public ObjectInstance (GameObject objectInstance) {
            go = objectInstance;
            xform = go.transform;
            
            if (go.GetComponent<PoolObject>() != null) {
                hasPoolObjectComponent = true;
                poolObject = go.GetComponent<PoolObject>();
            }
            go.SetActive(false);
        }

        //  get an object from pool and use it
        public void Reuse(Vector3 position, Quaternion rotation) {
            if (hasPoolObjectComponent) {
                poolObject.OnObjectReuse();
            }
            
            go.SetActive(true);
            go.transform.position = position;
            go.transform.rotation = rotation;
        }

        public void SetParent(Transform parent) {
            xform.parent = parent;
        }
    }
}
