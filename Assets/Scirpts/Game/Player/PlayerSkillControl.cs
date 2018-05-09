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
        this.SingletonCheck();
        _motor = GetComponent<PlatformerMotor2D>();
    }

    private void SingletonCheck() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void OnEnable() {

    }
}
