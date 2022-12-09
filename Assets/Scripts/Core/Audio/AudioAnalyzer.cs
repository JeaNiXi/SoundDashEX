using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAnalyzer : MonoBehaviour
{
    private float[] audioSignals;
    private GameObject[] objectsLeft;
    private GameObject[] objectsRight;
    private Vector3 startScale = Vector3.one;
    private float mainTimer;
    private float audioStrengthTimer;
    private bool isAllowedToCheckStrength;

    public float timeStep;
    public float scaleTime;
    public GameObject cubePrefab;
    public AudioSource mainAudioSource;

    public enum AudioSpeed
    {
        Stopping,
        Normal,
        Fast, 
    }
    public AudioSpeed audioSpeed;

    private void Start()
    {
        isAllowedToCheckStrength = true;

        audioSignals = new float[128];
        objectsLeft = new GameObject[audioSignals.Length];
        objectsRight = new GameObject[audioSignals.Length];

        for (int i = 0; i < audioSignals.Length; i++)
        {
            objectsRight[i] = Instantiate(cubePrefab);
            //objectsRight[i].transform.position = Vector3.zero + new Vector3(0.1f, 0, 0) * i;
            objectsRight[i].transform.position = transform.position + new Vector3(0.1f, 0, 0) * i;
            objectsLeft[i] = Instantiate(cubePrefab);
            //objectsLeft[i].transform.position = new Vector3(-0.1f, 0, 0) - new Vector3(0.1f, 0, 0) * i;
            objectsLeft[i].transform.position = new Vector3(-0.1f + transform.position.x, transform.position.y, transform.position.z) - new Vector3(0.1f, 0, 0) * i;
        }
    }

    private void Update()
    {
        UpdatePosition();
        if (isAllowedToCheckStrength && audioStrengthTimer > 0.2f)
        {
            isAllowedToCheckStrength = false;
            GetAudioStrength();
        }
        if (mainTimer > timeStep)
            GetBeat();
        mainTimer += Time.deltaTime;
        audioStrengthTimer += Time.deltaTime;
    }
    private void GetBeat()
    {
        StopCoroutine("EditObjects");
        StartCoroutine(EditObjects(scaleTime, objectsRight, objectsLeft));
        mainTimer = 0;
    }
    private IEnumerator EditObjects(float timeToScale, GameObject[] objectsRight, GameObject[] objectsLeft)
    {
        float timer = 0;
        for (int i = 0; i < objectsRight.Length; i++)
        {
            Vector3 currentScale = objectsRight[i].transform.localScale;
            Vector3 initialScale = currentScale;
            mainAudioSource.GetSpectrumData(audioSignals, 0, FFTWindow.Hamming);
            var currentSignal = audioSignals[i] * 100 + 1;
            Vector3 editedScale = new Vector3(startScale.x, currentSignal, startScale.z);

            while (currentScale != editedScale)
            {
                currentScale = Vector3.Lerp(initialScale, editedScale, timer / timeToScale);
                timer += Time.deltaTime;
                objectsRight[i].transform.localScale = currentScale;
                objectsLeft[i].transform.localScale = currentScale;
                yield return null;
            }
        }
    }
    private void UpdatePosition()
    {
        for (int i = 0; i < audioSignals.Length; i++)
        {
            objectsRight[i].transform.position = transform.position + new Vector3(0.1f, 0, 0) * i;
            objectsLeft[i].transform.position = new Vector3(-0.1f + transform.position.x, transform.position.y, transform.position.z) - new Vector3(0.1f, 0, 0) * i;
        }
    }

    private void GetAudioStrength()
    {
        float strength = 0;
        foreach(float beat in audioSignals)
        {
            strength += beat;
        }
        if (strength > 0 && strength < 0.1f)
            audioSpeed = AudioSpeed.Stopping;
        else if (strength < 1.2f)
            audioSpeed = AudioSpeed.Normal;
        else 
            audioSpeed = AudioSpeed.Fast;
        Debug.Log(strength);
        Debug.Log(audioSpeed);
        isAllowedToCheckStrength = true;
        audioStrengthTimer = 0;
    }
}
