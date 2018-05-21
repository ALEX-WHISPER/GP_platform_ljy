using System.Collections;
using UnityEngine;

public class HealthUIControl : MonoBehaviour {
    public GameObject[] m_HealthSlotArray;

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
