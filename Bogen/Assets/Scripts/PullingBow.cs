using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullingBow : MonoBehaviour {
    public GameObject GameManager;
    public GameObject arrow;
    public GameObject bow;

    GameObject newArrow;
    float power;
    bool shot;
    bool nocked=false;
    Vector3 arrowRotation;


    [Range(0.0f, 1500.0f)]
    public float pull = 0;

    Animator anim;


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
        //Debug.Log(GameManager.GetComponent<TelnetSocket>().isShot);

        if (!shot)
        {

            //pull = GameManager.GetComponent<TelnetSocket>().pull;


            //Debug.Log(GameManager.GetComponent<TelnetSocket>().pull);

            power = pull / 15;
            //Debug.Log(power);
            //Debug.Log("Seconds: "+power);

            power = power / 100;
            //Debug.Log(power);
            if (power > 0 && power <= 1)
            {
                //power = Mathf.Round(power * 10f) / 10f;
                //Debug.Log("Seconds: " + power);
                if (!nocked && pull>100)
                {
                    newArrow = Instantiate(arrow, bow.transform.position, bow.transform.rotation);
                    newArrow.transform.parent = bow.transform;
                    nocked = true;
                }
                if(nocked && pull>100)
                {
                    //- 0.485f * power
                    //newArrow.transform.position = new Vector3(bow.transform.localPosition.x, bow.transform.localPosition.y, bow.transform.position.z);
                    newArrow.transform.localPosition = new Vector3(0,0,0 - 0.485f * power);
                    newArrow.transform.rotation = bow.transform.rotation;

                }
                if (nocked && pull < 100)
                {
                    //Debug.Log("destroy");
                    Destroy(newArrow);
                    nocked = false;
                    //Debug.Log(nocked);
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
        if(shot)
        {
            //Debug.Log("Reset");
            
            anim.Play("Idle", 0, 0);
            StartCoroutine(Wait(1));
        }



    }
    IEnumerator Wait(int duration)
    {
        yield return new WaitForSeconds(duration);
        GameManager.GetComponent<TelnetSocket>().isShot = false;
    }

}