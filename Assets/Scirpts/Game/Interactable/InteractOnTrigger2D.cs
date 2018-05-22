using UnityEngine;
using UnityEngine.Events;

public class InteractOnTrigger2D : MonoBehaviour {
    public LayerMask layers;
    public UnityEvent OnEnter, OnExit;

    protected Collider2D m_Collider;

    void Reset() {
        layers = LayerMask.NameToLayer("Everything");
        m_Collider = GetComponent<Collider2D>();
        m_Collider.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!enabled)
            return;

        if (layers.Contains(other.gameObject)) {
            ExecuteOnEnter(other);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (!enabled)
            return;

        if (layers.Contains(other.gameObject)) {
            ExecuteOnExit(other);
        }
    }

    protected virtual void ExecuteOnEnter(Collider2D other) {
        OnEnter.Invoke();
    }

    protected virtual void ExecuteOnExit(Collider2D other) {
        OnExit.Invoke();
    }

    void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position, "InteractionTrigger", false);
    }
}
