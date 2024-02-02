using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CajaScript : MonoBehaviour
{
    bool canWin;

    private bool win = false;

    void Start()
    {
        Invoke(nameof(ActiveWin), 2f);
        canWin = false;
    }

    public void ActiveWin()
    {
        canWin = true;
    }
    
    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (win) return;
        
        if (collision.CompareTag("WinLine"))
        {
            if (canWin)
            {
                SceneController.instance.EndGame();
                win = true;
            }
        }
    }
}
