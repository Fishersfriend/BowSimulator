using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Animator))]
public class Zombie : Hitable {

    public List<Hitable> targets;
    public Transform rightEye, leftEye;

    public float speedMultiplier;

    private float speed;
    public float Speed { set { speed = value; } }

    private bool vulnerable;
    public bool Vulnerable { set { vulnerable = value; } }

    private float attackRange = .8f;

    [SerializeField]
    private int health = 8;
    public int Health {
        get { return health; }
        set {
            health = value;
            anim.SetInteger("Health", value);
        }
    }

    private Animator anim;


    private void Awake() {
        anim = GetComponent<Animator>();
        anim.SetInteger("Health", health);
    }

    private void Update() {
        while (targets.Count > 0 && !targets[0].Alive) targets.RemoveAt(0);

        if (targets.Count > 0) {

            anim.SetBool("Idle", false);
            TurnEyes();
            Move();

            if (InRange(targets[0].transform))
                anim.SetBool("Attacking", true);
            else
                anim.SetBool("Attacking", false);

        }
        else anim.SetBool("Idle", true);
        
    }

    private void TurnEyes() {
        TurnEye(rightEye, Quaternion.Euler(Vector3.right * -180));
        TurnEye(leftEye);
    }

    private void TurnEye(Transform eye, Quaternion offsetRotation) {
        float maxAngle = 40f;
        Vector3 lookDir = targets[0].transform.position - eye.position;
        float angle = Vector3.Angle(eye.parent.forward, lookDir);
        if (angle <= maxAngle && angle >= -maxAngle) {
            eye.LookAt(targets[0].transform);
            eye.localRotation *= offsetRotation;
        }
    }

    private void TurnEye(Transform eye) {
        TurnEye(eye, Quaternion.Euler(Vector3.zero));
    }


    private void Move() {
        Vector3 dir = targets[0].transform.position - transform.position;
        dir.y = 0;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, .002f * speed);
        transform.position += transform.forward * (speed * speedMultiplier) / 1000;
    }

    public override void Hit() {
        if (!vulnerable || !Alive) return;

        Health--;
        anim.SetTrigger("Hit");
        if (Health <= 0) {
            if (Random.Range(0f, 1f) < .05f) StartCoroutine(Revive(5));
            else Alive = false;
        }
    }

    public IEnumerator DealDamage(float time, AnimatorStateInfo stateInfo) {
        yield return new WaitForSeconds(time);
        bool isSameAnim = anim.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(stateInfo.shortNameHash);
        if (targets.Count > 0 && isSameAnim && InRange(targets[0].transform))
            targets[0].Hit();
    }

    private IEnumerator Revive(float time) {
        yield return new WaitForSeconds(time);
        Health = 1;
    }

    private bool InRange(Transform trm) {
        Vector3 dir = targets[0].transform.position - transform.position;
        dir.y = 0;
        return dir.magnitude <= attackRange;
    }
	
}
