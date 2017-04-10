using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hitable : MonoBehaviour {
    public bool Alive = true;
    public abstract void Hit();
}
