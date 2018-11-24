using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailSpawner : MonoBehaviour {
    public GameObject prefab;
    public Transform spawner;

    void Start () {
        InvokeRepeating ("Spawn", Random.Range (0.1f, 1.5f), Random.Range (1f, 2f));
    }

    public void Spawn () {
        Instantiate (prefab, spawner.position, spawner.rotation);
    }
}