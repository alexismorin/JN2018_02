using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class AnalysisManager : MonoBehaviour {

    public TextMeshPro InputTextMesh;
    public TextMeshPro AITextMesh;
    public TextMeshPro AILongFormatTextMesh;
    public TextMeshPro ConclusionsTextMesh;
    public TextMeshPro HintTextMesh;

    public GameObject ConclusionsObject;
    public GameObject Credits;
    public Panel panelA;
    public Panel panelB;
    public ContextDisplay ContextPanel;
    public MeshRenderer RedLight;
    public Material RedlightMaterial;
    public Color RedColor;
    public Color BlackColor;
    [Space (10)]
    public bool finalQuestion;
    string m_Hypotheses;
    string m_Recognitions;
    public DictationRecognizer m_DictationRecognizer;
    public AudioManager audioPlayer;
    [Space (10)]
    public AudioSource CameraAudio;
    public Animator CameraAnimation;

    public bool isListening = true;
    public AudioSource AIVoiceBox;
    [Space (10)]
    public AudioClip welcomeSound;
    public string welcomeText;
    public AudioClip NextQuestionSound;
    public AudioClip FinalQuestionSound;
    public AudioClip WeNeedMoreDataSound;
    public AudioClip TrueEndingSound;
    public string startTriggerKeyword = "start";
    [Space (10)]
    public TestCase[] tests;
    public int currentTest = 0;
    public int currentSubversiveness = 0;
    public int subersiveRequirement = 1;
    public Animator RailAnimator;
    public AudioSource RailSounds;
    bool gameStarted;

    void AnalyzeVoice (string inputText) {

        if (isListening == true) {
            InputTextMesh.SetText (inputText);
            // Begin Game Stuff
            if (currentTest == -1) {
                if (inputText.Contains (startTriggerKeyword)) {
                    Welcome ();
                    return;
                }
            }
            //Analyse Anwser
            else {
                foreach (string possibleMatchA in tests[currentTest].OptionA) {
                    if (inputText.ToLower ().Contains (possibleMatchA)) {
                        HandleResponse (0);
                        return;
                    }
                }

                foreach (string possibleMatchB in tests[currentTest].OptionB) {
                    if (inputText.ToLower ().Contains (possibleMatchB)) {
                        HandleResponse (1);
                        return;
                    }
                }

                foreach (string possibleMatchC in tests[currentTest].OptionC) {
                    if (inputText.ToLower ().Contains (possibleMatchC)) {
                        HandleResponse (2);
                        return;
                    }
                }
            }
        }
    }

    void Welcome () {
        ClearDictation ();
        gameStarted = true;
        audioPlayer.StartMusic ();
        isListening = false;
        RedlightMaterial.SetColor ("_EmissionColor", BlackColor);
        AITextMesh.SetText (welcomeText);
        AIVoiceBox.PlayOneShot (welcomeSound, 1f);
        Invoke ("AskQuestion", welcomeSound.length + 0.5f);
    }

    void AskQuestion () {

        InputTextMesh.SetText (" ");
        currentTest++;
        panelA.DisplayItem (tests[currentTest].OptionATexture, tests[currentTest].OptionAWord);
        panelB.DisplayItem (tests[currentTest].OptionBTexture, tests[currentTest].OptionBWord);
        ContextPanel.DisplayItem (tests[currentTest].ContextTexture);

        if (currentTest == tests.Length - 1) {
            ConclusionsObject.SetActive (true);
        }

        AITextMesh.SetText (tests[currentTest].QuestionCaption);
        AILongFormatTextMesh.SetText (tests[currentTest].QuestionCaptionLong);
        AIVoiceBox.PlayOneShot (tests[currentTest].QuestionAudio, 1f);
        Debug.Log (tests[currentTest].QuestionCaption);
        ComposeArray ();
        Invoke ("StartListening", tests[currentTest].QuestionAudio.length);
    }

    void ComposeArray () {
        string ComposedArray = "";
        string[] clueArray = (tests[currentTest].OptionA.Concat (tests[currentTest].OptionB).ToArray ()).Concat (tests[currentTest].OptionC).ToArray ();
        for (var i = 0; i < clueArray.Length; i++) {
            ComposedArray += clueArray[i].ToString () + " - ";
        }
        HintTextMesh.text = ComposedArray + ComposedArray + ComposedArray + ComposedArray + ComposedArray + ComposedArray;
    }

    void StartListening () {
        isListening = true;
        RedlightMaterial.SetColor ("_EmissionColor", RedColor * 2f);
        ReStartDictation ();
    }

    public void HandleResponse (int response) {
        ClearDictation ();
        isListening = false;
        RedlightMaterial.SetColor ("_EmissionColor", BlackColor);
        if (finalQuestion == true) {

            AITextMesh.SetText (" ");

            if (currentSubversiveness >= subersiveRequirement) {
                TrueEnding ();

            } else {
                switch (response) {
                    case 0:
                        Restart ();
                        return;
                    case 1:
                        Restart ();
                        return;
                    case 2:
                        Restart ();
                        return;
                }
            }

        } else {
            switch (response) {
                case 2:
                    AIVoiceBox.PlayOneShot (tests[currentTest].OptionCResponse, 1f);
                    AILongFormatTextMesh.SetText (tests[currentTest].OptionCCaption);
                    ConclusionsTextMesh.text += tests[currentTest].OptionCConclusion + "\n" + "\n";
                    currentSubversiveness++;
                    Invoke ("NextQuestion", tests[currentTest].OptionCResponse.length + 0.7f);
                    return;
                case 0:

                    AIVoiceBox.PlayOneShot (tests[currentTest].OptionAResponse, 1f);
                    AILongFormatTextMesh.SetText (tests[currentTest].OptionACaption);
                    ConclusionsTextMesh.text += tests[currentTest].OptionAConclusion + "\n" + "\n";
                    Invoke ("NextQuestion", tests[currentTest].OptionAResponse.length + 0.7f);
                    return;
                case 1:
                    AIVoiceBox.PlayOneShot (tests[currentTest].OptionBResponse, 1f);
                    AILongFormatTextMesh.SetText (tests[currentTest].OptionBCaption);
                    ConclusionsTextMesh.text += tests[currentTest].OptionBConclusion + "\n" + "\n";
                    Invoke ("NextQuestion", tests[currentTest].OptionBResponse.length + 0.7f);
                    return;
            }
        }

    }

    public void Restart () {

        AIVoiceBox.PlayOneShot (WeNeedMoreDataSound, 1f);
        finalQuestion = false;
        currentTest = -1;
        ConclusionsTextMesh.text = "";
        Invoke ("NextQuestion", WeNeedMoreDataSound.length + 0.7f);
        return;
    }

    public void TrueEnding () {
        ConclusionsObject.SetActive (true);
        AIVoiceBox.PlayOneShot (TrueEndingSound, 1f);
        Invoke ("Recall", TrueEndingSound.length);
    }

    public void Recall () {
        CameraAnimation.SetTrigger ("End");
        CameraAudio.Play ();
        Invoke ("OpenCredits", 3f);
        Invoke ("LoadMenu", 8f);
    }

    public void OpenCredits () {
        Credits.SetActive (true);
    }

    public void LoadMenu () {
        SceneManager.LoadScene ("MainMenu", LoadSceneMode.Single);
    }

    public void NextQuestion () {
        ConclusionsObject.SetActive (false);
        RailSounds.Play ();
        audioPlayer.StartMusic ();
        panelA.Close ();
        panelB.Close ();
        RailAnimator.ResetTrigger ("Go");
        RailAnimator.SetTrigger ("Go");
        ContextPanel.Close ();
        HintTextMesh.text = " ";
        InputTextMesh.SetText (" ");
        AITextMesh.SetText (" ");
        AILongFormatTextMesh.SetText (" ");

        if (currentTest < tests.Length - 2) {
            AIVoiceBox.PlayOneShot (NextQuestionSound, 1f);
            Debug.Log ("Next Question");
            Invoke ("AskQuestion", NextQuestionSound.length + 0.5f);
        } else {
            if (currentSubversiveness >= subersiveRequirement) {
                TrueEnding ();
            } else {

                finalQuestion = true;
                AIVoiceBox.PlayOneShot (FinalQuestionSound, 1f);
                Debug.Log ("End of Evaluation");
                Invoke ("AskQuestion", FinalQuestionSound.length + 0.5f);
            }
        }

    }

    void Start () {
        RedlightMaterial = RedLight.material;
        RailSounds.Play ();
        ResetDictation ();
    }

    void ClearDictation () {
        m_DictationRecognizer.Stop ();
        print ("Dictation is stopped");
        print (m_DictationRecognizer.Status);
    }

    void OnApplicationFocus (bool hasFocus) {
        if (gameStarted) {
            ReStartDictation ();
        }

    }

    void OnApplicationPause (bool pauseStatus) {
        if (gameStarted) {
            ClearDictation ();
        }
    }

    public void ReStartDictation () {
        m_DictationRecognizer.Start ();
    }

    public void ResetDictation () {

        m_DictationRecognizer = new DictationRecognizer ();
        m_DictationRecognizer.InitialSilenceTimeoutSeconds = 999991f;
        m_DictationRecognizer.AutoSilenceTimeoutSeconds = 999991f;

        m_DictationRecognizer.DictationResult += (text, confidence) => {
            AnalyzeVoice (text);
        };

        m_DictationRecognizer.Start ();

        print ("Dictation is started");
        print (m_DictationRecognizer.Status);
    }

    private void Update () {
        if (Input.GetKeyUp (KeyCode.X)) {
            print (m_DictationRecognizer.Status);
            print (m_DictationRecognizer.InitialSilenceTimeoutSeconds);
            print (m_DictationRecognizer.AutoSilenceTimeoutSeconds);
        }
    }
}