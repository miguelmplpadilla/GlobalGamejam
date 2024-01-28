using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MataBoles : MonoBehaviour
{
    public int bolesMataes;
    void Start()
    {
        bolesMataes = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        bolesMataes++;
        if (bolesMataes >= BallSpawner.instance.maxBalls) ExamGame.instance.EndGame();
    }
}
