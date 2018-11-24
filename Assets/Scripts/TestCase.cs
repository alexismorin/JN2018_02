using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

[CreateAssetMenu (fileName = "Test Case Asset", menuName = "Test Case", order = 1)]

public class TestCase : ScriptableObject {

    public AudioClip QuestionAudio;
    public string QuestionCaption = "";
    public string QuestionCaptionLong = "";
    public Texture OptionATexture;
    public Texture OptionBTexture;
    public Texture ContextTexture;
    [Space (10)]
    public string OptionACaption;
    public string OptionAWord;
    public AudioClip OptionAResponse;
    public string OptionAConclusion;
    public string[] OptionA;

    [Space (10)]
    public string OptionBCaption;
    public string OptionBWord;
    public AudioClip OptionBResponse;
    public string OptionBConclusion;
    public string[] OptionB;

    [Space (10)]
    public string OptionCCaption;
    public string OptionCWord;
    public AudioClip OptionCResponse;
    public string OptionCConclusion;
    public string[] OptionC;
}