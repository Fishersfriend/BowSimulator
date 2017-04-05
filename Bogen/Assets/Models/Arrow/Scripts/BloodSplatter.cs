using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class BloodSplatter : MonoBehaviour {

    ParticleSystem ps;

    private void Awake() {
        ps = GetComponent<ParticleSystem>();
    }

    private void Update() {
        if (!ps.IsAlive()) Destroy(gameObject);
    }
}
