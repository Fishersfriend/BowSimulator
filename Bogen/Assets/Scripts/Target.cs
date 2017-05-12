using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : Hitable {

    private int health = 2;

    Rigidbody[] pieces;

    private void Start()
    {
        pieces = GetComponentsInChildren<Rigidbody>();
    }

    public override void Hit()
    {
        if (!Alive) return;
        health--;

        if(health >= 1)
        {
            for (int i = 0; i < 3; i++)
            {
                pieces[i].constraints = RigidbodyConstraints.None;
                //pieces[i].velocity = (new Vector3(Random.value, Random.value, Random.value)).normalized * 5;
            }
        } else
        {
            for (int i = 3; i < 7; i++)
            {
                pieces[i].constraints = RigidbodyConstraints.None;
                //pieces[i].velocity = (new Vector3(Random.value, Random.value, Random.value)).normalized * 5;
            }
        }


        if (health <= 0) Alive = false;
    }
}
