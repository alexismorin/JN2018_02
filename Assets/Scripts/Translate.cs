﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour {
    // Start is called before the first frame update

    float speed = 1f;
    void Start () {
        Invoke ("Delete", 20f);
        speed = Random.Range (1f, 4f);
    }

    // Update is called once per frame
    void Update () {
        transform.Translate (Vector3.forward * Time.deltaTime * speed);
    }

    void Delete () {
        Destroy (gameObject);
    }
}