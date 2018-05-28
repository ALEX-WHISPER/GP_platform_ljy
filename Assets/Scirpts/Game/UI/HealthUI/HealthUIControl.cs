using System.Collections;
using UnityEngine;

public class HealthUIControl : MonoBehaviour {
    public GameObject[] m_HealthSlotArray;

    public void OnHealthPointUpdated(Damageable damageable) {
        for (int i = 0; i < m_HealthSlotArray.Length; i++) {
            if (m_HealthSlotArray[i].transform.Find("hp_Image") != null) {
                if (i < m_HealthSlotArray.Length - damageable.CurrentHealth)
                    m_HealthSlotArray[i].transform.Find("hp_Image").gameObject.SetActive(false);
                else
                    m_HealthSlotArray[i].transform.Find("hp_Image").gameObject.SetActive(true);
            }
        }
    }
}
