using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType {
    SWORD = 0,
    DAGGER = 1
}

public class SkillSlot : InventorySlot {
    private SkillType skillType;
    private float coolDownDuration;

    private PlayerSkillControl skillControl;

    protected new void Awake() {
        base.Awake();
        skillControl = PlayerSkillControl.GetInstance;
    }

    protected override void OnFilledFuncImplement() {
        base.OnFilledFuncImplement();

        if (this.skillType == SkillType.DAGGER) {
            PC2D.Input.WAVE_SWORD = KeyCode.Alpha1 + this.slotIndex;
            Debug.Log(string.Format("set input keycode for sword: index: {0}, keycode: {1}", this.slotIndex, PC2D.Input.WAVE_SWORD));
        }
        if (this.skillType == SkillType.SWORD) {
            PC2D.Input.THROW_DAGGER = KeyCode.Alpha1 + this.slotIndex;
            Debug.Log(string.Format("set input keycode for dagger: index: {0}, keycode: {1}", this.slotIndex, PC2D.Input.THROW_DAGGER));
        }
    }

    public override void OnSlotContentUsed() {
        StartCoroutine(Cooldown());
    }

    public override InventoryType InventoryTypeValue { get {return InventoryType.SKILL;} }
    public SkillType SkillTypeValue { get { return this.skillType; } set { this.skillType = value; } }
    public float SkillCoolDownDuration { get { return this.coolDownDuration; } set { this.coolDownDuration = value; } }

    IEnumerator Cooldown() {
        float elapsedTime = 0f;
        this.img_Mask.gameObject.SetActive(true);

        while(elapsedTime < coolDownDuration) {
            elapsedTime += Time.deltaTime;
            this.img_Mask.fillAmount = elapsedTime / coolDownDuration;
            yield return null;
        }
        this.img_Mask.gameObject.SetActive(false);
    }
}
