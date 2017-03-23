using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public Rigidbody rb;
    public GameObject arrow;
    Vector3 positionOld;
    Quaternion rotation;
    bool freeze = false;
    Vector3 normalCollider;

    public LayerMask mask;


	// Use this for initialization
	void Start ()
    {

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //positionOld = this.transform.position;

        if (Physics.Raycast(this.transform.position - 0.5f * this.transform.up, this.transform.up, 10f, mask))
        {
            //Debug.DrawRay(this.transform.position - 0.5f * this.transform.up, this.transform.up,Color.red, 10f);
            //Debug.Log("Here");
            positionOld = this.transform.position;
        }

        if (rb.velocity.magnitude>0.1)
        { 
            transform.LookAt(transform.position - rb.velocity);
            transform.Rotate(0, 180, 0);
            positionOld = this.transform.position;

        }

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Target")
        {
            //Debug.Log("freeze");
            rb.constraints = RigidbodyConstraints.FreezeAll;
            freeze = true;
            this.transform.position = this.positionOld + (this.transform.position - this.positionOld) / 2f;
            this.transform.parent = other.transform;
        }

        if(other.tag=="Floor")
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            freeze = true;
        }

        else if(other.tag=="Metal")
        {
            normalCollider = arrow.transform.position - other.transform.position;
            normalCollider= normalCollider.normalized;
            var x = Vector3.Dot(rb.velocity, normalCollider);
            rb.velocity = normalCollider * x;


            rb.velocity = -rb.velocity;
        }
    }
}
