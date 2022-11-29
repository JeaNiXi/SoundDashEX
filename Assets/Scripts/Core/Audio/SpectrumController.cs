using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumController : MonoBehaviour
{
    public float scale = 100;
    public float baseHeight = 1;
    public int visible = 128;
    public float observe = 2;

    public AudioSource audioSource;
    public GameObject cube;

    public float[] signals;
    public GameObject[] objectLeft;
    public GameObject[] objectRight;
    public Vector3 localScale;
    public float[] lastScales;

    private void Start()
    {
        signals = new float[128];
        objectLeft = new GameObject[signals.Length];
        objectRight = new GameObject[signals.Length];

        lastScales = new float[signals.Length];
        localScale = cube.transform.localScale;

        for (int i = 0; i < visible; i++)
        {
            cube.transform.position = Vector3.zero + new Vector3(localScale.x + .1f, 0, 0) * i;
            objectLeft[i] = Instantiate(cube);

            cube.transform.position = Vector3.zero - new Vector3(localScale.x + .1f, 0, 0) * i;
            objectRight[i] = Instantiate(cube);
        }
    }

    private void Update()
    {
        audioSource.GetSpectrumData(signals, 0, FFTWindow.Rectangular);
        for (int i = 0; i < visible; i++)
        {
            var current = signals[i] * scale + baseHeight;
            objectLeft[i].transform.localScale = new Vector3(localScale.x, current, localScale.z);
            objectRight[i].transform.localScale = new Vector3(localScale.x, current, localScale.z);
            lastScales[i] = current;
        }
    }
}
