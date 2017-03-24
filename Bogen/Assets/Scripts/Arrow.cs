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

    // Update is called once per frame
    void FixedUpdate()
    {
        //positionOld = this.transform.position;
        //zur Zeit nicht benutzt

        if (Physics.Raycast(this.transform.position - 0.5f * this.transform.up, this.transform.up, 10f, mask))
        {
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
            rb.constraints = RigidbodyConstraints.FreezeAll;
            freeze = true;
            //this.transform.position = this.positionOld + (this.transform.position - this.positionOld) / 2f;
            this.transform.parent = other.transform;
        }

        else if(other.tag=="Floor")
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            freeze = true;
        }
    }
}
