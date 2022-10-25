using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using TMPro;

public class PinchDetection : MonoBehaviour
{
    public static PinchDetection Instance;

    public enum PinchDirection { NONE, IN, OUT}

    private InputManager inputManager;

    [SerializeField] private PinchDirection currentDirection;
    public PinchDirection CurrentDirection { get { return currentDirection; } }

    Finger firstTouch;
    Finger secondTouch;

    private List<Finger> otherTouches = new List<Finger>();
    public IReadOnlyCollection<Finger> OtherTouches { get { return otherTouches.AsReadOnly(); } }

    bool enablePinch;
    float inputLeeway = 10;

    float prevDist = 0;
    float dist = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            firstTouch = null;
            secondTouch = null;
            enablePinch = true;
            otherTouches = new List<Finger>();
        }
        else
        {
            Destroy(this);
        }
        inputManager = InputManager.Instance;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        // Subscribe to the OnStartTouchEvent
        inputManager.OnStartTouch += StartTouch;
        inputManager.OnEndTouch += EndTouch;
        inputManager.OnTouchMoved += TouchMoved;
    }

    private void OnDisable()
    {
        // Subscribe to the OnStartTouchEvent
        inputManager.OnStartTouch += StartTouch;
        inputManager.OnEndTouch += EndTouch;
        inputManager.OnTouchMoved += TouchMoved;
    }

    private void StartTouch(Finger finger, float time)
    {
        if (!enablePinch) return;

        if(firstTouch == null)
        {
            firstTouch = finger;
        }
        else if(secondTouch == null)
        {
            secondTouch = finger;
        }
        else
        {
            otherTouches.Add(finger);
        }   
    }

    private void TouchMoved(Finger finger, float time)
    {
        if (!enablePinch) return;
        if (secondTouch == null) return; 
        dist = Vector2.Distance(firstTouch.screenPosition, secondTouch.screenPosition);
        if (Mathf.Abs(dist - prevDist) > inputLeeway)
        {
            if (dist > prevDist)
            {
                // zoom out
                currentDirection = PinchDirection.OUT;
            }
            else if (dist < prevDist)
            {
                // zoom in
                currentDirection = PinchDirection.IN;
            }
            else
            {
                currentDirection = PinchDirection.NONE;
            }
        }
        else
        {
            currentDirection = PinchDirection.NONE;
        }

        prevDist = dist;
    }

    private void EndTouch(Finger finger, float time)
    {
        if (finger == firstTouch)
        {
            firstTouch = null;
            if (otherTouches.Count > 0)
            {
                var nextTouch = otherTouches[0];
                if (nextTouch != null)
                {
                    firstTouch = nextTouch;
                }
                otherTouches.RemoveAt(0);
            }

        }
        else if (finger == secondTouch)
        {
            secondTouch = null;
            if (otherTouches.Count > 0)
            {
                var nextTouch = otherTouches[0];
                if (nextTouch != null)
                {
                    secondTouch = nextTouch;
                }
                otherTouches.RemoveAt(0);
            }
        }
        else
        {
            otherTouches.Remove(finger);
        }
    }
}
