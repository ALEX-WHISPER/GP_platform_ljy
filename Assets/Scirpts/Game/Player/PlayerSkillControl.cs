using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillControl : MonoBehaviour {
    public Sprite tackleSprite_Dagger;
    public Sprite tackleSprite_Sword;

    private PlatformerMotor2D _motor;
    private InventoryManager inventoryManager;

    private void Awake() {
        _motor = GetComponent<PlatformerMotor2D>();
        inventoryManager = InventoryManager.GetInstance;
    }

    private void OnEnable() {
        _motor.OnFireSkill_WaveSword += OnFireSkill_WaveSword;
        _motor.OnFireSkill_ThrowDaggers += OnFireSkill_ThrowDagger;
    }

    private void OnFireSkill_WaveSword() {
        SkillSlot swordSlot = (SkillSlot)inventoryManager.GetAvailableSlot();
        swordSlot.SetSlotImage(tackleSprite_Sword);
        swordSlot.SetSkillCooldownDuration(_motor.waveCooldown);

        swordSlot.FillSlot();
    }

    private void OnFireSkill_ThrowDagger() {
        SkillSlot daggerSlot = (SkillSlot)inventoryManager.GetAvailableSlot();
        daggerSlot.SetSlotImage(tackleSprite_Dagger);
        daggerSlot.SetSkillCooldownDuration(_motor.throwCooldown);

        daggerSlot.FillSlot();
    }
}
