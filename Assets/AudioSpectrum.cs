// Unity Audio Spectrum data analysis
// IMDM Course Material 
// Author: Myungin Lee
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]

public class AudioSpectrum : MonoBehaviour
{
    AudioSource source;
    public static int FFTSIZE = 1024; // https://en.wikipedia.org/wiki/Fast_Fourier_transform
    public static float[] samples = new float[FFTSIZE];
    public static float audioAmp = 0f;
    public static float drum1 = 0f;
    public static float drum2 = 0f;
    public static float vocal3 = 0f;
    public static float bell4 = 0f;
    void Start()
    {
        source = GetComponent<AudioSource>();       
    }
    void Update()
    {
        // The source (time domain) transforms into samples in frequency domain 
        GetComponent<AudioSource>().GetSpectrumData(samples, 0, FFTWindow.Hanning);
        // Empty first, and pull down the value.
        /*audioAmp = 0f;
        for (int i = 0; i < FFTSIZE; i++)
        {
            audioAmp += samples[i];
        }*/

        drum1 = 0f;
        for (int i = 12; i < 26; i++)
        {
            drum1 += samples[i];
        }
        drum1 *= 2f;

        drum2 = 0f;
        for (int i = 15; i < 18; i++)
        {
            drum2 += samples[i];
        }
        drum2 *= 10f;

        vocal3 = 0f;
        for (int i = 12; i < 15; i++)
        {
            vocal3 += samples[i];
        }
        vocal3 *= 10f;

        bell4 = 0f;
        for (int i = 0; i < 1; i++)
        {
            bell4 += samples[i];
        }
    }
}
