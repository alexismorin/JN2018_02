using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

[CreateAssetMenu (fileName = "Test Case Asset", menuName = "Test Case", order = 1)]

public class TestCase : ScriptableObject {

    public AudioClip QuestionAudio;
    public string QuestionCaption = "";
    public Texture OptionATexture;
    public Texture OptionBTexture;
    public string OptionCHint = "";
    [Space (10)]

    public AudioClip OptionAResponse;
    public string OptionAConclusion;
    public string[] OptionA;

    [Space (10)]
    public AudioClip OptionBResponse;
    public string OptionBConclusion;
    public string[] OptionB;

    [Space (10)]
    public AudioClip OptionCResponse;
    public string OptionCConclusion;
    public string[] OptionC;
}