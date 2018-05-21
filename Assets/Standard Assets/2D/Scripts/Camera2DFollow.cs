using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;
        public Vector2 posClampVector_Max;
        public Vector2 posClampVector_Min;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;

        private void Awake() {
            if (target == null) {
                if(GameObject.FindWithTag("Player") == null) {
                    Debug.LogError("Player is missing!");
                    return;
                }
                target = GameObject.FindWithTag("Player").transform;
            }
        }

        // Use this for initialization
        private void Start()
        {
            if (target == null) {
                return;
            }

            m_LastTargetPosition = target.position;
            m_OffsetZ = (transform.position - target.position).z;
            transform.parent = null;
        }


        // Update is called once per frame
        private void Update()
        {
            if (target == null) {
                return;
            }

            FollowTarget();
            //PositionClamp();
        }

        void FollowTarget() {
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget) {
                m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
            } else {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);
            float xClampValue = Mathf.Clamp(newPos.x, posClampVector_Min.x, posClampVector_Max.x);
            float yClampValue = Mathf.Clamp(newPos.y, posClampVector_Min.y, posClampVector_Max.y);
            newPos = new Vector3(xClampValue, yClampValue, newPos.z);

            transform.position = newPos;

            m_LastTargetPosition = target.position;
        }

        void PositionClamp() {
        }
    }
}
