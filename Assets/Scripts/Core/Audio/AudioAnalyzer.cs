using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAnalyzer : MonoBehaviour
{
    private float[] audioSignals;
    private GameObject[] objectsLeft;
    private GameObject[] objectsRight;

    public GameObject cubePrefab;

    private void Start()
    {
        audioSignals = new float[128];
        objectsLeft = new GameObject[audioSignals.Length];
        objectsRight = new GameObject[audioSignals.Length];

        for (int i = 0; i < audioSignals.Length; i++)
        {
            objectsRight[i] = Instantiate(cubePrefab);
            objectsRight[i].transform.position = Vector3.zero + new Vector3(0.1f, 0, 0) * i;
        }
    }

    private void Update()
    {
        
    }
}
