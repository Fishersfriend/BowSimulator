﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Arrow : MonoBehaviour {

    public Transform bloodSplatterPrefab;

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
        Hitable hitable = other.GetComponentInParent<Hitable>();
        if (hitable != null) {
            Transform blood = Instantiate<Transform>(bloodSplatterPrefab, transform.position, transform.rotation);
            blood.Rotate(0, 180, 0);

            if (hitable.Alive)
            {
                hitable.Hit();
            }
        }
    }
}
