using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class DaggerBehaviour : MonoBehaviour {

    float moveSpeed;
    float maxLifeTime;

    string damageableTag;
    Rigidbody2D m_Rigidbody2D;
    bool isHitEnemy;

    private void Awake() {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }

    public float MoveSpeed { get { return this.moveSpeed; } set { this.moveSpeed = value; } }
    public float MaxLifeTime { get { return this.maxLifeTime; } set { this.maxLifeTime = value; } }
    public string DamageTagName { get { return this.damageableTag; } set { this.damageableTag = value; } }
    
    public void Destroy() {
        if (!gameObject.activeSelf) {
            return;
        }
        
        gameObject.SetActive(false);
    }
    
    public void ResetMotion() {
        transform.localPosition = Vector2.zero;
        transform.localScale = Vector2.one;
        transform.localRotation = Quaternion.identity;
    }

    private void OnEnable() {
        StartCoroutine(StartFlyingWithLifeTime());
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(damageableTag)) {
            isHitEnemy = true;

            Destroy();
            Debug.Log("--- Hit ---");
        }
    }

    IEnumerator StartFlyingWithLifeTime () {
        float elapseTime = 0f;

        while (elapseTime < MaxLifeTime) {
            if (isHitEnemy) {
                break;
            }
            transform.Translate(Vector2.up * MoveSpeed * Time.deltaTime, Space.Self);
            elapseTime += Time.deltaTime;
            yield return null;
        }

        Destroy();
    }
}
