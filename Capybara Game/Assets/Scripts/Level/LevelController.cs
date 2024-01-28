#define ALLOW_TEST_MODE 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

            // Limit between 0-5.
            if (value < 0) return;

            // Disable ability to reset this way.
            if (_IndicatorsFilled == AvailableIndicators.Length) return;

            // Get previous value so we can calculate against it.
            int PreviousIndicatorFilled = _IndicatorsFilled;

            _IndicatorsFilled = value;

            if (value > PreviousIndicatorFilled)
            {
                Debug.Log("Point has been added, would flash news point.");
                // Set points before it.
                for (int i = 0; i < value-1; i++)
                {
                    AvailableIndicators[i].SetActive(true);
                }
                StartCoroutine(_FlashPointIndicator(AvailableIndicators[value-1], true));
            } else
            {
                Debug.Log("Point has been removed, would flash removed point");
                // Get the removed point.
                GameObject RemovedIndicatorObject = AvailableIndicators[value];
                RemovedIndicatorObject.SetActive(true);

                // Remove points after it;
                for (int i = value; i < AvailableIndicators.Length; i++) { 
                    AvailableIndicators[i].SetActive(false);
                }
                StartCoroutine(_FlashPointIndicator(AvailableIndicators[value], false));
            }

            if (value >= AvailableIndicators.Length)
            {
                // Show the win screen!
                gameManager.WinScreenCanvasVisible = true;
            }
        }
    }
    ///////////////// END VARAIBLES FOR INDICATOR UI /////////////////

    ///////////////// VARIABLES FOR FOOD HOLDING UI / NEST HANDLER /////////////////
    public GameObject FoodHoldingUIElement;
    public GameObject NestCollider;

    public AudioSource CrunchSoundFX;
    public AudioSource NestDepositSoundFX;

    public bool PlayerIsHoldingItem;
    public GameObject PlayerHoldingItemType;

    
    ///////////////// END VARIABLES FOR FOOD HOLDING UI / NEST HANDLER /////////////////
    
    ///////////////// VARIABLES FOR RANDOM FOOD SPAWN HANDLING /////////////////
    public GameObject[] FoodSpawnLocations;
    public GameObject[] FoodTypes;

    public AudioSource FoodSpawnSoundFX;

    public int FoodSpawnDelay;

    private float FoodSpawningTimer;
    private bool IsFirstFoodSpawn = true;
    ///////////////// END VARIABLES FOR RANDOM FOOD SPAWN HANDLING /////////////////

    ///////////////// VARIABLES FOR TIMER HANDLING /////////////////
    public int PlayerTimeToComplete;
    public GameObject TimerUIElement;
    
    private bool RunTimerDown = true;
    private float CompletionTimer;
    ///////////////// END VARIABLES FOR TIMER HANDLING /////////////////

    private void Start()
    {
        // Get gameManager.
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Set timer.
        this.FoodSpawningTimer = FoodSpawnDelay;
        this.CompletionTimer = this.PlayerTimeToComplete;
    }

    IEnumerator _FlashPointIndicator(GameObject indicatorToFlash, bool state)
    {

        Debug.Log("Flashy flashy flash");
        for (int i = 0;i < (state ? 2 : 3); i++)
        {
            indicatorToFlash.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            indicatorToFlash.SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }

        indicatorToFlash.SetActive(state);
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

    private void CompletionTimerUpdate()
    {
        // Don't run the timer if it's disabled.
        if (!this.RunTimerDown)
        {
            return;
        }

        // Reduce timer.
        this.CompletionTimer -= Time.deltaTime;

        // Perform UI update.
        this.TimerUIElement.GetComponent<Text>().text = TimeSpan.FromSeconds((double)CompletionTimer).ToString(@"mm\:ss");

        if (this.CompletionTimer <= 0)
        {
            // Player has failed probably. Submit fail event.
            this.RunTimerDown = false;
            gameManager.FailScreenCanvasVisible = true;
        }
    }

# if ALLOW_TEST_MODE
    private void DebugController()
    {
        // DEBUG FUNCTION: Increase time by 10 seconds when button pressed.
        // Control: A
        if (Input.GetAxisRaw("Left Trigger") == 0 && Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            this.CompletionTimer += 10;
        }

        // DEBUG FUNCTION: Decrease time by 10 seconds when button pressed.
        // Control: B
        if (Input.GetAxisRaw("Left Trigger") == 0 && Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            this.CompletionTimer -= 10;
        }

        // DEBUG FUNCTION: Add to indicator when button pressed.
        // Control: LT + A
        if (Input.GetAxisRaw("Left Trigger") == -1 && Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            this.IndicatorsFilled++;
        }

        // DEBUG FUNCTION: Remove from indicator when button pressed.
        // Control: LT + B
        if (Input.GetAxisRaw("Left Trigger") == -1 && Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            this.IndicatorsFilled--;
        }

        // DEBUG FUNCTION: Instantly fail when button pressed.
        // Control: LT + RB + LS
        if (Input.GetAxisRaw("Left Trigger") == -1 && Input.GetKey(KeyCode.Joystick1Button5) && Input.GetKeyDown(KeyCode.Joystick1Button9))
        {
            this.CompletionTimer = 1;
        }
    }
# endif

    private void FoodSpawner_CreateFood()
    {
        // Use the first spawn location if this is the first food spawn, otherwise pass to the food handler.
        GameObject SpawnLocation = this.IsFirstFoodSpawn ? this.FoodSpawnLocations[0] : null;

        // Only try and get if we are not the first spawn location.
        if (SpawnLocation == null)
        {
            // Get all foods.
            GameObject[] ActiveFoods = GameObject.FindGameObjectsWithTag("Food");

            bool GettingPosition = true;
            while (GettingPosition)
            {
                bool IsFoodNearbyThisPoint = false;
                SpawnLocation = this.FoodSpawnLocations[UnityEngine.Random.Range(0, this.FoodSpawnLocations.Length)];
                for (int i = 0; i < ActiveFoods.Length; i++)
                {
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
        }
        
        GameObject FoodObject = this.FoodTypes[UnityEngine.Random.Range(0, this.FoodTypes.Length)];

        # pragma warning disable CS0165
        Transform SpawnPosition = SpawnLocation.GetComponent<Transform>();
        # pragma warning restore CS0165
        // Play SFX.
        FoodSpawnSoundFX.Play();

        // Create the object with the given parameters.
        GameObject newFood = Instantiate(FoodObject, SpawnPosition.position, SpawnPosition.rotation);

        // Ensure that the first food doesn't destroy itself.
        if (this.IsFirstFoodSpawn)
        {
            this.IsFirstFoodSpawn = false;
            newFood.GetComponent<DestroyFood>().IsStarterFood = true;
        }

        // Tie into Nest Handler, don't enable collisions if food isn't meant to be picked up
        if (this.PlayerIsHoldingItem)
        {
            newFood.GetComponent<Collider2D>().enabled = false;
        }

        // Set name to allow the PlayerEatFood function to find the Prefab.
        newFood.name = FoodObject.name;
    }

    private void _UpdateFoodUI()
    {
        // Get Image from UI element.
        Image uiElementImage = this.FoodHoldingUIElement.GetComponent<Image>();

        // Don't do anything if player is not holding item (Food UI element will be INACTIVE).
        if (!this.PlayerIsHoldingItem)
        {
            uiElementImage.enabled = false;
            return;
        }

        // Get sprite renderer from prefab.
        Sprite prefabSprite = this.PlayerHoldingItemType.GetComponent<SpriteRenderer>().sprite;

        // Set current image to prefab element.
        uiElementImage.sprite = prefabSprite;

        // Finally, show image.
        uiElementImage.enabled = true;
    }

    public void PlayerEatFood(GameObject foodObject)
    {
        // Player has eaten a food.

        // TODO: perhaps provide audible feedback that player cannot pick more than
        // one item up?
        if (PlayerIsHoldingItem)
        {
            return;
        }

        // Player not holding item at this point, make them hold item.
        // Get item Prefab (not the cloned object!)
        GameObject itemPrefab = null;
        for (int i = 0; i < this.FoodTypes.Length; i++)
        {
            if (this.FoodTypes[i].name == foodObject.name)
            {
                itemPrefab = this.FoodTypes[i];
                break;
            }
        }

        // Handle a can't-happen scenario where an invalid food has been consumed.
        if (itemPrefab == null)
        {
            Debug.LogWarning("Tried to eat an invalid food item (name: " + foodObject.name + ")");
            return;
        }

        // Set player holding item type as it is known not to be null.
        PlayerIsHoldingItem = true;
        PlayerHoldingItemType = itemPrefab;

        // Make all food uncollidable.
        GameObject[] allFood = GameObject.FindGameObjectsWithTag("Food");
        for (int i = 0; i < allFood.Length; i++)
        {
            allFood[i].GetComponent<Collider2D>().enabled = false;
        }

        // Play crunch sound & destroy the food as all checks have been performed.
        this.CrunchSoundFX.Play();
        Destroy(foodObject);

        // Call UI update event.
        this._UpdateFoodUI();

        // Enable the nest collider.
        this.NestCollider.GetComponent<Collider2D>().enabled = true;
    }

    // Handle nest collider.
    public void PlayerDepositFood()
    {
        // Only fire if player has item.
        if (!PlayerIsHoldingItem)
        {
            return;
        }

        // Deposit food in nest, obtain point.
        PlayerIsHoldingItem = false;
        PlayerHoldingItemType = null;

        // Update UI and increment the indicator.
        _UpdateFoodUI();
        this.IndicatorsFilled++;

        // Play sound effect.
        this.NestDepositSoundFX.Play();

        // Allow food to be consumed again.
        GameObject[] allFood = GameObject.FindGameObjectsWithTag("Food");
        for (int i = 0; i < allFood.Length; i++)
        {
            allFood[i].GetComponent<Collider2D>().enabled = true;
        }

        // Also disable the nest collider.
        this.NestCollider.GetComponent<Collider2D>().enabled = false;
    }

    private void Update()
    {
        this.FoodSpawnerUpdate();
        this.CompletionTimerUpdate();

#if ALLOW_TEST_MODE
        this.DebugController();
#endif
    }


}
