using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MinijuegoCajas : MonoBehaviour
{
    public Animator anim;

    public GameObject prefabCaja;
    public GameObject canvas;

    public bool canSpawn;
    public GameObject cajaVisual;

    void Start()
    {
        canSpawn = true;
        //Invoke(nameof(FinalizarMinijuego), 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space)) 
        {
            if (canSpawn)
            {
                GameObject instanciaCaja = Instantiate(prefabCaja, transform.position, quaternion.identity);
                instanciaCaja.transform.SetParent(canvas.transform, true);
                canSpawn = false;
                Invoke(nameof(UnFreeze), 0.4f);
                cajaVisual.SetActive(false);
                
            }

        }

        cajaVisual.SetActive(canSpawn);

        
    }

    public void UnFreeze()
    {
        canSpawn = true;
        cajaVisual.SetActive(true);
        //anim.SetTrigger("Spawn");
    }

    public void FinalizarMinijuego()
    {

    }
}
