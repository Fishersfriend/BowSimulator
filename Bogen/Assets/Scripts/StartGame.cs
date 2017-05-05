using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

    public GameManager gamemanager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Wait(0f));

    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("StartGame");
        gamemanager.NextLevel(1);
        gamemanager.gameActive = true;
        gamemanager.StartTimer();
    }
}
