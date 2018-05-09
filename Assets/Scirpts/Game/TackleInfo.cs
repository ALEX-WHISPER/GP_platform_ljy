using UnityEngine;
using System;

public enum TackleContent {
    SWORD,
    DAGGER,
    DASH_SHOE
}

public enum TackleProperty {
    SKILL,
    BONUS
}

[Serializable]
public class TackleInfo: MonoBehaviour {
    public string tackleName;
    public TackleProperty tackleProperty;   //  which type of this tackle
    public TackleContent tackleContent;     //  what is it of this tackle
    [HideInInspector]
    public bool isPicked;
    public Sprite tackleSprite;     //  the sprite being displayed as a pickable tackle
    public Sprite tackleIcon;       //  the sprite being displayed in the slot
    
    [HideInInspector]
    public event Action<TackleInfo> onPickUp;   //  callback on picked up

    public void GetPickedUp() {
        isPicked = true;

        if (onPickUp != null) {
            onPickUp(this);
        }
    }

    private void OnEnable() {
        this.onPickUp += OnTackledPickedUp;
    }

    private void OnDisable() {
        this.onPickUp -= OnTackledPickedUp;
    }

    private void OnTackledPickedUp(TackleInfo tackle) {
        Debug.Log(string.Format("onTacklePickedUp: {0}", tackleContent));

        //  disapper
        this.isPicked = true;
        this.gameObject.SetActive(false);

        //  add to backpack
    }
}

