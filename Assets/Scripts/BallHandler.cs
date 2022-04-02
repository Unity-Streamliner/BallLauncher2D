using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float detachDelay = 0.1f;
    [SerializeField] private float respawnDelay;
    
    private Rigidbody2D _currentBallRigidbody;
    private SpringJoint2D _currentBallSpringJoint;
    private Camera _camera;
    private bool _isDragging;
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        SpawnNewBall();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    // Update is called once per frame
    void Update()
    {

        if (_currentBallRigidbody == null)
        {
            return;
        }
        if (Touch.activeTouches.Count == 0)
        {
            if (_isDragging)
            {
                LaunchBall();
            }

            _isDragging = false;
            return;
        }

        _isDragging = true;
        _currentBallRigidbody.isKinematic = true;
        Vector2 touchPosition = new Vector2();
        foreach (Touch touch in Touch.activeTouches)
        {
            touchPosition += touch.screenPosition;
        }

        touchPosition /= Touch.activeTouches.Count;
        Vector3 worldPoint = _camera.ScreenToWorldPoint(touchPosition);
        _currentBallRigidbody.position = worldPoint;
    }

    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        _currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        _currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        _currentBallSpringJoint.connectedBody = pivot;
    }

    private void LaunchBall()
    {
        _currentBallRigidbody.isKinematic = false;
        _currentBallRigidbody = null;

        Invoke(nameof(DetachBall), detachDelay);
        
    }

    private void DetachBall()
    {
        _currentBallSpringJoint.enabled = false;
        _currentBallSpringJoint = null;
        
        Invoke(nameof(SpawnNewBall), respawnDelay);
    }
}
