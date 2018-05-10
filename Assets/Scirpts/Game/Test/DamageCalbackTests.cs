using UnityEngine;

public class DamageCalbackTests : MonoBehaviour {

    public void OnHealthPointsInit(Damageable damageable) {
        Debug.Log(string.Format("set health: {0} points to character: {1}", damageable.CurrentHealth, damageable.name));
    }

    public void OnDamaged(Damager damager, Damageable damageable) {
        Debug.Log(string.Format("{0} hit {1} for {2} points", damager.name, damageable.name, damager.damage));
    }

    public void OnDie(Damager damager, Damageable damageable) {
        Debug.Log(string.Format("{0} hit {1} to DIE!!!", damager.name, damageable.name));
    }
}
