using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour {

    public GameObject texturePanel;
    public GameObject animatedPanel;

    public void DisplayItem (Texture newTexture) {
        animatedPanel.GetComponent<Animator> ().SetTrigger ("Open");
        texturePanel.GetComponent<MeshRenderer> ().material.SetTexture ("_MainTex", newTexture);

    }

    public void Close () {
        animatedPanel.GetComponent<Animator> ().SetTrigger ("Close");
    }

}