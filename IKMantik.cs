using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class IKMantik : MonoBehaviour
{
    bool duraksatildi;

    public Slider zamanSlider;
    public enum UzanmaHedefi { SOL_EL, SAG_EL};

    bool sagElUzat;
    bool solElUzat;

    public Animator karsiAnimator;

    public int animasyonNo;
    public int animasyonNoKarsi;

    [Range(0,1)] public float uzanmaSiddeti;

    public UzanmaHedefi uHedef;

    Animator animator;
    private int eskiAnimasyonNo;
    private int eskiAnimasyonNoKarsi;
    bool bastanBasla = false;

    float kendiZaman;
    float karsiZaman;

    float zamanKatSayisi;

    float baslangic, bitis;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        eskiAnimasyonNo = animasyonNo;
        animator.Play("" + animasyonNo);

        if(karsiAnimator != null)
        {
            eskiAnimasyonNoKarsi = animasyonNoKarsi;
            karsiAnimator.Play("" + animasyonNoKarsi);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(animasyonNo != eskiAnimasyonNo)
        {
            eskiAnimasyonNo = animasyonNo;
            bastanBasla = true;
        }
        
        if (karsiAnimator != null && eskiAnimasyonNoKarsi != animasyonNoKarsi)
        {
            eskiAnimasyonNoKarsi = animasyonNoKarsi;
            bastanBasla = true;
        }

        AnimatorStateInfo animState = animator.GetCurrentAnimatorStateInfo(0);
        kendiZaman = animState.normalizedTime;

        if(karsiAnimator != null)
        {
            AnimatorStateInfo animStateKarsi = karsiAnimator.GetCurrentAnimatorStateInfo(0);
            karsiZaman = animStateKarsi.normalizedTime;

            zamanKatSayisi = animState.length / animStateKarsi.length;
        }

        if(kendiZaman >= 1 && karsiZaman >= 1)
        {
            bastanBasla = true;
        }

        if (bastanBasla)
        {
            bastanBasla = false;

            animator.Play("" + animasyonNo,0,0);
            if(karsiAnimator != null)
            {
                karsiAnimator.Play("" + animasyonNoKarsi,0,0);
            }
        }

        if (zamanSlider != null)
        {
            if (duraksatildi)
            {
                animator.Play("" + animasyonNo, 0, zamanSlider.value);


                if(karsiAnimator != null)
                {
                    karsiAnimator.Play("" + animasyonNoKarsi, 0, zamanSlider.value * zamanKatSayisi);
                }
            }
            else
            {
                zamanSlider.value = kendiZaman % 1;
            }
        }
    }

    public void DuraksatCevir()
    {
        duraksatildi = !duraksatildi;
    }

    public void BaslangicEkle()
    {
        baslangic = zamanSlider.value;
    }
    
    public void BitisEkle()
    {
        bitis = zamanSlider.value;
    }

    int elModu = 0;
    public void ElDegistir()
    {
        elModu = (elModu + 1) % 4;
        switch(elModu)
        {
            case 0:
                sagElUzat = false;
                solElUzat = false;
                break;
            case 1:
                sagElUzat = true;
                solElUzat = false;
                break;
            case 2:
                sagElUzat = false;
                solElUzat = true;
                break;
            case 3:
                sagElUzat = true;
                solElUzat = true;
                break;
        }

    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(karsiAnimator != null && animator != null)
        {
            float zamanParcasi = kendiZaman % 1;
    
            if (zamanParcasi < baslangic)
            {
                uzanmaSiddeti = 0;
            }
            else if (zamanParcasi < bitis)
            {
                uzanmaSiddeti += Mathf.Lerp(0,1,Time.deltaTime);
            }
            else
            {
                uzanmaSiddeti -= Mathf.Lerp(0, 1, Time.deltaTime);
            }

            uzanmaSiddeti = Mathf.Clamp(uzanmaSiddeti, 0, 1);

            Vector3 pos = Vector3.zero;

            if(uHedef == UzanmaHedefi.SOL_EL)
            {
                pos = karsiAnimator.GetBoneTransform(HumanBodyBones.LeftHand).position;
            }
            else if (uHedef == UzanmaHedefi.SAG_EL)
            {
                pos = karsiAnimator.GetBoneTransform(HumanBodyBones.RightHand).position;
            }

            if(sagElUzat)
            {
                animator.SetIKPosition(AvatarIKGoal.RightHand, pos);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, uzanmaSiddeti);
            }

            if(solElUzat)
            {
                animator.SetIKPosition(AvatarIKGoal.LeftHand, pos);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, uzanmaSiddeti);
            }
            
        }
    }
}
