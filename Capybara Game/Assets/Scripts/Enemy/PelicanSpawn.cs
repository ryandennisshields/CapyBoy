using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelicanSpawn : MonoBehaviour
{
    private float locationNumber;

    private float spawnTimer = 30;

    public Transform left;
    public Transform right;

    public GameObject pelican;

    private Transform spawnPoint;

    [SerializeField] private AudioSource pop;

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            SpawnPelican();
        }
    }

    private void SpawnPelican()
    {
        locationNumber = Random.Range(1, 3); // 2 spawn points, Number between 1 and 2

        pop.Play();

        switch (locationNumber)
        {
            case 1:
                spawnPoint = left;
                Instantiate(pelican, spawnPoint.position, spawnPoint.rotation);
                break;
            case 2:
                spawnPoint = right;
                Instantiate(pelican, spawnPoint.position, spawnPoint.rotation);
                break;
        }
        spawnTimer = 30;
    }
}
