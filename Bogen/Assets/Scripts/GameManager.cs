using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject menue;
    public GameObject[] level;

    public GameObject Loose;
    public GameObject Win;

    public Text winnerText;
    public Light sun;
	// Use this for initialization
	void Start () {
        Loose.SetActive(false);
        Win.SetActive(false);
        menue.SetActive(false);     //changed
        level[0].SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.L))
        {
            GameOver();

        }
		
	}

    public void StartGame()
    {
        menue.SetActive(false);
        level[0].SetActive(true);
    }
    public void GameOver()
    {
        Loose.SetActive(true);
        StartCoroutine(DimLight());

    }
    public void Winner(int score)
    {

        Win.SetActive(true);
        winnerText.text = "Score:" + score + "\n Press Calibration-Button to restart";
    }

    IEnumerator DimLight()
    {
        while(sun.intensity>0)
        {
            yield return new WaitForEndOfFrame();
            sun.intensity = sun.intensity - 0.01f;
            RenderSettings.ambientIntensity = RenderSettings.ambientIntensity - 0.01f;
        }
    }

}
