using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContextDisplay : MonoBehaviour {

    public GameObject texturePanel;
    public GameObject blurPanel;

    public void Start () {
        Close ();
    }

    public void DisplayItem (Texture newTexture) {
        texturePanel.SetActive (true);
        blurPanel.SetActive (true);
        texturePanel.GetComponent<MeshRenderer> ().material.SetTexture ("_MainTex", newTexture);
    }

    public void Close () {
        texturePanel.SetActive (false);
        blurPanel.SetActive (false);
    }

}