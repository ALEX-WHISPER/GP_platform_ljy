using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlot : InventorySlot {
    public float coolDownDuration;

    protected new void Awake() {
        base.Awake();
    }
    
    protected new void OnFilledFuncImplement() {
        StartCoroutine(Cooldown());
    }

    public void SetSkillCooldownDuration(float cooldown) {
        this.coolDownDuration = cooldown;
    }

    IEnumerator Cooldown() {
        float elapsedTime = 0f;

        while(elapsedTime < coolDownDuration) {
            elapsedTime += Time.deltaTime;
            this.img_Main.fillAmount = elapsedTime / coolDownDuration;
            yield return null;
        }
    }
}
