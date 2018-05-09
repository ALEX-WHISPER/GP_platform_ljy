using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlot : InventorySlot {
    private float coolDownDuration;
    
    protected new void Awake() {
        base.Awake();
    }

    protected override void OnFilledFuncImplement() {
        base.OnFilledFuncImplement();

        //  set the input key of each skill based on which slot it locates
        if (this.SlotTackleType == TackleContent.SWORD) {PC2D.Input.WAVE_SWORD = KeyCode.Alpha1 + this.slotIndex;}
        if (this.SlotTackleType == TackleContent.DAGGER) {PC2D.Input.THROW_DAGGER = KeyCode.Alpha1 + this.slotIndex;}
        if (this.SlotTackleType == TackleContent.DASH_SHOE) {PC2D.Input.START_SLIDE = KeyCode.Alpha1 + this.slotIndex;}
    }

    //  consuming the content in this slot
    public override void OnSlotContentUsed() {
        StartCoroutine(Cooldown());
    }
    
    //  set cool down duration, which is also the life time of mask
    public float SkillCoolDownDuration { get { return this.coolDownDuration; } set { this.coolDownDuration = value; } }

    //  mask's cool down effect
    IEnumerator Cooldown() {
        float elapsedTime = 0f;
        this.img_Mask.gameObject.SetActive(true);
        this.img_Mask.fillAmount = 1f;

        while(elapsedTime < coolDownDuration) {
            elapsedTime += Time.deltaTime;
            this.img_Mask.fillAmount = 1 - elapsedTime / coolDownDuration;
            yield return null;
        }
        this.img_Mask.gameObject.SetActive(false);
    }
}
