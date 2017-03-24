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
        //Calibrate
		if(Input.GetKeyDown(KeyCode.C))
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            this.transform.localPosition = new Vector3(0, 0, 0);

            calibrating = false;
            calibrationTarget.GetComponent<MeshRenderer>().enabled = false;
            calibrationTarget.GetComponent<MeshCollider>().enabled = false;
            calibrationTarget.transform.position = new Vector3(0, 0, 0);
        }

        //Activate CalibrationTarget
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("Calibrating");
            calibrating = true;
            calibrationTarget.transform.position = new Vector3(bow.transform.position.x, bow.transform.position.y, bow.transform.position.z + 3);
            calibrationTarget.GetComponent<MeshRenderer>().enabled = true;
            calibrationTarget.GetComponent<MeshCollider>().enabled = true;
        }

        //Transform CalibrationTarget
        if (calibrating)
        {
            calibrationTarget.transform.position = new Vector3(bow.transform.position.x, bow.transform.position.y, bow.transform.position.z + 3);

            Debug.DrawRay(bow.transform.position, new Vector3(0, 0, +3), Color.red, 0.01f);
        }
    }
}
