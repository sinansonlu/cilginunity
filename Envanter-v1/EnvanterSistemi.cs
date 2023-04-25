using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnvanterSistemi : MonoBehaviour
{
    private Esya eldekiEsya;

    public GameObject birim;
    public int yatay;
    public int dikey;

    public Esya[] baslangictakiEsyalar;
    List<Esya> eldekiEsyalar = new List<Esya>();


    Birim[,] birimler;

    void Start()
    {
        birimler = new Birim[yatay, dikey];

        for (int i = 0; i < yatay; i++)
        {
            for (int j = 0; j < dikey; j++)
            {
                GameObject yeni = Instantiate(birim, gameObject.transform);
                yeni.transform.localPosition = new Vector3(i - yatay / 2, 0, j - dikey / 2);
                Birim birimClassi = yeni.AddComponent<Birim>();
                birimClassi.x = i;
                birimClassi.y = j;
                birimClassi.esyaIndeksi = -1;
                birimler[i, j] = birimClassi;
            }
        }

        birim.SetActive(false);

        for (int k = 0; k < baslangictakiEsyalar.Length; k++)
        {
            EsyaYerlestir(baslangictakiEsyalar[k]);
        }
    }

    public bool EsyaYerlestir(Esya yeniEsya)
    {
        for (int i = 0; i < yatay; i++)
        {
            for (int j = dikey-1; j >= 0; j--)
            {
                if(HaneleriKontrolEt(i,j,yeniEsya.genislik,yeniEsya.yukseklik))
                {
                    eldekiEsyalar.Add(yeniEsya);
                    yeniEsya.atanmisEsyaIndeksi = eldekiEsyalar.Count;
                    HanelereYerlestir(i, j, yeniEsya.genislik, yeniEsya.yukseklik, yeniEsya);
                    yeniEsya.KendiniKonumlandir(i-yatay/2, j-dikey/2);
                    return true;
                }
            }
        }

        return false;
    }

    public bool HaneleriKontrolEt(int i, int j, int w, int h)
    {
        for (int ii = i; ii < i+w; ii++)
        {
            for (int jj = j; jj < j+h; jj++)
            {
                if(ii < yatay && jj < dikey)
                {
                    if (birimler[ii, jj].esyaIndeksi != -1)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                
            }
        }

        return true;
    }

    public void HanelereYerlestir(int i, int j, int w, int h, Esya e)
    {
        for (int ii = i; ii < i+w; ii++)
        {
            for (int jj = j; jj < j+h; jj++)
            {
                birimler[ii, jj].esyaIndeksi = e.atanmisEsyaIndeksi;
            }
        }
    }

    public void HanelerdenCikart(int esyaIndeksi)
    {
        for (int i = 0; i < yatay; i++)
        {
            for (int j = 0; j < dikey; j++)
            {
                if(birimler[i,j].esyaIndeksi == esyaIndeksi)
                {
                    birimler[i, j].esyaIndeksi = -1;
                }
            }
        }   
    }

    int sonGecerliX = -1;
    int sonGecerliY = -1;

    private void Update()
    {
        int layerMask = 1;

        // edit: eþya varsa taþýma ve býrakma mantýðýný baþa alýyorum ki aldý & býraktý olmasýn
        if (eldekiEsya != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                GameObject esya = hit.collider.gameObject;
                Birim birimScripti = esya.GetComponent<Birim>();
                if (birimScripti)
                {
                    int x = birimScripti.x;
                    int y = birimScripti.y;
                    if (HaneleriKontrolEt(x, y, eldekiEsya.genislik, eldekiEsya.yukseklik))
                    {
                        eldekiEsya.KendiniKonumlandir(x - yatay / 2, y - dikey / 2);
                        sonGecerliX = x;
                        sonGecerliY = y;
                    }
                }
            }

            // edit: tuþ olarak iki durumda da GetMouseButtonDown daha mantýklý
            if (Input.GetMouseButtonDown(0) && sonGecerliX != -1 && sonGecerliY != -1)
            {
                HanelereYerlestir(sonGecerliX, sonGecerliY, eldekiEsya.genislik, eldekiEsya.yukseklik, eldekiEsya);
                eldekiEsya.gameObject.layer = 0;
                
                eldekiEsya = null;
                sonGecerliX = -1;
                sonGecerliY = -1;
            }    
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, layerMask))
                {
                    GameObject esya = hit.collider.gameObject;
                    Esya esyaScripti = esya.GetComponent<Esya>();
                    if (esyaScripti)
                    {
                        eldekiEsya = esyaScripti;
                        eldekiEsya.gameObject.layer = 5;
                        
                        // edit: eþyayý elimize aldýðýmýz anda yerinden çýkartmazsak eski konumuna koyamýyoruz
                        HanelerdenCikart(eldekiEsya.atanmisEsyaIndeksi);
                    }
                }
            }
        }
    }
}
