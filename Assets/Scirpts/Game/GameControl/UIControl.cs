using System.Collections;
using UnityEngine;

public class UIControl : MonoBehaviour {

    private static UIControl _instance = null;

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(this);
        }
    }
}
