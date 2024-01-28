using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public static CarSpawner instance;
    public GameObject[] cars;

    public float yRange;

    private int contador;
    public int maxCars = 20;
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
        SpawnCar();
        contador++;
        StartCoroutine(SpawningCars(2f));
    }

    IEnumerator SpawningCars(float delay)
    {
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
        GameObject currBall = Instantiate(cars[Random.Range(0, cars.Length)], transform);
        currBall.transform.localPosition = new Vector3(currBall.transform.localPosition.x, currBall.transform.localPosition.y + Random.Range(-yRange, yRange+10), currBall.transform.localPosition.z);
    }
}
