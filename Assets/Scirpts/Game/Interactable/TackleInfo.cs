using UnityEngine;
using System;

public enum TackleContent {
    SWORD,
    DAGGER,
    DASH_SHOE,
    HEALTH_POINT,
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

    private GameObject m_Player;

    public void GetPickedUp() {
        isPicked = true;

        if (onPickUp != null) {
            onPickUp(this);
        }
    }

    private void Awake() {
        m_Player = GameObject.FindWithTag("Player");
        if (m_Player == null) {
            Debug.Log("No Player Founded");
        }
    }

    private void OnEnable() {
        this.onPickUp += OnTacklePickedUp;

        if (m_Player != null) {
            this.onPickUp += m_Player.GetComponent<PlayerTackleControl>().OnTacklePickedUp;
        } else {
            Debug.Log("m_Player is null, function: onTacklePickedUp from PlayerTackleControl added failed");
        }
    }

    private void OnDisable() {
        this.onPickUp -= OnTacklePickedUp;

        if (m_Player != null) {
            this.onPickUp -= m_Player.GetComponent<PlayerTackleControl>().OnTacklePickedUp;
        }
    }

    private void OnTacklePickedUp(TackleInfo tackle) {
        Debug.Log(string.Format("onTacklePickedUp: {0}", tackleContent));

        //  disapper
        this.isPicked = true;
        this.gameObject.SetActive(false);

        //  add to backpack
    }
}

