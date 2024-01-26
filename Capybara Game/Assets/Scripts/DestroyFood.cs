using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFood : MonoBehaviour
{
    public float bulletLifetime = 10;

    void Start()
    {
        StartCoroutine(bulletDie());
    }

    IEnumerator bulletDie()
    {
        yield return new WaitForSeconds(bulletLifetime);
        Explode();
    }



    void Explode()
    {
        Destroy(gameObject);
    }
}
