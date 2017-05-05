using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Calibration : MonoBehaviour {

    public GameObject bow;
    public GameObject calibrationTarget;
    Quaternion Offset;
    Vector3 OffsetVec;
    public bool calibrationStart = false;
    bool target = false;

    bool timer = false;

    Quaternion MeanOffset;


    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Calibrate
		if(calibrationStart&&target)
        {
            Debug.Log("Calibrating");
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            this.transform.localPosition = new Vector3(0, 0, 0);

            target = false;
            calibrationStart = false;
            calibrationTarget.GetComponent<MeshRenderer>().enabled = false;
            calibrationTarget.GetComponent<MeshCollider>().enabled = false;
            calibrationTarget.transform.position = new Vector3(0, 0, 0);
        }

        //Activate CalibrationTarget
        if (calibrationStart && !target)
        {
            Debug.Log("Target");
            target = true;
            calibrationTarget.transform.position = new Vector3(bow.transform.position.x, bow.transform.position.y, bow.transform.position.z + 3);
            calibrationTarget.GetComponent<MeshRenderer>().enabled = true;
            calibrationTarget.GetComponent<MeshCollider>().enabled = true;
            calibrationStart = false;
        }

        //Transform CalibrationTarget
        if (target)
        {
            calibrationTarget.transform.position = new Vector3(bow.transform.position.x, bow.transform.position.y, bow.transform.position.z + 3);

            Debug.DrawRay(bow.transform.position, new Vector3(0, 0, +3), Color.red, 0.01f);
        }
    }
}
