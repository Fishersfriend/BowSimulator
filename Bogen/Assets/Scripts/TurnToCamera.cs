using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToCamera : MonoBehaviour {

    public GameObject camera;
    private Vector3 direction;
    private Vector3 newForward;

    private float speed = 0.005f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        direction = -camera.transform.position + this.transform.position;
        direction.y = 0;
        newForward = Vector3.Lerp(this.transform.forward, direction, speed);

        this.transform.forward = newForward; 
	}
}
