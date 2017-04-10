using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceWall : Hitable {

    private int health = 3;
    private int healthStep;

    Rigidbody[] pieces;

    private int lastId = 0;

    private void Start() {
        pieces = GetComponentsInChildren<Rigidbody>();

        for (var i = pieces.Length - 2; i > 0; i--) {
            var r = Random.Range(0, i);
            var tmp = pieces[i];
            pieces[i] = pieces[r];
            pieces[r] = tmp;
        }

        healthStep = Mathf.FloorToInt(pieces.Length / health);
    }

    public override void Hit() {
        if (!Alive) return;
        health--;

        for (int i = lastId; i < pieces.Length - healthStep * health; i++) {
            pieces[i].constraints = RigidbodyConstraints.None;
            pieces[i].velocity = (new Vector3(Random.value, Random.value, Random.value)).normalized * 5;
        }
        lastId = pieces.Length - healthStep * health;

        if (health <= 0) Alive = false;
    }
}
