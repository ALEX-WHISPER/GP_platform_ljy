using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillControl : MonoBehaviour {
    public GameObject m_Daggers;
    public Transform m_EmitPoint;

    private PlatformerMotor2D _motor;
    private static PlayerSkillControl _instance = null;

    private void Awake() {
        RefsInit();
    }

    private void OnEnable() {
        _motor.OnFireSkill += OnPlayerFireSkill;
    }

    private void RefsInit() {
        _motor = GetComponent<PlatformerMotor2D>();
    }

    private void OnPlayerFireSkill(TackleContent skillType) {
        if (skillType == TackleContent.DAGGER) {
            //  throw daggers
            Instantiate(m_Daggers, Vector3.zero, Quaternion.identity, m_EmitPoint);
        }

        if (skillType == TackleContent.SWORD) {
            //  wave sword
        }

        if (skillType == TackleContent.DASH_SHOE) {
            //  dash
        }
    }
}
