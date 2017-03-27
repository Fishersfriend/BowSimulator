using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Arrow : MonoBehaviour {

    private Rigidbody rb;
    private bool frozen = false;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (transform.position.y < -50) Destroy(gameObject);

        if(!frozen) transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void OnTriggerStay(Collider other) {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        transform.parent = other.transform;
        GetComponent<Collider>().enabled = false;
        frozen = true;
        Zombie zombie = other.GetComponentInParent<Zombie>();
        if (zombie != null) zombie.Hit();
    }
}
