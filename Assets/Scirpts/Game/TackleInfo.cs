using UnityEngine;
using System;

public enum TackleType {
    SWORD = 0,
    DAGGER = 1
}

[Serializable]
public class TackleInfo: MonoBehaviour {
    public TackleType tackleType;
    public bool isPicked;
    public string tagName;
    
    [HideInInspector]
    public event Action onPickUp;

    public void GetPickedUp() {
        isPicked = true;

        if (onPickUp != null) {
            onPickUp();
        }
    }

    private void OnEnable() {
        this.onPickUp += OnTackledPickedUp;
    }

    private void OnDisable() {
        this.onPickUp -= OnTackledPickedUp;
    }

    private void OnTackledPickedUp() {
        //  disapper
        this.gameObject.SetActive(false);

        //  add to backpack
    }
}

