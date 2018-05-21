using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    public float m_HurtTwinkleDuration = 2.5f;
    
    private Animator m_Animator;
    private PlayerController2D m_PlayerControl2D;
    private Damageable m_PlayerDamageable;

    protected readonly int m_HashHurt = Animator.StringToHash("hurt");
    protected readonly int m_HashDead = Animator.StringToHash("dead");

    private void Awake() {
        m_Animator = GetComponent<Animator>();
        m_PlayerControl2D = GetComponent<PlayerController2D>();
        m_PlayerDamageable = GetComponent<Damageable>();
    }

    public void OnPlayerGetHurt(Damager damager, Damageable damageable) {
        StartCoroutine(PlayerGetHurtEffect());
    }

    public void OnPlayerDie(Damager damager, Damageable damageable) {
        Debug.Log("Player dead");
        m_Animator.SetBool(m_HashDead, true);
    }

    IEnumerator PlayerGetHurtEffect() {
        //  trigger twinkle animation
        m_Animator.SetTrigger(m_HashHurt);
        m_PlayerControl2D.DisableMovement();
        m_PlayerDamageable.EnableInvulnerability();

        yield return new WaitForSeconds(m_HurtTwinkleDuration);

        //  back to idle
        m_PlayerControl2D.ResetPosToOrigin();
        m_PlayerControl2D.EnableMovement();
        m_PlayerDamageable.DisableInvulnerability();
    }
}
