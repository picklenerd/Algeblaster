using UnityEngine;
using System.Collections;

public class MultizapperZap : MonoBehaviour {

    private float zapSpeed;
    private int zapDamage;
    private float chainRange;
    private int numberOfChains = 1;
    private Transform myTarget;
    private WeaponHit myHit;

    void FixedUpdate() {
        if (myTarget != null)
            rigidbody.velocity = (myTarget.transform.position - transform.position) * zapSpeed;
        if (numberOfChains <= 0)
            Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<EnemyHealthManager>()) {
            other.gameObject.GetComponent<EnemyHealthManager>().Hit(myHit);
            myTarget = null;
            numberOfChains--;
            if (numberOfChains > 0) {
                Collider[] colliders = Physics.OverlapSphere(transform.position, chainRange);
                foreach (Collider c in colliders) {
                    if (c.rigidbody && c.rigidbody.CompareTag("Fodder") && c.gameObject != other.gameObject) {
                        myTarget = c.transform;
                    }
                }
                if (myTarget == null)
                    Destroy(this.gameObject);
            }
        }

    }

    public void SetProperties(Transform target, float zSpeed, int zDamage, float cRange, int numChains, WeaponHit hit) {
        myTarget = target;
        zapSpeed = zSpeed;
        zapDamage = zDamage;
        chainRange = cRange;
        numberOfChains = numChains;
        myHit = hit;
    }

}