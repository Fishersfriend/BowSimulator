using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject menue;
    public GameObject[] level;

	// Use this for initialization
	void Start () {
        menue.SetActive(true);
        level[0].SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame()
    {
        menue.SetActive(false);
        level[0].SetActive(true);
    }

}
