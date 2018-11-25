using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRobot : MonoBehaviour {

    Material robotMaterial;
    // Start is called before the first frame update
    void Start () {

        robotMaterial = GetComponent<MeshRenderer> ().material;
    }

    // Update is called once per frame
    void Update () {
        Color newColor = Color.HSVToRGB (Random.Range (0f, 1f), 1f, Random.Range (0.8f, 1f));
        Color finalColor = newColor * 5f;
        robotMaterial.SetColor ("_EmissionColor", newColor);
    }
}