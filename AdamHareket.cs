using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdamHareket : MonoBehaviour
{
    public float hiz = 6.0f;
    public float ziplama = 8.0f;
    public float yerCekimi = 20.0f;

    bool yonSag = true;

    CharacterController cc;
    Animator anim;

    Vector3 hareket = Vector3.zero;

    Vector3 baslangic;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        baslangic = cc.transform.position;

        if (cc == null)
        {
            Debug.LogError("CharacterController ekli değil!");
        }

        if (anim == null)
        {
            Debug.LogError("Animator ekli değil!");
        }
    }

    void Update()
    { 
        if(Input.GetKey(KeyCode.R))
        {
            cc.enabled = false;
            cc.transform.position = baslangic;
            cc.enabled = true;
        }
        
        if (cc.isGrounded)
        {
            hareket = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
            hareket *= hiz;

            if(hareket.x != 0.0f)
            {
                anim.SetBool("yuru", true);
            }
            else
            {
                anim.SetBool("yuru", false);
            }

            if (Input.GetButton("Jump"))
            {
                hareket.y = ziplama;
                anim.SetBool("zipla", true);
            }
            else
            {
                anim.SetBool("zipla", false);
            }
        }
        else
        {
            hareket.x += Mathf.Clamp(Input.GetAxis("Horizontal") * hiz,-hiz * 1.1f,hiz * 1.1f) * Time.deltaTime;
            hareket.y -= yerCekimi * Time.deltaTime;
        }

        if(hareket.x > 0f)
        {
            yonSag = true;
            gameObject.transform.SetPositionAndRotation(gameObject.transform.position, Quaternion.Euler(0, 0, 0));
        }
        else if(hareket.x < 0f)
        {
            yonSag = false;
            gameObject.transform.SetPositionAndRotation(gameObject.transform.position, Quaternion.Euler(0, 180, 0));
        }

        cc.Move(hareket * Time.deltaTime);
    }
}
