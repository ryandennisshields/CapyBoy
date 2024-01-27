using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerStub : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        // Only run this stub if GameManager isn't instantiated.
        if (object.ReferenceEquals(GameObject.Find("GameManager"), null))
        {
            // Instantiate game manager.
            this.gameObject.AddComponent<GameManager>();
            this.gameObject.name = "GameManager";
        }

        // Destroy myself now. My work here is done.
        Destroy(this.gameObject.GetComponent<GameManagerStub>());
    }

}
