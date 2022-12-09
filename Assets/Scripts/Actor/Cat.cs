using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    [SerializeField] private float startForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallForce;
    [SerializeField] private float moveDelay;

    [SerializeField] private AudioAnalyzer audioAnalyzer;

    [SerializeField] private float currentSpeed = 0;

    private Rigidbody2D rbCat;
    private Vector3 moveVector;
    private bool isSpeedCoroRunning;
    private bool hasPeak;


    private void Start()
    {
        rbCat = gameObject.GetComponent<Rigidbody2D>();
        moveVector = Vector3.right;
        InputManager.Instance.OnLeftMouseClick += HandleLeftMouseClick;
        InputManager.Instance.OnRightMouseClick += HandleRightMouseClick;
    }
    void Update()
    {
        LerpTo(audioAnalyzer.audioSpeed);
        MoveCat(moveVector, currentSpeed);
    }

    private void HandleLeftMouseClick()
    {
        Debug.Log("Left Clicked");
        //MoveCat(Vector3.right + Vector3.up, jumpForce);
        moveVector = Vector3.right + Vector3.up;
    }
    private void HandleRightMouseClick()
    {
        Debug.Log("Right Clicked");
        //MoveCat(Vector3.right + Vector3.down, fallForce);
        moveVector = Vector3.right + Vector3.down;
    }
    private void MoveCat(Vector3 direction, float speed)
    {
        rbCat.velocity = Vector3.zero;
        rbCat.AddForce(speed * Time.deltaTime * direction, ForceMode2D.Impulse);
    }

    private void LerpTo(AudioAnalyzer.AudioSpeed audioSpeed)
    {
        if (isSpeedCoroRunning)
            return;
        switch (audioSpeed)
        {
            case AudioAnalyzer.AudioSpeed.Stopping:
                {
                    hasPeak = false;
                    StartCoroutine(UpdateSpeed(currentSpeed, 100f));
                    break;
                }
            case AudioAnalyzer.AudioSpeed.Normal:
                {
                    if (hasPeak)
                        return;
                    StartCoroutine(UpdateSpeed(currentSpeed, 400f));
                    break;
                }
            case AudioAnalyzer.AudioSpeed.Fast:
                {
                    hasPeak = true;
                    StartCoroutine(UpdateSpeed(currentSpeed, 800f));
                    break;
                }
            default:
                break;
        }
    }

    private IEnumerator UpdateSpeed(float oldSpeed, float newSpeed)
    {
        isSpeedCoroRunning = true;
        float speed = oldSpeed;
        float timeToLerp = 0.4f;
        float timer = 0;
        while (speed != newSpeed) 
        {
            speed = Mathf.Lerp(oldSpeed, newSpeed, timer / timeToLerp);
            timer += Time.deltaTime;
            currentSpeed = speed;
            yield return null;
        }
        isSpeedCoroRunning = false;
    }
}
