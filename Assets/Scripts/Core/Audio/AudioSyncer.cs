using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncer : MonoBehaviour
{
    public float bias;
    public float timeStep;
    public float timeToBeat;
    public float restSmoothTime;

    private float previousAudioValue;
    protected float audioValue; // changed from private
    private float timer;

    protected bool isBeat;

    private void Update()
    {
        OnUpdate();
    }

    public virtual void OnUpdate()
    {
        previousAudioValue = audioValue;
        audioValue = AudioSpectrum.SpectrumValue;

        //if (previousAudioValue > bias && audioValue <= bias)
        //    if (timer > timeStep)
        //    {
        //        Debug.Log("Call First Beat");
        //        OnBeat();
        //    }
        if (previousAudioValue <= bias && audioValue > bias)
            //if (timer > timeStep)
            //{
                OnBeat();
            //}

        timer += Time.deltaTime;
    }

    public virtual void OnBeat()
    {
        timer = 0;
        isBeat = true;
    }
}
