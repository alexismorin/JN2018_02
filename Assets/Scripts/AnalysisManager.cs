using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class AnalysisManager : MonoBehaviour {

    public TextMeshPro InputTextMesh;
    public TextMeshPro AITextMesh;
    public TextMeshPro ConclusionsTextMesh;
    public Panel panelA;
    public Panel panelB;
    [Space (10)]
    public bool finalQuestion;
    string m_Hypotheses;
    string m_Recognitions;
    DictationRecognizer m_DictationRecognizer;

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

    void AnalyzeVoice (string inputText) {

        InputTextMesh.SetText (inputText);

        if (isListening == true) {

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
                    if (possibleMatchA.Contains (inputText.ToLower ())) {
                        HandleResponse (0);
                        return;
                    }
                }

                foreach (string possibleMatchB in tests[currentTest].OptionB) {
                    if (possibleMatchB.Contains (inputText.ToLower ())) {
                        HandleResponse (1);
                        return;
                    }
                }

                foreach (string possibleMatchC in tests[currentTest].OptionC) {
                    if (possibleMatchC.Contains (inputText.ToLower ())) {
                        HandleResponse (2);
                        return;
                    }
                }
            }
        }
    }

    void Welcome () {
        isListening = false;
        AITextMesh.SetText (welcomeText);
        AIVoiceBox.PlayOneShot (welcomeSound, 1f);
        Invoke ("AskQuestion", welcomeSound.length + 0.5f);
    }

    void AskQuestion () {
        InputTextMesh.SetText (" ");
        currentTest++;
        panelA.DisplayItem (tests[currentTest].OptionATexture);
        panelB.DisplayItem (tests[currentTest].OptionBTexture);

        AITextMesh.SetText (tests[currentTest].QuestionCaption);
        AIVoiceBox.PlayOneShot (tests[currentTest].QuestionAudio, 1f);
        Debug.Log (tests[currentTest].QuestionCaption);
        Invoke ("StartListening", tests[currentTest].QuestionAudio.length);
    }

    void StartListening () {
        isListening = true;
    }

    public void HandleResponse (int response) {
        isListening = false;

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
                case 0:
                    panelB.Close ();
                    AIVoiceBox.PlayOneShot (tests[currentTest].OptionAResponse, 1f);
                    AITextMesh.SetText (tests[currentTest].OptionAConclusion);
                    ConclusionsTextMesh.text += tests[currentTest].OptionAConclusion + "\n" + "\n";
                    Invoke ("NextQuestion", tests[currentTest].OptionAResponse.length + 0.7f);
                    return;
                case 1:
                    panelA.Close ();
                    AIVoiceBox.PlayOneShot (tests[currentTest].OptionBResponse, 1f);
                    AITextMesh.SetText (tests[currentTest].OptionBConclusion);
                    ConclusionsTextMesh.text += tests[currentTest].OptionBConclusion + "\n" + "\n";
                    Invoke ("NextQuestion", tests[currentTest].OptionBResponse.length + 0.7f);
                    return;
                case 2:
                    panelA.Close ();
                    panelB.Close ();
                    AIVoiceBox.PlayOneShot (tests[currentTest].OptionCResponse, 1f);
                    AITextMesh.SetText (tests[currentTest].OptionCConclusion);
                    ConclusionsTextMesh.text += tests[currentTest].OptionCConclusion + "\n" + "\n";
                    currentSubversiveness++;
                    Invoke ("NextQuestion", tests[currentTest].OptionCResponse.length + 0.7f);
                    return;
            }
        }

    }

    public void Restart () {

        AIVoiceBox.PlayOneShot (WeNeedMoreDataSound, 1f);
        finalQuestion = false;
        currentTest = -1;
        Invoke ("NextQuestion", WeNeedMoreDataSound.length + 0.7f);
        return;
    }

    public void TrueEnding () {

        AIVoiceBox.PlayOneShot (TrueEndingSound, 1f);

    }

    public void NextQuestion () {

        InputTextMesh.SetText (" ");
        AITextMesh.SetText (" ");

        if (currentTest < tests.Length - 2) {
            AIVoiceBox.PlayOneShot (NextQuestionSound, 1f);
            Debug.Log ("Next Question");
            Invoke ("AskQuestion", NextQuestionSound.length + 0.5f);
        } else {
            finalQuestion = true;
            AIVoiceBox.PlayOneShot (FinalQuestionSound, 1f);
            Debug.Log ("End of Evaluation");
            Invoke ("AskQuestion", FinalQuestionSound.length + 0.5f);
        }

    }

    void Start () {
        m_DictationRecognizer = new DictationRecognizer ();

        m_DictationRecognizer.DictationResult += (text, confidence) => {
            AnalyzeVoice (text);
            //    Debug.LogFormat ("Dictation result: {0}", text);
        };

        m_DictationRecognizer.DictationHypothesis += (text) => {
            //    Debug.LogFormat ("Dictation hypothesis: {0}", text);
        };

        m_DictationRecognizer.DictationComplete += (completionCause) => {
            //  if (completionCause != DictationCompletionCause.Complete)
            //       Debug.LogErrorFormat ("Dictation completed unsuccessfully: {0}.", completionCause);
        };

        m_DictationRecognizer.DictationError += (error, hresult) => {
            //    Debug.LogErrorFormat ("Dictation error: {0}; HResult = {1}.", error, hresult);
        };

        m_DictationRecognizer.Start ();
    }

    void OnApplicationQuit () {
        m_DictationRecognizer.Dispose ();
    }

}