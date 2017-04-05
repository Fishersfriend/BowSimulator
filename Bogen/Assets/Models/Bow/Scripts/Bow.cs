using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour {
    public Arrow arrowPrefab;
    private Animator anim;

    public float reloadTime = 1f;
    public bool nocked = true;

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
        previewArrow.transform.localPosition = Vector3.forward * (0.62f - 0.5f * Power);

        anim.Play("Idle", 0, Power);
    }

    public void Shoot() {
        if (!nocked) return;
        Arrow newArrow = Instantiate<Arrow>(arrowPrefab, transform.position, transform.rotation);
        newArrow.GetComponent<Rigidbody>().velocity = transform.forward * powerMulti * Power;
        StartCoroutine(Reload());
    }

    private IEnumerator Reload() {
        previewArrow.gameObject.SetActive(false);
        nocked = false;
        yield return new WaitForSeconds(reloadTime);
        previewArrow.gameObject.SetActive(true);
        nocked = true;
    }
}
