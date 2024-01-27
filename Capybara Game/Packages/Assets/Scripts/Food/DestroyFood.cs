using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFood : MonoBehaviour
{
    public float foodLifetime = 10;

    void Start()
    {
        StartCoroutine(foodExpire());
    }

    IEnumerator foodExpire()
    {
        yield return new WaitForSeconds(foodLifetime);
        Expire();
    }

    void Expire()
    {
        Destroy(gameObject);
    }
}
