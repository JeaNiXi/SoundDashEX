using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{

    private float speedTimer = 0;
    private float speedCheckDelay = 1.0f;

    public enum TrackSpeed
    {
        BREAK,
        SLOW,
        MODERATE,
        FASTER,
        HIGHEST,
    }

    public TrackSpeed CurrentSongSpeed;

    public static float SpectrumValue
    {
        get;
        private set;
    }

    private float[] audioSpectrum;

    private void Start()
    {
        audioSpectrum = new float[128];
    }
    private void Update()
    {
        AudioListener.GetSpectrumData(audioSpectrum, 0, FFTWindow.Hamming);
        if (audioSpectrum != null && audioSpectrum.Length > 0)
        {
            SpectrumValue = audioSpectrum[0] * 100;
        }
        
        if (speedTimer > speedCheckDelay)
        {
            CurrentSongSpeed = GetCurrentTrackSpeed();
            Debug.Log(CurrentSongSpeed);
        }
        speedTimer += Time.deltaTime;
    }

    private TrackSpeed GetCurrentTrackSpeed()
    {
        float speed = 0;
        foreach (float x in audioSpectrum)
        {
            speed += x;
        }
        speedTimer = 0;
        Debug.Log(speed);
        if (speed >= 0 && speed <= 0.35f)
            return TrackSpeed.BREAK;
        else if (speed <= 0.6f)
            return TrackSpeed.SLOW;
        else if (speed <= 1.0f)
            return TrackSpeed.MODERATE;
        else if (speed <= 1.5f)
            return TrackSpeed.FASTER;
        else
            return TrackSpeed.HIGHEST;
    }
}
