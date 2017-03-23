using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    bool hit = false;
    Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
        if (hit)
        {
            //Debug.Log("Fallen");
            anim.SetTrigger("Fall");
        }

    }
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Here");
        hit = true;

    }
}



