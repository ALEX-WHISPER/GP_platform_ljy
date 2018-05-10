using UnityEngine.Events;
using System;
using UnityEngine;

public class Damageable : MonoBehaviour {
    [Serializable]
    public class HealthEvent : UnityEvent<Damageable> { }

    [Serializable]
    public class DamageEvent : UnityEvent<Damager, Damageable> { }

    [Serializable]
    public class HealEvent : UnityEvent<int, Damageable> { }

    public int startingHealth = 5;
    public bool invulnerableAfterDamage = true;
    public float invulnerabilityDuration = 3f;
    public bool disableOnDeath = false;

    public HealthEvent OnHealthSet;
    public DamageEvent OnTakeDamage;
    public DamageEvent OnDie;
    public HealEvent OnGainHealth;

    protected bool m_Invulnerable;
    protected float m_InulnerabilityTimer;
    protected int m_CurrentHealth;
    protected Vector2 m_DamageDirection;
    protected bool m_ResetHealthOnSceneReload;

    public int CurrentHealth {
        get { return m_CurrentHealth; }
    }

    private void OnEnable() {
        //  set initial health point
        m_CurrentHealth = startingHealth;

        //  call back on health set
        OnHealthSet.Invoke(this);

        //  disable invulnerability
        DisableInvulnerability();
    }

    private void Update() {
        if (m_Invulnerable) {
            m_InulnerabilityTimer -= Time.deltaTime;
            if (m_InulnerabilityTimer <= 0) {
                m_Invulnerable = false;
            }
        }
    }

    public void DisableInvulnerability() {
        invulnerableAfterDamage = false;
    }

    public void EnableInvulnerability(bool ignoreTimer = false) {
        m_Invulnerable = true;
        m_InulnerabilityTimer = ignoreTimer ? float.MaxValue : invulnerabilityDuration;
    }

    public void TakeDamage(Damager damager, bool ignoreInvincible = false) {
        if ((m_Invulnerable && !ignoreInvincible) || m_CurrentHealth <= 0)
            return;

        //  we can reach that point if the damager was one that was ignoring invincible state.
        //  We still want the callback that we were hit, but not the damage to be removed from health.
        if (!m_Invulnerable) {
            m_CurrentHealth -= damager.damage;
            OnHealthSet.Invoke(this);
        }
        
        OnTakeDamage.Invoke(damager, this);

        if (m_CurrentHealth <= 0) {
            OnDie.Invoke(damager, this);
            m_ResetHealthOnSceneReload = true;
            EnableInvulnerability();
            if (disableOnDeath) gameObject.SetActive(false);
        }
    }

    public void GainHealth(int amount) {
        m_CurrentHealth += amount;

        if (m_CurrentHealth > startingHealth)
            m_CurrentHealth = startingHealth;

        OnHealthSet.Invoke(this);

        OnGainHealth.Invoke(amount, this);
    }

    public void SetHealth(int amount) {
        m_CurrentHealth = amount;

        OnHealthSet.Invoke(this);
    }
}
