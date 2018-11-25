using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class MainMenuAnalysis : MonoBehaviour {

    public DictationRecognizer m_DictationRecognizer;
    public string trigger = "human";

    void AnalyzeVoice (string inputText) {

        if (inputText.ToLower ().Contains (trigger)) {
            m_DictationRecognizer.Dispose ();
            SceneManager.LoadScene ("Main", LoadSceneMode.Single);
        }
    }

    void Start () {
        m_DictationRecognizer = new DictationRecognizer ();
        m_DictationRecognizer.InitialSilenceTimeoutSeconds = 999999f;
        m_DictationRecognizer.AutoSilenceTimeoutSeconds = 999999f;

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
            Debug.LogErrorFormat ("Dictation error: {0}; HResult = {1}.", error, hresult);
        };

        m_DictationRecognizer.Start ();
    }

}