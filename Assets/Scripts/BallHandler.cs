using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    // Update is called once per frame
    void Update()
    {

        if (_currentBallRigidbody == null)
        {
            return;
        }
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (_isDragging)
            {
                LaunchBall();
            }

            _isDragging = false;
            return;
        }

        _isDragging = true;
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPoint = _camera.ScreenToWorldPoint(touchPosition);
        _currentBallRigidbody.position = worldPoint;
        _currentBallRigidbody.isKinematic = true;
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
