using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public GameObject fence;

    public int health = 5;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void Hit()
    {
        health--;
        if(health == 0)
        {
            fence.SetActive(false);
        }

    }
}
