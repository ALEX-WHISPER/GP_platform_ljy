using System.Collections;
using UnityEngine;

public class TestObject : PoolObject {

    private void Update() {
        transform.localScale += Vector3.one * Time.deltaTime;
        transform.Translate(Vector3.forward * Time.deltaTime * 20f);
    }

    public override void OnObjectReuse() {
        base.OnObjectReuse();
        transform.localScale = Vector3.one;
    }
}
