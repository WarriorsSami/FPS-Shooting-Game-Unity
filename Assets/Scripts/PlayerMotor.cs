using System;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))] public class PlayerMotor : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        
        private Vector3 _velocity = Vector3.zero;
        private Vector3 _rotation = Vector3.zero;
        private float _cameraRotationX = 0f;
        private float _currentCameraRotationX = 0f;
        private Vector3 _thrusterForce = Vector3.zero;

        [SerializeField] private float cameraRotationLimit = 85f;
        
        private Rigidbody _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        // gets a movement vector
        public void Move(Vector3 velocity)
        {
            _velocity = velocity;
        }
        
        // gets a rotational vector
        public void Rotate(Vector3 rotation)
        {
            _rotation = rotation;
        }
        
        // gets a rotational vector for the camera
        public void RotateCamera(float cameraRotationX)
        {
            _cameraRotationX = cameraRotationX;
        }
        
        // gets a thruster force based on user input
        public void Push(Vector3 thrusterForce)
        {
            _thrusterForce = thrusterForce;
        }

        // run every physics iteration
        private void FixedUpdate()
        {
            PerformMovement();
            PerformRotation();
        }     
    
        // perform movement based on velocity variable
        private void PerformMovement()
        {
            if (_velocity != Vector3.zero)
            {
                _rb.MovePosition(_rb.position + _velocity * Time.fixedDeltaTime);
            }
            
            if (_thrusterForce != Vector3.zero)
            {
                _rb.AddForce(_thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
        }
        
        // perform rotation based on rotation variable
        private void PerformRotation()
        {
            _rb.MoveRotation(_rb.rotation * Quaternion.Euler(_rotation));
            
            // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
            if (cam != null)
            {
                // set our rotation and clamp it
                _currentCameraRotationX -= _cameraRotationX;
                _currentCameraRotationX = Mathf.Clamp(
                    _currentCameraRotationX, 
                    -cameraRotationLimit,
                    cameraRotationLimit);

                // apply our rotation to transform camera
                cam.transform.localEulerAngles = new Vector3(_currentCameraRotationX, 0f, 0f);
            }
        }
    }
}
