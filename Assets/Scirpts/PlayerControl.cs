using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(UserInputControl))]
[RequireComponent(typeof(PlayerAnimViewControl))]

public class PlayerControl : MonoBehaviour {
    #region Anim Events
    public event Action PlayAnim_IDLE;
    public event Action<float> PlayAnim_RUN;
    public event Action<bool> PlayAnim_JUMP;
    public event Action PlayAnim_ATTACK;
    public event Action PlayAnim_SLIDE;
    #endregion

    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
    [SerializeField] private bool m_AirControl = true;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character    

    private PlayerAnimViewControl m_AnimControl;
    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    private void Awake() {
        this.ReferenceSettings();
    }

    private void FixedUpdate() {
        this.CheckIfGrounded();
    }

    private void ReferenceSettings() {
        // Setting up references.
        m_AnimControl = GetComponent<PlayerAnimViewControl>();
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void CheckIfGrounded() {
        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
        }

        //  Play jump anim
        if (PlayAnim_JUMP != null) {
            PlayAnim_JUMP(m_Grounded);
        }
    }

    public void Move(float horizontalMove, bool jump) {
       
        if (m_Grounded || m_AirControl) {
            //  play run anim
            if (PlayAnim_RUN != null) {
                PlayAnim_RUN(horizontalMove);
            }

            m_Rigidbody2D.velocity = new Vector2(horizontalMove * m_MaxSpeed, m_Rigidbody2D.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (horizontalMove > 0 && !m_FacingRight) {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (horizontalMove < 0 && m_FacingRight) {
                // ... flip the player.
                Flip();
            }
        }

        if (m_Grounded && jump) {
            m_Rigidbody2D.AddForce(new Vector2(0, m_JumpForce));
        }
    }
    public void Attack() {
        if (PlayAnim_ATTACK != null) {
            PlayAnim_ATTACK();
        }
    }

    public void Slide() {
        if (PlayAnim_SLIDE != null) {
            PlayAnim_SLIDE();
        }
    }

    private void Flip() {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
