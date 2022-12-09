using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // ToDelete

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    [SerializeField] private InputActionReference leftMouseClick;
    [SerializeField] private InputActionReference rightMouseClick;


    public Action OnLeftMouseClick;
    public Action OnRightMouseClick;


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
    private void Update()
    {
        //CheckMouseClick();
    }
    private void CheckMouseClick()
    {
        if(leftMouseClick.action.WasPressedThisFrame())
        {
            OnLeftMouseClick?.Invoke();
        }
        if(rightMouseClick.action.WasPressedThisFrame())
        {
            OnRightMouseClick?.Invoke();
        }
    }

    public void InvokeLeftButton()
    {
        OnLeftMouseClick?.Invoke();
    }    
    public void InvokeRightButton()
    {
        OnRightMouseClick?.Invoke();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
