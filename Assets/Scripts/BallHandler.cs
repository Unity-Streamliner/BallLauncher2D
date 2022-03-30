using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D currentBallRigidbody;

    private Camera _camera;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            currentBallRigidbody.isKinematic = false;
            return;
        }
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPoint = _camera.ScreenToWorldPoint(touchPosition);
        currentBallRigidbody.position = worldPoint;
        currentBallRigidbody.isKinematic = true;
        
    }
}
