using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooter : MonoBehaviour {

    public Arrow arrowPrefab;
    public bool aimBot;
    public Transform target;
    [Range(0, 50)]
    public float power;
    [Range(0, 5)]
    public float arrowFrequence;
    [Range(0, 1)]
    public float powerVariance;
    [Range(0, 1)]
    public float spread;

    private void Start() {
        StartCoroutine(ShootArrow());
    }

    private void Update() {
        if(aimBot && target != null) {
            transform.LookAt(target.transform.position + Vector3.up);
            transform.Rotate(90, 0, 0);
        }
    }

    private IEnumerator ShootArrow() {
        while (true) {
            Arrow arrow = Instantiate<Arrow>(arrowPrefab);
            arrow.transform.position = transform.position;
            arrow.transform.rotation = Quaternion.Lerp(transform.rotation, Random.rotation, spread);
            arrow.transform.Rotate(-90, 0, 0);
            Vector3 vel = arrow.transform.forward * power;
            arrow.GetComponent<Rigidbody>().velocity = vel - Random.Range(0f, powerVariance) * vel;
            if (aimBot) arrow.GetComponent<Rigidbody>().useGravity = false;
            yield return new WaitForSeconds(arrowFrequence);
        }

    }
}
