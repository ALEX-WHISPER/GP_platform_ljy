using System.Collections;
using UnityEngine;

public class SwitchDoorControl : MonoBehaviour {

    public float nextTriggerDelay = 1f;

    protected readonly int m_HashSwitchTo = Animator.StringToHash("switchTo");
    protected readonly int m_HashSwitchBack = Animator.StringToHash("switchBack");

    private bool nextTriggerFlag;
    private Animator m_Animator;

    private void Awake() {
        m_Animator = GetComponent<Animator>();
    }

    private void Start() {
        nextTriggerFlag = true;
    }

    public void DoorSwitchTo() {
        if (!nextTriggerFlag) {
            return;
        }
        m_Animator.SetTrigger(m_HashSwitchTo);
    }

    public void DoorSwitchBack() {
        if (!nextTriggerFlag) {
            return;
        }
        m_Animator.SetTrigger(m_HashSwitchBack);
    }
}
