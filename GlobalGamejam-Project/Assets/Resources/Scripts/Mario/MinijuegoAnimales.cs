using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinijuegoAnimales : MonoBehaviour
{

    public GameObject[] animales;

    public int indice;

    public GameObject animalesParent;
    public GameObject luz;

    void Start()
    {
        for (int i = 0; i < animales.Length; i++)
            animales[i].SetActive(i == indice);
        
        ApagarLuz();
        Invoke(nameof(EncenderLuz),0.5f);
    }

    public void Next()
    {
        if (indice+1 < animales.Length)indice++;
        else
        {
            CardController.instance.EndGame();
            CancelInvoke(nameof(EncenderLuz));
            return;
        }

        for (int i = 0; i < animales.Length; i++)
            animales[i].SetActive(i == indice);
    }

    public void PulsarBoton()
    {
        if (!luz.activeSelf) return;
        
        ApagarLuz();
        Invoke(nameof(EncenderLuz),0.5f);
        
        Next();
    }
    
    public void ApagarLuz()
    {
        animalesParent.SetActive(false);
        luz.SetActive(false);
    }

    public void EncenderLuz()
    {
        animalesParent.SetActive(true);
        luz.SetActive(true);
    }
}
