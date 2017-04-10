using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bow))]
public class BowDebugController : MonoBehaviour {

    [Range(0, 30)]
    public float pullStrength = 10f;

    private Bow bow;
    private bool pulling = false;

    private void Awake() {
        bow = GetComponent<Bow>();
    }

    private void FixedUpdate() {
        if (Input.GetMouseButton(0)) {
            if(bow.pull < 1500 - pullStrength) bow.pull += pullStrength;
            pulling = true;
        }
        else if (Input.GetMouseButtonUp(0) && pulling) {
            bow.Shoot();
            pulling = false;
        }
        else bow.pull *= .6f;
    }
}
