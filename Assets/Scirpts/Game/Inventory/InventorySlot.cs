using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;

public abstract class InventorySlot : MonoBehaviour {
    public int slotIndex;
    protected Sprite slotSprite;
    protected bool isFilled;
    protected event Action OnFilledFunc;

    private Image img_BG;
    protected Image img_Main;

    protected void Awake() {
        if (transform.Find("BG") == null) {
            Debug.Log("There's no BG in slot's child nodes");
            return;
        }

        if (transform.Find("icon") == null) {
            Debug.Log("There's no icon in slot's child nodes");
            return;
        }

        img_BG = transform.Find("BG").GetComponent<Image>();
        img_Main = transform.Find("icon").GetComponent<Image>();
    }

    protected void OnEnable() {
        this.OnFilledFunc += OnFilledFuncImplement;
    }

    protected void OnDisable() {
        this.OnFilledFunc -= OnFilledFuncImplement;
    }

    public void SetSlotImage(Sprite img) {
        this.slotSprite = img;
    }

    /// <summary>
    /// fill this slot
    /// </summary>
    public virtual void FillSlot() {
        if (slotSprite == null) {
            Debug.Log("slot sprite is null");
            return;
        }

        if (this.isFilled) {
            Debug.Log("this slot has been set up");
            return;
        }

        img_BG.sprite = slotSprite;
        img_Main.sprite = slotSprite;
        isFilled = true;

        if (OnFilledFunc != null) {
            OnFilledFunc();
        }
    }

    /// <summary>
    /// on filled callback
    /// </summary>
    protected virtual void OnFilledFuncImplement() { }
}
