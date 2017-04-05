using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bow))]
public class BowDebugController : MonoBehaviour {

    private float pullStrength = 10f;

    private Bow bow;
    [SerializeField]
    private bool pulling = false;

    private void Awake() {
        bow = GetComponent<Bow>();
    }

    private void FixedUpdate() {
        if (Input.GetMouseButton(0) && bow.nocked) {
            if(bow.pull < 1500 - pullStrength) bow.pull += pullStrength;
            pulling = true;
        }

        if (Input.GetMouseButtonUp(0) && pulling) {
            bow.Shoot();
            bow.pull = 0;
            pulling = false;
        }
    }
}
