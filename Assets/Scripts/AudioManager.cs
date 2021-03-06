﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource speaker;
    public AudioClip[] tracks;

    void Start () {
        InvokeRepeating ("AdjustPitch", 1, 6f);
    }

    public void StartMusic () {
        speaker.clip = tracks[Random.Range (0, tracks.Length)];
        speaker.Play ();

    }

    public void AdjustPitch () {
        speaker.pitch = Random.Range (0.8f, 1.2f);
    }

}