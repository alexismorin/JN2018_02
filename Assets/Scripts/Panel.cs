using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Panel : MonoBehaviour {

    public GameObject texturePanel;
    public GameObject animatedPanel;
    public TextMeshPro textmesh;

    public void DisplayItem (Texture newTexture, string displayText) {
        animatedPanel.GetComponent<Animator> ().SetTrigger ("Open");
        texturePanel.GetComponent<MeshRenderer> ().material.SetTexture ("_MainTex", newTexture);
        textmesh.text = displayText;
    }

    public void Close () {
        animatedPanel.GetComponent<Animator> ().SetTrigger ("Close");
    }

}