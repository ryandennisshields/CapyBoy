using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFood : MonoBehaviour
{
    public float foodLifetime = 10;

    // The initial food spawn must not expire.
    public bool IsStarterFood;

    void Awake()
    {
        // Don't run this script on starter food!
        if (IsStarterFood)
        {
            Destroy(this.gameObject.GetComponent<DestroyFood>());
        }
    }

    void Start()
    {
        if (!IsStarterFood)
        {
            StartCoroutine(foodExpire());
        }
    }

    IEnumerator foodExpire()
    {
        yield return new WaitForSeconds(foodLifetime);
        Expire();
    }

    void Expire()
    {
        if (!IsStarterFood)
        {
            Destroy(this.gameObject);
        }
    }
}
