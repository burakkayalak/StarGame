using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

public class SwipeDetection : MonoBehaviour
{
    public bool debugMode;

    [SerializeField]
    private float minimumDistance = 1f;
    [SerializeField]
    private float maximumTime = 1.0f;
    [SerializeField, Range(0f, 1f)]
    private float directionThreshold = 0.9f;


    private InputManager inputManager;

    private Vector2 startPosition;
    private float startTime;

    private Vector2 endPosition;
    private float endTime;

    private void Awake()
    {
        
        inputManager = InputManager.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void SwipeStart(Vector2 position, float time)
    {
        Log("Swipe started.. position: " + position);
        startPosition = position;
        startTime = time;
        
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        Log("Swipe end.. position: " + position);

        endPosition = position;
        endTime = time;

        DetectSwipe();
    }
    

    private void DetectSwipe()
    {
        if (Vector2.Distance(startPosition, endPosition) >= minimumDistance && (endTime - startTime) <= maximumTime)
        {
            Log("Swipe detected.");
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;

            SwipeDirection(direction2D);
            Log("Direction: " + direction2D);
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        if(Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            Log("Swipe UP");
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            Log("Swipe DOWN");
        }
        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            Log("Swipe LEFT");
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            Log("Swipe RIGHT");
        }
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
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
