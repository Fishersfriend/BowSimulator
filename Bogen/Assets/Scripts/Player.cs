using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public GameManager gamemanager;
    private int health = 3;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HitPlayer()
    {
        health--;
        if (health == 0)
        {
            gamemanager.GameOver();
        }

    }

}
