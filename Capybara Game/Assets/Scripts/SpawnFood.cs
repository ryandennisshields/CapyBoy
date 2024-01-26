using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFood : MonoBehaviour
{
    private float locationNumber;
    private float foodNumber;

    private float shootTimer;

    public Transform topLeft;
    public Transform topRight;
    public Transform bottomLeft;
    public Transform bottomRight;
    public Transform middle;

    public GameObject burger;
    public GameObject pasta;
    public GameObject pizza;

    private Transform spawnPoint;

    private bool active;

    void Start()
    {
        
    }

  
    void Update()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer > 15 && !active)
        {
            StartCoroutine(FastFood());
        }
    }

    IEnumerator FastFood()
    {
        active = true;

        locationNumber = Random.Range(1, 5); // 4 spawn points, Number between 1 and 4
        foodNumber = Random.Range(1, 4); // 3 food types, Number between 1 and 3

        if (locationNumber == 1)
        {
            spawnPoint = topLeft;
        }

        if (locationNumber == 2)
        {
            spawnPoint = topRight;
        }

        if (locationNumber == 3)
        {
            spawnPoint = bottomLeft;
        }

        if (locationNumber == 4)
        {
            spawnPoint = bottomRight;
        }

        if (locationNumber == 5)
        {
            spawnPoint = middle;
        }

        if (foodNumber == 1)
        {
            GameObject newFood = Instantiate(burger, spawnPoint.position, Quaternion.Euler(0, 0, 0));
        }

        if (foodNumber == 2)
        {
            GameObject newFood = Instantiate(pizza, spawnPoint.position, Quaternion.Euler(0, 0, 0));
        }

        if (foodNumber == 3)
        {
            GameObject newFood = Instantiate(pasta, spawnPoint.position, Quaternion.Euler(0, 0, 0));
        }

        yield return new WaitForSeconds(shootTimer);

        active = false;
    }
}
