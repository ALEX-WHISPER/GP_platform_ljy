using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlatformerMotor2D), typeof(Rigidbody2D))]
public class PlayerTackleControl : MonoBehaviour {
    public int tackleLayerNum = 12;
    public List<TackleInfo> tackleList;

    public event Action PickUpDaggerSkill;
    
    private PlatformerMotor2D _motor;
    
    private void Awake() {
        _motor = GetComponent<PlatformerMotor2D>();
    }

    private void OnEnable() {
        PickedUpEventsReg();    //  tackles got picked up
        FireSkillEventsReg();   //  skills got fired
    }

    private void OnDisable() {
        PickedUpEventsDeReg();
        FireSkillEventsDeReg();
    }

    #region Pick up events
    private void PickedUpEventsReg() {
        for (int i = 0; i < tackleList.Count; i++) {
            int index = i;
            tackleList[index].onPickUp += OnTacklePickedUp;
        }
    }

    private void PickedUpEventsDeReg() {
        for (int i = 0; i < tackleList.Count; i++) {
            int index = i;
            tackleList[index].onPickUp -= OnTacklePickedUp;
        }
    }
    #endregion

    #region On picked up tackle: put it in a slot
    private void OnTacklePickedUp(TackleInfo tackle) {
        //  pick up skill tackle
        if (tackle.tackleProperty == TackleProperty.SKILL) {
            SkillSlot slot_Skill = (SkillSlot)InventoryManager.GetInstance.GetAvailableSlot();

            slot_Skill.SetSlotImage(tackle.tackleIcon);
            slot_Skill.SlotTackleType = tackle.tackleContent;

            if (slot_Skill.SlotTackleType == TackleContent.SWORD) {
                slot_Skill.SkillCoolDownDuration = _motor.waveCooldown;
                _motor._isWaveEnabled = true;
            } else if (slot_Skill.SlotTackleType == TackleContent.DAGGER) {
                slot_Skill.SkillCoolDownDuration = _motor.throwCooldown;
                _motor._isThrowEnabled = true;
            } else if (slot_Skill.SlotTackleType == TackleContent.DASH_SHOE) {
                slot_Skill.SkillCoolDownDuration = _motor.dashCooldown;
                _motor._isDashEnabled = true;
            }

            slot_Skill.FillSlot();

            if (tackle.tackleContent == TackleContent.DAGGER) {
                if (PickUpDaggerSkill != null) {
                    PickUpDaggerSkill();
                }
            }

            Debug.Log(string.Format("Put tackle: {0} into slot: {1}", tackle.tackleContent, slot_Skill.slotIndex));
        }
    }
    #endregion

    #region Consuming events: consuming the content in slot
    private void FireSkillEventsReg() {
        _motor.OnFireSkill += OnFireSkill;
    }
    private void FireSkillEventsDeReg() {
        _motor.OnFireSkill -= OnFireSkill;
    }
    #endregion

    #region On skill fired
    private void OnFireSkill(TackleContent skillType) {
        for (int i = 0; i < InventoryManager.GetInstance.GetUsedSlots.Count; i++) {
            if (InventoryManager.GetInstance.GetUsedSlots[i].SlotTackleType == skillType) {
                SkillSlot slot = (SkillSlot)InventoryManager.GetInstance.GetUsedSlots[i];
                slot.OnSlotContentUsed();
            }
        }
    }
    #endregion
    
    private void OnTriggerEnter2D(Collider2D collision) {
        //  not interact with tackle layer
        if (!collision.gameObject.layer.Equals(tackleLayerNum)) {
            return;
        }

        //  no tackle settings attached
        if (collision.gameObject.GetComponent<TackleInfo>() == null) {
            return;
        }

        //  the tackle has been picked up already
        if (collision.gameObject.GetComponent<TackleInfo>().isPicked) {
            return;
        }

        TackleInfo tackle_Triggered = collision.gameObject.GetComponent<TackleInfo>();
        tackle_Triggered = collision.gameObject.GetComponent<TackleInfo>();
        tackle_Triggered.GetPickedUp();
    }
}
