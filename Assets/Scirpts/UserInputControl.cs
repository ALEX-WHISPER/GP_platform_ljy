using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(PlayerControl))]
public class UserInputControl : MonoBehaviour {
    public KeyCode keyCode_JUMP = KeyCode.Space;
    public KeyCode keyCode_ATTACK = KeyCode.A;
    public KeyCode keyCode_SLIDE = KeyCode.X;

    private PlayerControl m_Character;
    private bool m_Jump;

    private void Awake() {
        m_Character = GetComponent<PlayerControl>();
    }

    private void Update() {
        JumpInput();
        AttackInput();
        SlideInput();
    }

    private void FixedUpdate() {
        MoveInput();
    }

    private void AttackInput() {
        if (Input.GetKeyDown(keyCode_ATTACK)) {
            m_Character.Attack();
        }
    }

    private void SlideInput() {
        if (Input.GetKeyDown(keyCode_SLIDE)) {
            m_Character.Slide();
        }
    }

    private void JumpInput() {
        if (!m_Jump) {
            // Read the jump input in Update so button presses aren't missed.
            m_Jump = Input.GetKeyDown(keyCode_JUMP);
        }
    }

    private void MoveInput() {
        float h = Input.GetAxis("Horizontal");

        // Pass all parameters to the character control script.
        m_Character.Move(h, m_Jump);
        m_Jump = false;
    }
}
