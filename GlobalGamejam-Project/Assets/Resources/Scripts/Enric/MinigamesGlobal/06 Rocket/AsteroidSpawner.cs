using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public static AsteroidSpawner instance;
    public GameObject[] cars;
    public GameObject giantAsteroid;

    public float yMinRange;
    public float yMaxRange;

    private int contador;
    public int maxCars = 20;
    public bool spawning;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        contador = 0;
        spawning = true;
    }
    public void StartSpawning()
    {
        SpawnCar();
        contador++;
        StartCoroutine(SpawningCars(2f));
        StartCoroutine(SpawnGiant(17));
    }

    IEnumerator SpawningCars(float delay)
    {
        if (!spawning) yield break;
        yield return new WaitForSeconds(delay);
        SpawnCar();
        contador++;
        if (contador < maxCars)
        {
            StartCoroutine(SpawningCars(Random.Range(1f, 2f)));
        }
    }
    // Update is called once per frame
    private void SpawnCar()
    {
        if (!spawning) return;
        GameObject currBall = Instantiate(cars[Random.Range(0, cars.Length)], transform);
        currBall.transform.localPosition = new Vector3(currBall.transform.localPosition.x, currBall.transform.localPosition.y + Random.Range(-yMinRange, yMaxRange), currBall.transform.localPosition.z);
    }

    IEnumerator SpawnGiant(int delay)
    {
        yield return new WaitForSeconds(delay);
        spawning = false;
        GameObject giantAst = Instantiate(giantAsteroid, transform);
    }
}
