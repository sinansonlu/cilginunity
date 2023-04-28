using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuzKontrol : MonoBehaviour
{
    public Animator animator;
    public Renderer rend;

    public SkinnedMeshRenderer yuzSkin;
    public SkinnedMeshRenderer kirpikSkin;

    float zamanlayici;

    bool gulecek;

    void Start()
    {
        zamanlayici = 0;
        gulecek = true;
    }

    void Update()
    {
        zamanlayici += gulecek ? Time.deltaTime * 60 : -Time.deltaTime * 60;

        if(zamanlayici > 200)
        {
            gulecek = false;
        }

        if (zamanlayici < 0)
        {
            zamanlayici = 0;
            gulecek = true;
        }


        yuzSkin.SetBlendShapeWeight(0, Mathf.Clamp(zamanlayici,0,100));
        kirpikSkin.SetBlendShapeWeight(0, Mathf.Clamp(zamanlayici, 0, 100));

        rend.material.SetFloat("_Mixture", Mathf.Clamp(zamanlayici, 0, 100) / 100);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetLookAtWeight(1, 0.1f, 0.3f, 1f);

        animator.SetLookAtPosition(
            Camera.main.ScreenToWorldPoint(
                new Vector3(
                    Input.mousePosition.x,
                    Input.mousePosition.y,
                    0.3f
                )
            )
        );
    }
}
