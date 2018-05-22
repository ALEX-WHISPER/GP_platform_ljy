using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class ButtonSwingOnPointerEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private Button thisBtn;
    private Animator m_Animator;
    
    private void Awake() {
        thisBtn = GetComponent<Button>();
        m_Animator = GetComponent<Animator>();
    }
    
    public void OnPointerEnter(PointerEventData eventData) {
        m_Animator.SetBool("Swing", true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        m_Animator.SetBool("Swing", false);
    }
}
