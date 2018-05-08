using UnityEngine;
using System;

[Serializable]
public class TackleInfo: MonoBehaviour {
    public bool isPicked;
    public string tagName;
    public Sprite tackleSprite;
    
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

