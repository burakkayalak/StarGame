using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.Touch;

public class StarCharacter : MonoBehaviour
{
    [SerializeField] 
    private bool debugMode = true;
    
    [Header("Movement Parameters")]
    [SerializeField] 
    private float moveSpeedX = 8f;
    [SerializeField] 
    private float positionYMultiplier = 3f;
    [SerializeField] 
    private int currentPositionY = 0;
    [SerializeField] 
    private int minPositionY = -2;
    [SerializeField] 
    private int maxPositionY = +2;
    [SerializeField] 
    private float travelTimeAxisY = 0.4f;
    
    [Header("Swipe Detection Parameters")]
    [SerializeField]
    private float minimumDistance = 1f;
    [SerializeField]
    private float maximumTime = 1.0f;
    [SerializeField, Range(0f, 1f)]
    private float directionThreshold = 0.9f;
    
    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;
    
    [SerializeField] private bool bTravelling = false; 
    
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        MoveToRight();

        if (bTravelling) { return; }
        
        if (Touchscreen.current.press.wasPressedThisFrame)
        {
            startPosition = Touchscreen.current.position.ReadValue();
            Log("Swipe started.. position: " + startPosition);
            startTime = Time.time;
        }
        else if (Touchscreen.current.press.wasReleasedThisFrame)
        {
            endPosition = Touchscreen.current.position.ReadValue();
            Log("Swipe end.. position: " + endPosition);
            endTime = Time.time;

            DetectSwipe();
        }
    }

    private void MoveToRight()
    {
        transform.position += new Vector3(moveSpeedX, 0f, 0f) * Time.deltaTime;
    }

    void DetectSwipe()
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
            MoveToNewLocation(ESwipeDirection.UP);
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            Log("Swipe DOWN");
            MoveToNewLocation(ESwipeDirection.DOWN);
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

    private void MoveToNewLocation(ESwipeDirection swipeDirection)
    {
        int newPositionY;
        switch (swipeDirection)
        {
            case ESwipeDirection.UP:
                newPositionY = Mathf.Clamp(currentPositionY + 1, minPositionY, maxPositionY);
                if (newPositionY != currentPositionY)
                {
                    currentPositionY = newPositionY;
                    //StartCoroutine(GoToTarget(newPositionY, travelTimeAxisY));
                    GoToTarget(newPositionY, travelTimeAxisY);
                    bTravelling = true;
                }
                break;
            case ESwipeDirection.DOWN:
                newPositionY = Mathf.Clamp(currentPositionY - 1, minPositionY, maxPositionY);
                if (newPositionY != currentPositionY)
                {
                    currentPositionY = newPositionY;
                    //StartCoroutine(GoToTarget(newPositionY, travelTimeAxisY));
                    GoToTarget(newPositionY, travelTimeAxisY);
                    bTravelling = true;
                }
                break;
            case ESwipeDirection.LEFT:
                
                break;
            case ESwipeDirection.RIGHT:
                
                break;
        }
    }

    // IEnumerator GoToTarget(int positionY, float time)
    // {
    //     float newPositionY = positionYMultiplier * positionY;
    //     float addToY = (newPositionY - transform.position.y) / time;
    //
    //     while (true)
    //     {
    //         transform.position = new Vector3(transform.position.x, transform.position.y + addToY, transform.position.z) * Time.deltaTime;
    //         
    //         yield return new WaitForSeconds(time);
    //     }
    //     
    //     yield return null;
    // }

    private void GoToTarget(int positionY, float time)
    {
        bTravelling = true;
        float newPositionY = positionYMultiplier * positionY;
        transform.DOMoveY(newPositionY, time).OnComplete(() => { bTravelling = false; });

        // float newPositionY = positionYMultiplier * positionY;
        // float deltaY = newPositionY - transform.position.y;
        // transform.position += new Vector3(0f, deltaY, 0f) * Time.deltaTime / time;
        //
        // if (Mathf.Abs(newPositionY - transform.position.y) <= 0.1f)
        // {
        //     bTravelling = false;
        // }
    }
    

    private void OnEnable()
    {
        //EnhancedTouchSupport.Enable();
    }
    
    private void OnDisable()
    {
        //EnhancedTouchSupport.Disable();
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

enum ESwipeDirection
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}
