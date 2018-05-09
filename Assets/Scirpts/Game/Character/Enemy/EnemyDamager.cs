using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyBehaviour))]
public class EnemyDamager : MonoBehaviour {

    private EnemyBehaviour m_EnemyBehaviour;

    private void Awake() {
        m_EnemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    private void OnEnable() {
        m_EnemyBehaviour.PlayerEnterDamageRange += EnableDamager;
        m_EnemyBehaviour.PlayerOutofDamageRange += DisableDamager;
    }

    private void OnDisable() {
        m_EnemyBehaviour.PlayerEnterDamageRange -= EnableDamager;
        m_EnemyBehaviour.PlayerOutofDamageRange -= DisableDamager;
    }

    private void EnableDamager() {
        Debug.Log("--- Player enter attack range ---");
    }

    private void DisableDamager() {
        Debug.Log("--- Player out of attcka range ---");
    }
}
