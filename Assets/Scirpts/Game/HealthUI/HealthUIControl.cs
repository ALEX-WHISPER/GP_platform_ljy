using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIControl : MonoBehaviour {
    public GameObject[] m_HealthSlotArray;

    private static HealthUIControl _instance = null;

    public static HealthUIControl GetInstance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<HealthUIControl>();

                if (_instance == null) {
                    GameObject go = new GameObject("HealthUIControl");
                    go.AddComponent<HealthUIControl>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void OnHealthPointUpdated(Damageable damageable) {
        //  health is full
        if (damageable.CurrentHealth >= m_HealthSlotArray.Length) {
            for (int i = 0; i < m_HealthSlotArray.Length; i++) {
                if (m_HealthSlotArray[i].transform.Find("hp_Image") != null) {
                    m_HealthSlotArray[i].transform.Find("hp_Image").gameObject.SetActive(true);
                }
            }
        } else {
            for (int i = 0; i < m_HealthSlotArray.Length - damageable.CurrentHealth; i++) {
                if (m_HealthSlotArray[i].transform.Find("hp_Image") != null) {
                    m_HealthSlotArray[i].transform.Find("hp_Image").gameObject.SetActive(false);
                }
            }
        }
    }
}
