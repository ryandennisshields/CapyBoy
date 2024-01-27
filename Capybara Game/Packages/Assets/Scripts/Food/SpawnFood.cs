using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFood : MonoBehaviour
{
    private float locationNumber;
    private float foodNumber;

    private float spawnTimer = 15;

    public Transform topLeft;
    public Transform topRight;
    public Transform bottomLeft;
    public Transform bottomRight;
    public Transform middle;

    public GameObject burger;
    public GameObject pasta;
    public GameObject pizza;

    private Transform spawnPoint;
  
    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            FastFood();
        }
    }

    private void FastFood()
    {
        locationNumber = Random.Range(1, 6); // 5 spawn points, Number between 1 and 5
        foodNumber = Random.Range(1, 4); // 3 food types, Number between 1 and 3

        switch (locationNumber)
        {
            case 1:
                spawnPoint = topLeft;
                break;
            case 2:
                spawnPoint = bottomLeft;
                break;
            case 3:
                spawnPoint = middle;
                break;
            case 4:
                spawnPoint = topRight;
                break;
            case 5:
                spawnPoint = bottomRight;
                break;
        }

        switch (foodNumber)
        {
            case 1:
                Instantiate(burger, spawnPoint.position, spawnPoint.rotation);
                break;
            case 2:
                Instantiate(pizza, spawnPoint.position, spawnPoint.rotation);
                break;
            case 3:
                Instantiate(pasta, spawnPoint.position, spawnPoint.rotation);
                break;
        }
        spawnTimer = 15;
    }
}
