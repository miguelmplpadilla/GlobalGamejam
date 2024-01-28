using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerClouds : MonoBehaviour
{
    public static SpawnerClouds instance;
    public GameObject[] clouds;
    public GameObject cloudDick;

    public float yMinRange;
    public float yMaxRange;

    public bool spawning;
    public float velocity;

    public float dickProb;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        spawning = true;
        StartSpawning();
    }
    public void StartSpawning()
    {
        StartCoroutine(SpawningClouds(2f));
    }

    IEnumerator SpawningClouds(float delay)
    {
        if (!spawning) yield break;
        yield return new WaitForSeconds(delay);
        SpawnCloud();
        StartCoroutine(SpawningClouds(Random.Range(4f, 8f)));
    }
    // Update is called once per frame
    private void SpawnCloud()
    {
        if (!spawning) return;
        if (Random.value < dickProb)
        {
            GameObject currBall = Instantiate(cloudDick, transform);
            currBall.transform.localPosition = new Vector3(currBall.transform.localPosition.x, currBall.transform.localPosition.y + Random.Range(-yMinRange, yMaxRange), currBall.transform.localPosition.z);
        }
        else
        {
            GameObject currBall = Instantiate(clouds[Random.Range(0, clouds.Length)], transform);
            currBall.transform.localPosition = new Vector3(currBall.transform.localPosition.x, currBall.transform.localPosition.y + Random.Range(-yMinRange, yMaxRange), currBall.transform.localPosition.z);
        }
    }
}
