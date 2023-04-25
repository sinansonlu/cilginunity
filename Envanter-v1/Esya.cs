using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esya : MonoBehaviour
{
    public string esyaAdi;
    public int genislik;
    public int yukseklik;
    public int atanmisEsyaIndeksi;
    public void KendiniKonumlandir(int i, int j)
    {
        gameObject.transform.position = new Vector3(i,0,j);
    }
}
