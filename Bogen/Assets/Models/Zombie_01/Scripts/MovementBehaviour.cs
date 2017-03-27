using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : StateMachineBehaviour {

    public float speed = 0;
    public bool vulnerable = true;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<Zombie>().Speed = speed;
        animator.GetComponent<Zombie>().Vulnerable = vulnerable;
    }
}
