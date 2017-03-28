using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PullingBow : MonoBehaviour
{
    
    public TelnetSocket telnetSocket;
    public Arrow arrowPrefab;
    private Animator anim;

    bool shot;
    bool nocked = false;
    
    public float powerMulti = 1f;

    [Range(0.0f, 1500.0f)]
    public float pull = 0;

    private float Power {
        get {
            return pull / 1500;
        }
    }

    private Arrow previewArrow;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    
    void Start() {
        previewArrow = Instantiate<Arrow>(arrowPrefab, transform.position, transform.rotation, transform);
        previewArrow.enabled = false;
        previewArrow.GetComponent<Rigidbody>().isKinematic = true;
        previewArrow.GetComponent<Collider>().enabled = false;
    }

    private void FixedUpdate() {
        pull = telnetSocket.pull;
  

        if (pull > 100) {
            previewArrow.gameObject.SetActive(true);
            previewArrow.transform.localPosition = Vector3.forward * (0.62f - 0.5f * Power);
        }
        else previewArrow.gameObject.SetActive(false);

        anim.Play("Idle", 0, Power);

        //Debug.Log("Shot: " + shot + ", pull: " + pull);

        if (telnetSocket.isShot) {
            ShootArrow();
            telnetSocket.isShot = false;
        }

        //if (pull <= 100) telnetSocket.isShot = false;
    }

    private void ShootArrow() {
        Arrow newArrow = Instantiate<Arrow>(arrowPrefab, transform.position, transform.rotation);
        newArrow.GetComponent<Rigidbody>().velocity = transform.forward * powerMulti * telnetSocket.powerInt;
    }

}



