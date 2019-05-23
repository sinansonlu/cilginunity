using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KutuAlici : MonoBehaviour
{
    public Transform elNoktasi;
    public Camera cam;
    public Animator anim;

    GameObject eldeki;
    Rigidbody eldekiRigid;
    Collider eldekiCollider;

    int eldekilerLayer, yerdekilerLayer, layerMask;

    void Start()
    {
        eldekilerLayer = 8;
        yerdekilerLayer = 0;
        layerMask = 1 << 0;
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            OndekiniAl();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            EldekiniKoy();
        }
    }

    void EldekiniKoy()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 4f, layerMask))
        {
            if (eldeki != null)
            {
                eldeki.transform.SetParent(null);

                eldeki.transform.position = hit.point + hit.normal * (eldekiCollider.bounds.size.y / 2);
                eldeki.transform.rotation = Quaternion.identity;

                eldekiRigid.isKinematic = false;
                eldeki.layer = yerdekilerLayer;

                eldeki = null;
                eldekiRigid = null;
                eldekiCollider = null;

                anim.SetBool("tut", false);
            }
        }
    }

    void OndekiniAl()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 3f, layerMask))
        {
            if(hit.transform.tag.Equals("Alinabilir"))
            {
                
                if(eldeki != null)
                {
                    eldeki.transform.SetParent(null);
                    eldekiRigid.isKinematic = false;
                    eldeki.layer = yerdekilerLayer;
                }

                eldeki = hit.transform.gameObject;
                eldekiRigid = hit.rigidbody;
                eldekiCollider = hit.collider;
                eldeki.layer = eldekilerLayer;

                eldeki.transform.position = elNoktasi.position + (eldeki.transform.position - hit.point);
                eldeki.transform.rotation = Quaternion.identity;
                eldeki.transform.SetParent(elNoktasi.transform);
                eldekiRigid.isKinematic = true;

                anim.SetBool("tut", true);
            }
        }
    }
}
