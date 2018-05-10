using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggersPoolObject : PoolObject {
    public List<GameObject> daggers;

    public float daggerMoveSpeed;
    public float daggerMaxLifeDuration;
    public string damageableTagName;

    protected void Awake() {
        DaggerPropertiesInit();
    }

    private void DaggerPropertiesInit() {
        for (int i = 0; i < daggers.Count; i++) {
            int index = i;
            DaggerBehaviour daggerControl = daggers[index].GetComponent<DaggerBehaviour>();
            daggerControl.MoveSpeed = daggerMoveSpeed;
            daggerControl.MaxLifeTime = daggerMaxLifeDuration;
            daggerControl.DamageTagName = damageableTagName;
        }
    }

    public override void OnObjectReuse() {
        base.OnObjectReuse();

        CheckFacingDir();

        for (int i = 0; i < daggers.Count; i++) {
            int index = i;
            daggers[i].SetActive(true);
            DaggerBehaviour daggerControl = daggers[index].GetComponent<DaggerBehaviour>();

            daggerControl.ResetMotion();
        }
    }

    private void CheckFacingDir() {
        bool isFacingRight = PlayerController2D.GetInstance.IsFacingRight;
        int dir = isFacingRight ? 1 : -1;
        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * dir, transform.localScale.y);
    }
}
