using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pull : MonoBehaviour {

    public GameObject GameManager;
    float power;

    Animator anim;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        power = GameManager.GetComponent<ManagerPull>().power;
        power = power / 15;
        //Debug.Log(power);
        //Debug.Log("Seconds: "+power);

        power = power / 100;
        if(power>0)
        {
            //power = Mathf.Round(power * 10f) / 10f;
            Debug.Log("Seconds: "+power);

            anim.Play("Idle",0,power);

        }
        else
        {
            power = 0;

        }
    }
}
