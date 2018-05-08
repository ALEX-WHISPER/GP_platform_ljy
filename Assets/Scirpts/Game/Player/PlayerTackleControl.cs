using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlatformerMotor2D), typeof(Rigidbody2D))]
public class PlayerTackleControl : MonoBehaviour {
    public LayerMask tackleLayer;

    [Header("Tackles")]
    public TackleInfo tackle_Sword;
    public TackleInfo tackle_Dagger;
    
    private PlatformerMotor2D _motor;

    private void Awake() {
        _motor = GetComponent<PlatformerMotor2D>();
        
        //Debug.Log("<color='red'> TEST</color>");
    }

    private void OnEnable() {
        PickedUpEventsReg();
    }

    private void OnDisable() {
        PickedUpEventsDeReg();
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

    #region Tackle Events
    private void OnPickedUp_Sword() {
        Debug.Log("pick up sword!");
        _motor._isWaveEnabled = true;
    }

    private void OnPickedUp_Dagger() {
        Debug.Log("pick up dagger!");
        _motor._isThrowEnabled = true;
    }
    #endregion
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
