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
        {
            if (i == indice)
            {
                animales[i].SetActive(true);
            }
            else
            {
                animales[i].SetActive(false);
            }
        }
    }

    public void Next()
    {
        if (indice < animales.Length)indice++;
        else
        {
            print("FinalizarMinijuego");
        }

        for(int i = 0; i < animales.Length; i++) 
        {
            if (i == indice)
            {
                animales[i].SetActive(true);
            }
            else
            {
                animales[i].SetActive(false);
            }
        }
    }

    public void PulsarBoton()
    {
        ApagarLuz();
        Next();

        Invoke(nameof(EncenderLuz),0.5f);
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
    void Update()
    {
        
    }
}
