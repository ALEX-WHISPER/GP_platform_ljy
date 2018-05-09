using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyBehaviour : MonoBehaviour {
    static Collider2D[] s_ColliderCache = new Collider2D[16];

    public event Action PlayerEnterDamageRange;
    public event Action PlayerOutofDamageRange;

    public bool spriteFaceLeft = false;

    [Header("Movement")]
    public float chaseSpeed;
    public float patrolSpeed;
    public float gravity = 10.0f;
    public float patrolingForwardDistance;

    [Header("Scanning settings")]
    [Tooltip("The angle of the forward of the view cone. 0 is forward of the sprite, 90 is up, 180 behind etc.")]
    [Range(0.0f, 360.0f)]
    public float viewDirection = 0.0f;
    [Range(0.0f, 360.0f)]
    public float viewFov;
    public float viewDistance;
    [Tooltip("Time in seconds without the target in the view cone before the target is considered lost from sight")]
    public float timeBeforeTargetLost = 3.0f;

    [Header("Melee Attack Data")]
    //[EnemyMeleeRangeCheck]
    public float meleeRange = 3.0f;
    //public Damager meleeDamager;
    //public Damager contactDamager;
    [Tooltip("if true, the enemy will jump/dash forward when it melee attack")]
    public bool attackDash;
    [Tooltip("The force used by the dash")]
    public Vector2 attackForce;

    protected SpriteRenderer m_SpriteRenderer;
    protected CharacterController2D m_CharacterController2D;
    protected Collider2D m_Collider;
    protected Animator m_Animator;

    //as we flip the sprite instead of rotating/scaling the object, this give the forward vector according to the sprite orientation
    protected Vector2 m_SpriteForward;
    protected Bounds m_LocalBounds;
    protected ContactFilter2D m_Filter;

    protected Vector3 m_MoveVector;
    protected Transform m_Target;
    protected float m_TimeSinceLastTargetView;

    protected float m_FireTimer = 0.0f;

    protected bool m_Dead = false;

    protected readonly int m_HashGroundedPara = Animator.StringToHash("Grounded");

    private void Awake() {
        m_CharacterController2D = GetComponent<CharacterController2D>();
        m_Collider = GetComponent<Collider2D>();
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        
        m_SpriteForward = spriteFaceLeft ? Vector2.left : Vector2.right;
        if (m_SpriteRenderer.flipX) m_SpriteForward = -m_SpriteForward;
    }

    private void Start() {
        m_LocalBounds = new Bounds();
        int count = m_CharacterController2D.Rigidbody2D.GetAttachedColliders(s_ColliderCache);
        for (int i = 0; i < count; ++i) {
            m_LocalBounds.Encapsulate(transform.InverseTransformBounds(s_ColliderCache[i].bounds));
        }

        m_Filter = new ContactFilter2D();
        m_Filter.layerMask = m_CharacterController2D.groundedLayerMask;
        m_Filter.useLayerMask = true;
        m_Filter.useTriggers = false;
    }

    private void OnEnable() {
        PlayerEnterDamageRange += MoveTowardsTarget;
    }

    private void OnDisable() {
        PlayerEnterDamageRange -= MoveTowardsTarget;
    }

    void FixedUpdate() {
        if (m_Dead)
            return;

        UpdateTargetFindingState();

        m_MoveVector.y = Mathf.Max(m_MoveVector.y - gravity * Time.deltaTime, -gravity);

        m_CharacterController2D.Move(m_MoveVector * Time.deltaTime);
        
        m_CharacterController2D.CheckCapsuleEndCollisions();

        UpdateTimers();
    }

    void UpdateTimers() {
        if (m_TimeSinceLastTargetView > 0.0f)
            m_TimeSinceLastTargetView -= Time.deltaTime;

        if (m_FireTimer > 0.0f)
            m_FireTimer -= Time.deltaTime;
    }

    void UpdateTargetFindingState() {
        if (m_Target == null) {
            Patrolling();
        } else {
            OrientToTarget();
            UpdateFacing();
            CheckTargetStillVisible();
        }
    }

    void Patrolling() {
        if (CheckForObstacle(patrolingForwardDistance)) {
            Debug.Log("detect ground end");
            this.SetHorizontalSpeed(-patrolSpeed);
            this.UpdateFacing();
        } else {
            Debug.Log("on patrolling...");
            this.SetHorizontalSpeed(patrolSpeed);
        }
        ScanForPlayer();
    }

    public void ScanForPlayer() {
        Vector3 dir = PlayerController2D.GetInstance.transform.position - transform.position;

        if (dir.sqrMagnitude > viewDistance * viewDistance) {
            return;
        }

        Vector3 testForward = Quaternion.Euler(0, 0, spriteFaceLeft ? Mathf.Sign(m_SpriteForward.x) * -viewDirection : Mathf.Sign(m_SpriteForward.x) * viewDirection) * m_SpriteForward;

        float angle = Vector3.Angle(testForward, dir);

        if (angle > viewFov * 0.5f) {
            return;
        }

        m_Target = PlayerController2D.GetInstance.transform;
        m_TimeSinceLastTargetView = timeBeforeTargetLost;
        
        if (PlayerEnterDamageRange != null) {
            PlayerEnterDamageRange();
        }
    }

    public void CheckTargetStillVisible() {
        if (m_Target == null)
            return;

        Vector3 toTarget = m_Target.position - transform.position;

        if (toTarget.sqrMagnitude < viewDistance * viewDistance) {
            Vector3 testForward = Quaternion.Euler(0, 0, spriteFaceLeft ? -viewDirection : viewDirection) * m_SpriteForward;
            if (m_SpriteRenderer.flipX) testForward.x = -testForward.x;

            float angle = Vector3.Angle(testForward, toTarget);

            if (angle <= viewFov * 0.5f) {
                //we reset the timer if the target is at viewing distance.
                m_TimeSinceLastTargetView = timeBeforeTargetLost;
            }
        }
        
        if (m_TimeSinceLastTargetView <= 0.0f) {
            ForgetTarget();
        }
    }

    public bool CheckForObstacle(float forwardDistance) {
        //we circle cast with a size sligly small than the collider height. That avoid to collide with very small bump on the ground
        if (Physics2D.CircleCast(m_Collider.bounds.center, m_Collider.bounds.extents.y - 0.2f, m_SpriteForward, forwardDistance, m_Filter.layerMask.value)) {
            return true;
        }

        Vector3 castingPosition = (Vector2)(transform.position + m_LocalBounds.center) + m_SpriteForward * m_LocalBounds.extents.x;
        Debug.DrawLine(castingPosition, castingPosition + Vector3.down * (m_LocalBounds.extents.y + 0.2f));

        if (!Physics2D.CircleCast(castingPosition, 0.1f, Vector2.down, m_LocalBounds.extents.y + 0.2f, m_CharacterController2D.groundedLayerMask.value)) {
            return true;
        }

        return false;
    }

    public void ForgetTarget() {
        m_Target = null;

        if (PlayerOutofDamageRange != null) {
            PlayerOutofDamageRange();
        }
    }

    public void SetFacingData(int facing) {
        if (facing == -1) {
            m_SpriteRenderer.flipX = !spriteFaceLeft;
            m_SpriteForward = spriteFaceLeft ? Vector2.right : Vector2.left;
        } else if (facing == 1) {
            m_SpriteRenderer.flipX = spriteFaceLeft;
            m_SpriteForward = spriteFaceLeft ? Vector2.left : Vector2.right;
        }
    }

    public void OrientToTarget() {
        if (m_Target == null)
            return;

        Vector3 toTarget = m_Target.position - transform.position;

        if (Vector2.Dot(toTarget, m_SpriteForward) < 0) {
            SetFacingData(Mathf.RoundToInt(-m_SpriteForward.x));
        }
    }

    public void UpdateFacing() {
        bool faceLeft = m_MoveVector.x < 0f;
        bool faceRight = m_MoveVector.x > 0f;

        if (faceLeft) {
            SetFacingData(-1);
        } else if (faceRight) {
            SetFacingData(1);
        }
    }

    protected void MoveTowardsTarget() {
        StartCoroutine(ChasingTarget());
    }

    IEnumerator ChasingTarget() {
        Vector3 dir = PlayerController2D.GetInstance.transform.position - transform.position;
        Vector2 targetPos = m_Target.position;

        while (dir.sqrMagnitude <= viewDistance * viewDistance) {
            //Vector2 tarPos = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            //transform.position = new Vector2(tarPos.x, transform.position.y);
            SetHorizontalSpeed(chaseSpeed);
            dir = PlayerController2D.GetInstance.transform.position - transform.position;
            yield return null;
        }
    }

    protected void LoseTarget() {

    }

    public void SetHorizontalSpeed(float horizontalSpeed) {
        m_MoveVector.x = horizontalSpeed * m_SpriteForward.x;
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
        {
            //draw the cone of view
            Vector3 forward = spriteFaceLeft ? Vector2.left : Vector2.right;
            forward = Quaternion.Euler(0, 0, spriteFaceLeft ? -viewDirection : viewDirection) * forward;

            if (GetComponent<SpriteRenderer>().flipX) forward.x = -forward.x;

            Vector3 endpoint = transform.position + (Quaternion.Euler(0, 0, viewFov * 0.5f) * forward);

            Handles.color = new Color(0, 1.0f, 0, 0.2f);
            Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, viewFov, viewDistance);

            //  Draw attack range
            Handles.color = new Color(1.0f, 0,0, 0.1f);
            Handles.DrawSolidDisc(transform.position, Vector3.back, meleeRange);
        }
#endif
}
