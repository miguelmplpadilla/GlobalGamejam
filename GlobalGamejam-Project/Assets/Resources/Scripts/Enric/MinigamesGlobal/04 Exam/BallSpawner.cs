using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public static BallSpawner instance;
    public GameObject ball;

    public float yRange;

    private int contador;
    public int maxBalls = 20;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        contador = 0;
    }
    public void StartSpawning()
    {
        SpawnBall();
        contador++;
        StartCoroutine(SpawningBalls(1f));
    }

    IEnumerator SpawningBalls(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnBall();
        contador++;
        if (contador < maxBalls)
        {
            StartCoroutine(SpawningBalls(Random.Range(.3f,1f)));
        }
    }
    // Update is called once per frame
    private void SpawnBall()
    {
        GameObject currBall = Instantiate(ball, transform);
        currBall.transform.localPosition = new Vector3(currBall.transform.localPosition.x, currBall.transform.localPosition.y + Random.Range(-yRange, yRange), currBall.transform.localPosition.z);
    }
}
