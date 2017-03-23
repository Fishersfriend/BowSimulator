using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibration : MonoBehaviour {

    public GameObject bow;
    public GameObject calibrationTarget;
    Quaternion Offset;
    Vector3 OffsetVec;
    bool calibrating = false;

    int counter = 0;

    bool timer = false;

    public Vector3[] calibrationValue;
    Quaternion MeanOffset;


    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {

		if(Input.GetKeyDown(KeyCode.C))
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            Debug.Log("Calibration");
            Offset = bow.transform.rotation;
            OffsetVec = Offset.eulerAngles;
            this.transform.rotation = Quaternion.Euler(-OffsetVec.x, -OffsetVec.y, -OffsetVec.z);
        }

        if(Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(Calibrate());

        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Calibrating");
            calibrating = true;
            calibrationTarget.transform.position = new Vector3(bow.transform.position.x, bow.transform.position.y, bow.transform.position.z + 3);
            calibrationTarget.GetComponent<MeshRenderer>().enabled = true;
            calibrationTarget.GetComponent<MeshCollider>().enabled = true;

        }


        if (calibrating)
        {
            calibrationTarget.transform.position = new Vector3(bow.transform.position.x, bow.transform.position.y, bow.transform.position.z + 3);

            Debug.DrawRay(bow.transform.position, new Vector3(0, 0, +3), Color.red, 0.01f);
        }



    }

    
    IEnumerator Calibrate()
    {
        while (counter < 10)
        {
            yield return new WaitForSeconds(0.5f);

            //calibrationValue[counter] = new Quaternion(counter, counter, counter,counter);
            calibrationValue[counter] = bow.transform.rotation.eulerAngles;

            Debug.Log("Counter: "+counter);

            counter++;
            //StartCoroutine(Calibrate());
        }

        if (counter == 10)
        {
            MeanOffset.eulerAngles = (calibrationValue[0] + calibrationValue[1] + calibrationValue[2] + calibrationValue[3] + calibrationValue[4] + calibrationValue[5] + calibrationValue[6] + calibrationValue[7] + calibrationValue[8] + calibrationValue[9]) / 10;
            Debug.Log("MeanError: " + MeanOffset.eulerAngles);

            this.transform.rotation = Quaternion.Euler(-MeanOffset.eulerAngles.x, -MeanOffset.eulerAngles.y, -MeanOffset.eulerAngles.z);

        }
        calibrating = false;
        calibrationTarget.GetComponent<MeshRenderer>().enabled = false;
        calibrationTarget.GetComponent<MeshCollider>().enabled = false;
        calibrationTarget.transform.position = new Vector3(0, 0, 0);

        counter = 0;

    }
    
}
