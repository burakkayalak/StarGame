using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    public bool debugMode;

    public static InputManager Instance;

    private StarInputSystem starInputSystem;

    private Camera cameraMain;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        starInputSystem = new StarInputSystem();
        cameraMain = Camera.main;
    }

    private void Start()
    {
        SubscribeToInputEvents();
    }

    // Primary touch events
    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;

    public delegate void TouchDeltaEvent(Vector2 position);
    public event TouchDeltaEvent OnTouchDelta;

    private void StartTouch(InputAction.CallbackContext _ctx)
    {
        if (OnStartTouch != null)
        {
            Vector2 worldPosition = cameraMain.ScreenToWorldPoint(starInputSystem.Mobile.Touch.ReadValue<Vector2>());

            OnStartTouch(worldPosition, (float)_ctx.startTime);
        }
    }

    private void EndTouch(InputAction.CallbackContext _ctx)
    {
        if (OnEndTouch != null)
        {
            Vector2 worldPosition = cameraMain.ScreenToWorldPoint(starInputSystem.Mobile.Touch.ReadValue<Vector2>());

            OnEndTouch(worldPosition, (float)_ctx.time);
        }
    }

    private void GetDelta(InputAction.CallbackContext _ctx)
    {
        if (OnTouchDelta != null)
        {
            OnTouchDelta(_ctx.ReadValue<Vector2>());
        }
    }

    private void SubscribeToInputEvents()
    {
        SubscribeStarted(starInputSystem.Mobile.Touch, StartTouch);
        SubscribeCancelled(starInputSystem.Mobile.Touch, EndTouch);

        SubscribeAllPhases(starInputSystem.Mobile.TouchDelta, GetDelta);
    }

    public Vector2 PrimaryPosition()
    {
        return cameraMain.ScreenToWorldPoint(starInputSystem.Mobile.Touch.ReadValue<Vector2>());
    }

    private void SubscribeStarted(InputAction action, Action<InputAction.CallbackContext> function)
    {
        action.started += function;
        //action.performed += function;
        //action.canceled += function;
    }

    private void SubscribeCancelled(InputAction action, Action<InputAction.CallbackContext> function)
    {
        //action.started += function;
        //action.performed += function;
        action.canceled += function;
    }

    private void SubscribeAllPhases(InputAction action, Action<InputAction.CallbackContext> function)
    {
        action.started += function;
        action.performed += function;
        action.canceled += function;
    }

    private void OnEnable()
    {
        starInputSystem.Enable();
    }
    private void OnDisable()
    {
        starInputSystem.Disable();
    }

    
    private void Log(string _msg)
    {
        if (!debugMode)
            return;

        Debug.Log("[" + this.name + "] > [" + this.GetType().Name + "]: " + _msg);
    }

    private void LogWarning(string _msg)
    {
        if (!debugMode)
            return;

        Debug.LogWarning("[" + this.name + "] > [" + this.GetType().Name + "]: " + _msg);
    }

    private void LogError(string _msg)
    {
        if (!debugMode)
            return;

        Debug.LogError("[" + this.name + "] > [" + this.GetType().Name + "]: " + _msg);
    }
}
