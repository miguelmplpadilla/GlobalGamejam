using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRoomSpawner : MonoBehaviour
{
    public float offset = 12;
    public GameObject roomPrefab;
    public int contador = 0;
    public int roomsToSpawn = 500;
    void Start()
    {
        contador = 0;
        for(int i = 0; i < roomsToSpawn; i++)
        {
            SpawnNextRoom();
        }
    }

    private void SpawnNextRoom()
    {
        GameObject room = Instantiate(roomPrefab, transform.TransformPoint(contador * 12, 0, 0), Quaternion.identity, transform );
        room.GetComponent<TileScript>().setNumber(contador+1);
        if (Random.value < 0.3f)
        {
            room.GetComponent<TileScript>().SpawnDoctor();
        }
        contador++;
    }
}
