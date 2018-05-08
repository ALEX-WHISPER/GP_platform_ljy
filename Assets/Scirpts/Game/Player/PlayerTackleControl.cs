using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlatformerMotor2D), typeof(Rigidbody2D))]
public class PlayerTackleControl : MonoBehaviour {
    //  tackle layer
    public LayerMask tackleLayer;

    [Header("Tackles")]
    public TackleInfo tackle_Sword;
    public TackleInfo tackle_Dagger;
    
    private PlatformerMotor2D _motor;
    private InventoryManager inventoryManager;

    private void Awake() {
        _motor = GetComponent<PlatformerMotor2D>();
        inventoryManager = InventoryManager.GetInstance;
    }

    private void OnEnable() {
        PickedUpEventsReg();
        FireSkillEventsReg();
    }

    private void OnDisable() {
        PickedUpEventsDeReg();
        FireSkillEventsDeReg();
    }

    #region Pick up events
    private void PickedUpEventsReg() {
        tackle_Sword.onPickUp += OnPickedUp_Sword;
        tackle_Dagger.onPickUp += OnPickedUp_Dagger;
    }

    private void PickedUpEventsDeReg() {
        tackle_Sword.onPickUp -= OnPickedUp_Sword;
        tackle_Dagger.onPickUp -= OnPickedUp_Dagger;
    }
    
    #region On picked up tackle
    private void OnPickedUp_Sword() {
        Debug.Log("pick up sword!");
        _motor._isWaveEnabled = true;

        SkillSlot slot_ForSword = (SkillSlot)inventoryManager.GetAvailableSlot();
        Debug.Log("GetSlot: " + slot_ForSword.slotIndex);

        slot_ForSword.SetSlotImage(tackle_Sword.tackleSprite);
        slot_ForSword.SkillTypeValue = SkillType.SWORD;
        slot_ForSword.SkillCoolDownDuration = _motor.waveCooldown;

        slot_ForSword.FillSlot();
    }

    private void OnPickedUp_Dagger() {
        Debug.Log("pick up dagger!");
        _motor._isThrowEnabled = true;

        SkillSlot slot_ForDagger = (SkillSlot)inventoryManager.GetAvailableSlot();
        Debug.Log("GetSlot: " + slot_ForDagger.slotIndex);
        slot_ForDagger.SetSlotImage(tackle_Dagger.tackleSprite);
        slot_ForDagger.SkillTypeValue = SkillType.DAGGER;
        slot_ForDagger.SkillCoolDownDuration = _motor.throwCooldown;

        slot_ForDagger.FillSlot();
    }
    #endregion
    #endregion

    #region Comsuming events
    private void FireSkillEventsReg() {
        _motor.OnFireSkill_WaveSword += OnFireSkill_Sword;
        _motor.OnFireSkill_ThrowDaggers += OnFireSkill_Dagger;
    }

    private void FireSkillEventsDeReg() {
        _motor.OnFireSkill_WaveSword -= OnFireSkill_Sword;
        _motor.OnFireSkill_ThrowDaggers -= OnFireSkill_Dagger;
    }

    private void OnFireSkill_Sword() {
        for (int i = 0; i < inventoryManager.usedSlots.Count; i++) {
            if (inventoryManager.usedSlots[i].InventoryTypeValue == InventoryType.SKILL) {
                SkillSlot slot = (SkillSlot)inventoryManager.usedSlots[i];
                if (slot.SkillTypeValue == SkillType.SWORD) {
                    slot.OnSlotContentUsed();
                }
            }
        }
    }

    private void OnFireSkill_Dagger() {
        for (int i = 0; i < inventoryManager.usedSlots.Count; i++) {
            if (inventoryManager.usedSlots[i].InventoryTypeValue == InventoryType.SKILL) {
                SkillSlot slot = (SkillSlot)inventoryManager.usedSlots[i];
                if (slot.SkillTypeValue == SkillType.DAGGER) {
                    slot.OnSlotContentUsed();
                }
            }
        }
    }
    #endregion
    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log(collision.gameObject.layer);

        if (collision.gameObject.GetComponent<TackleInfo>() == null) {
            return;
        }

        if (collision.gameObject.GetComponent<TackleInfo>().isPicked) {
            return;
        }

        TackleInfo tackle_Triggered = collision.gameObject.GetComponent<TackleInfo>();
        tackle_Triggered = collision.gameObject.GetComponent<TackleInfo>();
        tackle_Triggered.GetPickedUp();
    }
}
