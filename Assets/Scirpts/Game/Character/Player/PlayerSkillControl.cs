using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillControl : MonoBehaviour {
    public GameObject m_DaggersPrefab;
    public Transform m_EmitPoint;

    private Damager meleeAttack;
    private PlatformerMotor2D _motor;
    private SpriteRenderer playerSprite;

    private void Awake() {
        RefsInit();
    }

    private void OnEnable() {
        _motor.OnFireSkill += OnPlayerFireSkill;
    }

    private void RefsInit() {
        _motor = GetComponent<PlatformerMotor2D>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    private void OnPlayerFireSkill(TackleContent skillType) {
        if (skillType == TackleContent.DAGGER) {
            //  throw daggers
            PoolManager.GetInstance.ReuseObject(m_DaggersPrefab, m_EmitPoint.position, Quaternion.identity);
        }

        if (skillType == TackleContent.SWORD) {
            //  wave sword
        }

        if (skillType == TackleContent.DASH_SHOE) {
            //  dash
        }
    }

    public void PrepareObjectPool(GameObject prefab, int poolSize) {

    }
}
