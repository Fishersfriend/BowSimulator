using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Hitable {

    public int health = 5;

    public override void Hit() {
        health--;
        if (health <= 0) Alive = false;
    }
}
