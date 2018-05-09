using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;

public abstract class InventorySlot : MonoBehaviour {
    public int slotIndex;

    protected TackleContent tackleType;
    protected Sprite slotSprite;
    protected bool isFilled;
    protected event Action OnFilledFunc;

    protected Image img_Main;
    protected Image img_Mask;
    
    protected void Awake() {
        if (transform.Find("icon") == null) {
            Debug.Log("There's no icon in slot's child nodes");
            return;
        }

        if (transform.Find("icon").Find("mask") == null) {
            Debug.Log("There's no mask in icon's child node");
            return;
        }

        img_Main = transform.Find("icon").GetComponent<Image>();
        img_Mask = img_Main.transform.Find("mask").GetComponent<Image>();
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

        if (img_Main == null || img_Mask == null) {
            Debug.Log("img_Main or img_Mask is null");
            return;
        }

        img_Main.gameObject.SetActive(true);
        img_Main.sprite = slotSprite;
        img_Mask.gameObject.SetActive(false);
        isFilled = true;

        if (OnFilledFunc != null) {
            OnFilledFunc();
        }
    }

    public TackleContent SlotTackleType { get { return this.tackleType; } set { this.tackleType = value; } }

    #region Abstract / Virtual funcs
    /// <summary>
    /// on slot used
    /// </summary>
    public abstract void OnSlotContentUsed();

    /// <summary>
    /// on filled callback
    /// </summary>
    protected virtual void OnFilledFuncImplement() {
        InventoryManager.GetInstance.GetUsedSlots.Add(this);
        Debug.Log(string.Format("Add this slot: {0}, {1} to usedSlots list", this.slotIndex, this.tackleType));
    }
#endregion
}
