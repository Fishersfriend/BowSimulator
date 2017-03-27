using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Zombie : MonoBehaviour {

    public Transform target;
    public Transform rightEye, leftEye;

    public float speedMultiplier;

    private float speed;
    public float Speed { set { speed = value; } }

    private bool vulnerable;
    public bool Vulnerable { set { vulnerable = value; } }

    private float attackRange = 1f;

    [SerializeField]
    private int health = 8;
    public int Health {
        get { return health; }
        set {
            health = value;
            animator.SetInteger("Health", value);
        }
    }

    private Animator animator;


    private void Awake() {
        animator = GetComponent<Animator>();
        animator.SetInteger("Health", health);
    }

    private void Update() {
        TurnEyes();
        Move();

        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0;
        if (dir.magnitude <= attackRange) animator.SetBool("Attacking", true);
        else animator.SetBool("Attacking", false);
    }

    private void TurnEyes() {
        if (target != null) {
            rightEye.transform.LookAt(target);
            rightEye.transform.Rotate(-180, 0, 0);
            leftEye.transform.LookAt(target);
        }
    }


    private void Move() {
        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, .002f * speed);
        transform.position += transform.forward * (speed * speedMultiplier) / 1000;
    }

    public void Hit() {
        if (!vulnerable) return;

        Health--;
        animator.SetTrigger("Hit");

        if (Health <= 0 && Random.Range(0f, 1f) < .33f) StartCoroutine(Revive(5));
    }

    private IEnumerator Revive(float time) {
        yield return new WaitForSeconds(time);
        Health = 2;
    }
	
}
