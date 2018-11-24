using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour {

    public GameObject panelObject;

    public void DisplayItem (Texture newTexture) {
        panelObject.GetComponent<MeshRenderer> ().material.SetTexture ("_MainTex", newTexture);
        GetComponent<Animator> ().SetTrigger ("Open");
    }

}