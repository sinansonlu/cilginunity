using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class LineMaker : MonoBehaviour
{
    public Camera c1;
    public Camera c2;

    public LineRenderer lineRenderer;
    public LineRenderer lr2;

    Animator anim;

    List<string> aniNames = new List<string>();
    int currAni = 0;

    HumanBodyBones b1 = HumanBodyBones.LeftHand;
    HumanBodyBones b2 = HumanBodyBones.RightHand;

    List<List<float>> dists = new List<List<float>>();
    List<float> listMin = new List<float>();
    List<float> listMax = new List<float>();
    float max;
    float min;

    int skalaParca = 16;
    float skalaD;

    List<LineRenderer> lrs = new List<LineRenderer>();

    void Start()
    {
        anim = GetComponent<Animator>();

        skalaD = 1f / skalaParca;

        for (int i = 0; i < skalaParca; i++)
        {
            lrs.Add(Instantiate(lr2));
        }

        foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
        {
            aniNames.Add(clip.name);

            List<float> list = new List<float>();
            for (float t = 0; t < 1; t += skalaD)
            {
                clip.SampleAnimation(anim.gameObject, t);
                float dist = Vector3.Distance(
                    anim.GetBoneTransform(b1).position,
                    anim.GetBoneTransform(b2).position
                );
                list.Add(dist);
            }
            dists.Add(list);
            listMin.Add(list.Min());
            listMax.Add(list.Max());
        }

        min = listMin.Min();
        max = listMax.Max();
    }

    void Update()
    {
        Transform hTra = anim.GetBoneTransform(HumanBodyBones.Hips);

        Vector3 cLoc = hTra.position + hTra.forward * 3;

        c1.transform.position = cLoc;
        c2.transform.position = cLoc;

        c1.transform.LookAt(hTra.position, Vector3.up);
        c2.transform.LookAt(hTra.position, Vector3.up);

        for(int i = 0; i < lrs.Count; i++){
            
            lrs[i].SetPosition(0, hTra.position + c1.transform.right * (i - (lrs.Count/2)) * (4.6f / skalaParca) - c1.transform.up * 1f);
            lrs[i].SetPosition(1, hTra.position + c1.transform.right * ((i+1) - (lrs.Count / 2)) * (4.6f / skalaParca) - c1.transform.up * 1f);
            Color cc = Color.Lerp(Color.red, Color.green, (dists[currAni][i] - min) / (max - min));
            lrs[i].startColor = cc;
            lrs[i].endColor = cc;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currAni = (currAni + 1) % aniNames.Count;
            anim.Play(aniNames[currAni]);
        }

        lineRenderer.SetPosition(0, anim.GetBoneTransform(b1).position);
        lineRenderer.SetPosition(1, anim.GetBoneTransform(b2).position);

        float nt = anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;

        float dist = dists[currAni][(int)(nt / skalaD)];
        Color c = Color.Lerp(Color.red, Color.green, (dist - min) / (max - min));

        lr2.startWidth = 0.45f;
        lr2.endWidth = 0.45f;
        lr2.SetPosition(0, hTra.position + c1.transform.right * (nt - 0.495f) * (4.6f) - c1.transform.up * 1f);
        lr2.SetPosition(1, hTra.position + c1.transform.right * (nt - 0.505f) * (4.6f) - c1.transform.up * 1f);
        lr2.startColor = c;
        lr2.endColor = c;

        lineRenderer.startColor = c;
        lineRenderer.endColor = c;
    }
}
