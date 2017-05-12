using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class UI : MonoBehaviour {

    public RectTransform heartPrefab;

    public string Message {
        set {
            msgText.text = value;
        }
    }

    private Animator anim;
    private Text msgText;

    private RectTransform[] hearts;
    private int health;
    public int Health {
        get { return health; }
        set {
            if (health > value) anim.Play("BloodScreen", 0, 0f);
            health = value;

            if (hearts != null) {
                for (int i = hearts.Length - 1; i >= health; i--) {
                    hearts[i].gameObject.SetActive(false);
                }
            }
            else CreateHearts();
        }
    }

    private void Awake() {
        anim = GetComponent<Animator>();
        msgText = transform.Find("Message").GetComponent<Text>();
    }

    private void CreateHearts() {
        hearts = new RectTransform[health];
        float size = 0.05f;
        for (int i = 0; i < health; i++) {
            hearts[i] = Instantiate<RectTransform>(heartPrefab, transform);
            hearts[i].localScale = Vector3.one;
            hearts[i].sizeDelta = new Vector2(size, size);
            hearts[i].anchoredPosition3D = new Vector2(size * .8f * i + size / 2, -size/2);
        }

    }

    public IEnumerator DisplayMessage(string msg, float duration) {
        Debug.Log("Here");
        Message = msg;
        ShowMessage();
        yield return new WaitForSeconds(duration);
        HideMessage();
    }

    public void ShowMessage() {
        anim.Play("MessageBlendIn", 0, 0f);

    }

    public void HideMessage() {
        anim.Play("MessageBlendOut", 0, 0f);
    }
}
