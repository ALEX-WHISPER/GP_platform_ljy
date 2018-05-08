using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillControl : MonoBehaviour {
    private PlatformerMotor2D _motor;
    private static PlayerSkillControl _instance = null;

    public static PlayerSkillControl GetInstance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<PlayerSkillControl>();

                if (_instance == null) {
                    GameObject go = new GameObject("PlayerSkillControl");
                    go.AddComponent<PlayerSkillControl>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    private void Awake() {
        _motor = GetComponent<PlatformerMotor2D>();
    }

    private void OnEnable() {
        _motor.OnFireSkill_WaveSword += OnFireSkill_WaveSword;
        _motor.OnFireSkill_ThrowDaggers += OnFireSkill_ThrowDagger;
    }

    private void OnFireSkill_WaveSword() {
    }

    private void OnFireSkill_ThrowDagger() {
    }
}
