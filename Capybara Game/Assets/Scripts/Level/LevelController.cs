#define ALLOW_TEST_MODE 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private GameManager gameManager;

    ///////////////// VARIABLES FOR INDICATOR UI /////////////////
    public GameObject[] AvailableIndicators;

    private int _IndicatorsFilled = 0;
    public int IndicatorsFilled
    {
        get { return _IndicatorsFilled; }
        set
        {
            // Limit to 5.
            if (value > AvailableIndicators.Length) return;

            _IndicatorsFilled = value;

            // Perform setting.
            for (int i =0; i < AvailableIndicators.Length; i++)
            {
                AvailableIndicators[i].SetActive(i < value);
            }

            if (value >= AvailableIndicators.Length)
            {
                // Show the win screen!
                gameManager.WinScreenCanvasVisible = true;
            }
        }
    }
    ///////////////// END VARAIBLES FOR INDICATOR UI /////////////////

    ///////////////// VARIABLES FOR RANDOM FOOD SPAWN HANDLING /////////////////
    public GameObject[] FoodSpawnLocations;
    public GameObject[] FoodTypes;

    public AudioSource FoodSpawnSoundFX;

    public int FoodSpawnDelay;

    private float FoodSpawningTimer;
    ///////////////// END VARIABLES FOR RANDOM FOOD SPAWN HANDLING /////////////////

    private void Start()
    {
        // Get gameManager.
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Set timer.
        this.FoodSpawningTimer = FoodSpawnDelay; 
    }

    private void FoodSpawnerUpdate()
    {
        // Reduce timer.
        this.FoodSpawningTimer -= Time.deltaTime;

        // When timer is at zero, perform setting.
        if (this.FoodSpawningTimer <= 0)
        {
            this.FoodSpawner_CreateFood();
            this.FoodSpawningTimer = FoodSpawnDelay;
        }
    }

    private void FoodSpawner_CreateFood()
    {
        GameObject SpawnLocation = null;

        // Get all foods.
        GameObject[] ActiveFoods = GameObject.FindGameObjectsWithTag("Food");

        bool GettingPosition = true;
        while (GettingPosition)
        {
            bool IsFoodNearbyThisPoint = false;
            SpawnLocation = this.FoodSpawnLocations[Random.Range(0, this.FoodSpawnLocations.Length)];
            for (int i = 0; i < ActiveFoods.Length; i++)
            {
                //Debug.Log(ActiveFoods[i].name + " is " + Vector2.Distance(SpawnLocation.GetComponent<Transform>().position, ActiveFoods[i].transform.position).ToString() + "away!");
                if (Vector2.Distance(SpawnLocation.GetComponent<Transform>().position, ActiveFoods[i].transform.position) < 1)
                {
                    IsFoodNearbyThisPoint = true;
                    break;
                }
            }

            if (!IsFoodNearbyThisPoint)
            {
                GettingPosition = false;
                break;
            }
        }
        
        GameObject FoodObject = this.FoodTypes[Random.Range(0, this.FoodTypes.Length)];

        # pragma warning disable CS0165
        Transform SpawnPosition = SpawnLocation.GetComponent<Transform>();
        # pragma warning restore CS0165
        // Play SFX.
        FoodSpawnSoundFX.Play();

        // Create the object with the given parameters.
        Instantiate(FoodObject, SpawnPosition.position, SpawnPosition.rotation);
    }

    private void Update()
    {
        this.FoodSpawnerUpdate();
    }


}
