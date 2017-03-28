using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullingBow : MonoBehaviour
{
    public GameObject GameManager;
    public GameObject arrow;
    public GameObject bow;
    Animator anim;

    GameObject newArrow;

    bool shot;
    bool nocked = false;

    float power;
    public float powerMulti = 0.1f;

    [Range(0.0f, 1500.0f)]
    public float pull = 0;


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        TelnetSocket telnetSocket = GameManager.GetComponent<TelnetSocket>();
    }

    // Update is called once per frame
    void Update()
    {
        shot = GameManager.GetComponent<TelnetSocket>().isShot;

        if (!shot)
        {
            //pull = GameManager.GetComponent<TelnetSocket>().pull;     //<- HERE
            power = pull / 15;
            power = power / 100;

            if (power > 0 && power <= 1)
            {
                if (!nocked && pull > 100)
                {
                    newArrow = Instantiate(arrow, this.transform.position, this.transform.rotation);
                    
                    newArrow.transform.parent = this.transform;
                    nocked = true;
                }
                if (nocked && pull > 100)
                {
                    newArrow.transform.localPosition = new Vector3(0, 0, 0 + 0.12f * power);
                    newArrow.transform.rotation = this.transform.rotation;

                }
                anim.Play("Idle", 0, power);

            }
            if (power < 0)
            {
                power = 0;
                anim.Play("Idle", 0, 0);
            }
            if (power > 1)
            {
                power = 1;
                anim.Play("Idle", 0, 1);
            }
        }
        if (shot)
        {
            //Debug.Log("Reset");
            anim.Play("Idle", 0, 0);
            if (nocked)
            {
                Debug.Log("destroy");
                Destroy(newArrow);
                nocked = false;
                //Debug.Log(nocked);

                int powerInt = GameManager.GetComponent<TelnetSocket>().powerInt;
                Quaternion rotation = this.transform.rotation;
                Vector3 direction = rotation * Vector3.forward;
                newArrow = Instantiate(arrow, this.transform.position, this.transform.rotation);
                newArrow.transform.position = this.transform.position;
                newArrow.transform.rotation = this.transform.rotation;
                newArrow.GetComponent<Rigidbody>().AddForce(direction * powerInt * powerMulti);
            }
            StartCoroutine(Wait(1));
        }


    }

    IEnumerator Wait(int duration)
    {
        yield return new WaitForSeconds(duration);
        GameManager.GetComponent<TelnetSocket>().isShot = false;
        anim.Play("Idle", 0, 0);
    }

}



