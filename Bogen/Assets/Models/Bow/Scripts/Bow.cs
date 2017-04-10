using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Animator))]
public class Bow : MonoBehaviour {
    public Arrow arrowPrefab;
    private Animator anim;

    [Range(0, 5)]
    public float reloadTime = 1f;

    [System.NonSerialized]
    public bool nocked = true;

    [Range(0, 100)]
    public float powerMulti = 1f;

    [Range(0.0f, 1500.0f)]
    public float pull = 0;

    private float Power {
        get {
            return pull / 1500;
        }
    }
    
    private Transform arrowHolder;
    private Arrow previewArrow;

    private void Awake() {
        anim = GetComponent<Animator>();
        arrowHolder = transform.FindChild("ARROW_HOLDER");
    }


    void Start() {
        previewArrow = Instantiate<Arrow>(arrowPrefab, arrowHolder);
        previewArrow.enabled = false;
        previewArrow.GetComponent<Rigidbody>().isKinematic = true;
        previewArrow.GetComponent<Collider>().enabled = false;
        previewArrow.transform.localEulerAngles = Vector3.right * -90;
        previewArrow.transform.localPosition = Vector3.up * 1.121f;
        StartCoroutine(Reload());
    }

    private void FixedUpdate() {
        arrowHolder.localPosition = Vector3.forward * (-.4963f - .5f * Power);

        anim.Play("Idle", 0, Power);
    }

    public void Shoot() {
        if (!nocked) return;
        Arrow newArrow = Instantiate<Arrow>(arrowPrefab, previewArrow.transform.position, transform.rotation);
        newArrow.GetComponent<Rigidbody>().velocity = transform.forward * powerMulti * Power;
        StartCoroutine(Reload());
    }

    private IEnumerator Reload() {
        anim.Play("Reload", 1, 0f);
        anim.speed = 1 / reloadTime;
        nocked = false;
        yield return new WaitForSeconds(reloadTime);
        nocked = true;
    }
}
