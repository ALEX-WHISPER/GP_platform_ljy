using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SwitchEffect : MonoBehaviour {
    public Sprite lightOff;
    public Sprite lightOn;

    private SpriteRenderer m_SpriteRenderer;

    private void Awake() {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SwitchLightOn() {
        if (lightOn == null) {
            return;
        }

        m_SpriteRenderer.sprite = lightOn;
    }

    public void SwitchLightOff() {
        if (lightOff == null) {
            return;
        }

        m_SpriteRenderer.sprite = lightOff;
    }
}
