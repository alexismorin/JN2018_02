using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PitchDetect : MonoBehaviour {

    public AudioSource audios;
    int qSamples = 1024; // array size
    float refValue = 0.1f; // RMS value for 0 dB
    float threshold = 0.02f; // minimum amplitude to extract pitch
    float rmsValue; // sound level - RMS
    public float dbValue; // sound level - dB
    public float pitchValue; // sound pitch - Hz

    float[] samples; // audio samples
    float[] spectrum; // audio spectrum
    float fSample;
    float pitchBuffer;

    Material robotMaterial;

    void Start () {
        samples = new float[qSamples];
        spectrum = new float[qSamples];
        fSample = AudioSettings.outputSampleRate;

        robotMaterial = GetComponent<MeshRenderer> ().material;

    }

    void AnalyzeSound () {
        audios.GetOutputData (samples, 0); // fill array with samples
        int i;
        float sum = 0;
        for (i = 0; i < qSamples; i++) {
            sum += samples[i] * samples[i]; // sum squared samples
        }
        rmsValue = Mathf.Sqrt (sum / qSamples); // rms = square root of average
        dbValue = 20 * Mathf.Log10 (rmsValue / refValue); // calculate dB
        if (dbValue < -160) dbValue = -160; // clamp it to -160dB min
        // get sound spectrum
        audios.GetSpectrumData (spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0;
        int maxN = 0;
        for (i = 0; i < qSamples; i++) { // find max 
            if (spectrum[i] > maxV && spectrum[i] > threshold) {
                maxV = spectrum[i];
                maxN = i; // maxN is the index of max
            }
        }
        float freqN = maxN; // pass the index to a float variable
        if (maxN > 0 && maxN < qSamples - 1) { // interpolate index using neighbours
            var dL = spectrum[maxN - 1] / spectrum[maxN];
            var dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        pitchValue = freqN * (fSample / 2) / qSamples; // convert index to frequency
    }

    void Update () {
        AnalyzeSound ();

        if (dbValue >= pitchBuffer) {
            pitchBuffer = dbValue;

        }

        float ClampedValue = Mathf.Clamp (Mathf.Abs (dbValue) / pitchBuffer, 0f, 1f);

        Color newColor = Color.HSVToRGB (ClampedValue, 0.7f, ClampedValue);
        Color finalColor = newColor * 5f;
        robotMaterial.SetColor ("_EmissionColor", newColor);
    }

}