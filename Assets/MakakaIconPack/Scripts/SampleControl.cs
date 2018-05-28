using UnityEngine;
using System.Collections;

public class SampleControl : MonoBehaviour 
{
    public float showDelay = 0.2f;

    private Animator m_MenuAnimator;

    private void Awake() {
        m_MenuAnimator = GetComponent<Animator>();        
    }

    void Start () 
	{
        Invoke("AutoShowMenu", showDelay);
	}

    private void AutoShowMenu() {
        if (m_MenuAnimator) {
            m_MenuAnimator.SetTrigger("Show");
            Debug.Log("Game Over Menu Show");
        }
    }
}
