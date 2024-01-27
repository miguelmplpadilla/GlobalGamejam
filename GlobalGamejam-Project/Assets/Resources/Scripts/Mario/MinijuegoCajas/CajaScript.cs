using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CajaScript : MonoBehaviour
{
    bool canWin;

    void Start()
    {
        Invoke(nameof(ActiveWin), 1f);
        canWin = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveWin()
    {
        canWin = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WinLine"))
        {
            print("HasGanado");
        }
    }
}
