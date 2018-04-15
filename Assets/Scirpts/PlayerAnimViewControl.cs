using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[RequireComponent(typeof(Animator))]
//[RequireComponent(typeof(PlayerControl))]
public class PlayerAnimViewControl : MonoBehaviour {
    private PlayerControl m_PlayerControl;
    private Player m_Player;
    private Animator m_Anim;

    private void Awake() {
        m_PlayerControl = GetComponent<PlayerControl>();
        m_Anim = GetComponent<Animator>();
        m_Player = GetComponent<Player>();
    }

    private void OnEnable() {
        if (m_PlayerControl != null) {
            m_PlayerControl.PlayAnim_IDLE += OnPlayIdle;
            m_PlayerControl.PlayAnim_RUN += OnPlayRun;
            m_PlayerControl.PlayAnim_JUMP += OnPlayJump;
            m_PlayerControl.PlayAnim_ATTACK += OnPlayAttack;
            m_PlayerControl.PlayAnim_SLIDE += OnPlaySlide;
        }

        if (m_Player != null) {
            m_Player.PlayAnim_IDLE += OnPlayIdle;
            m_Player.PlayAnim_RUN += OnPlayRun;
            m_Player.PlayAnim_JUMP += OnPlayJump;
            m_Player.PlayAnim_ATTACK += OnPlayAttack;
            m_Player.PlayAnim_SLIDE += OnPlaySlide;
        }
    }

    private void OnDisable() {
        if (m_PlayerControl != null) {
            m_PlayerControl.PlayAnim_IDLE -= OnPlayIdle;
            m_PlayerControl.PlayAnim_RUN -= OnPlayRun;
            m_PlayerControl.PlayAnim_JUMP -= OnPlayJump;
            m_PlayerControl.PlayAnim_ATTACK -= OnPlayAttack;
            m_PlayerControl.PlayAnim_SLIDE -= OnPlaySlide;
        }
        if (m_Player != null) {
            m_Player.PlayAnim_IDLE -= OnPlayIdle;
            m_Player.PlayAnim_RUN -= OnPlayRun;
            m_Player.PlayAnim_JUMP -= OnPlayJump;
            m_Player.PlayAnim_ATTACK -= OnPlayAttack;
            m_Player.PlayAnim_SLIDE -= OnPlaySlide;
        }
    }

    private void OnPlayIdle() {

    }

    private void OnPlayRun(float h_Move) {
        m_Anim.SetFloat("speed", Mathf.Abs(h_Move));
    }

    private void OnPlayJump(bool grounded) {
        m_Anim.SetBool("ground", grounded);
    }

    private void OnPlayAttack() {
        m_Anim.SetTrigger("attack");
    }

    private void OnPlaySlide() {
        m_Anim.SetTrigger("slide");
    }
}
