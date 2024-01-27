using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public float duracion;
    void Start()
    {
        Invoke(nameof(FinalizarAnim), duracion);
    }   
    void Update()
    {
        
    }

    public void FinalizarAnim()
    {
        print("Finalizar_Anim");
    }
}
