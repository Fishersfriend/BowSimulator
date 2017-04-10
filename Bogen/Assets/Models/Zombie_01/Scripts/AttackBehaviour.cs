using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour {

    public float attackTime = 1f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Zombie zombie = animator.GetComponent<Zombie>();
        zombie.StartCoroutine(zombie.DealDamage(attackTime, stateInfo));
    }
}
