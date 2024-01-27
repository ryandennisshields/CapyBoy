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
                GameObject newBurger = Instantiate(burger, spawnPoint.position, spawnPoint.rotation);
                break;
            case 2:
                GameObject newPizza = Instantiate(pizza, spawnPoint.position, spawnPoint.rotation);
                break;
            case 3:
                GameObject newPasta = Instantiate(pasta, spawnPoint.position, spawnPoint.rotation);
                break;
        }

        yield return new WaitForSeconds(shootTimer);

        active = false;
    }
}
