using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _horizontal;
    private float _vertical;
    private Transform _camera;
    private Animator _animator;
    private CharacterController _controller;
    [SerializeField] private bool _isGrounded;

    private float _gravity = 9.81f;
    private Vector3 _playerGravity;

    private float turnSmoothTime;
    
    [SerializeField] private float _playerSpeed = 5;
    [SerializeField] private float _jumpHeight = 1;

    [SerializeField] private Transform _sensorPosition;
    [SerializeField] private float _sensorRadius = 0.2f;
    [SerializeField] private float turnSmoothVelocity;
    [SerializeField] private LayerMask groundSensor;

    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        Movement();
        Jump();
    }

    void Movement()
    {
        Vector3 direction = new Vector3 (_horizontal, 0, _vertical);
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;

        transform.rotation =Quaternion.Euler(0, targetAngle,0);
        Vector3 moveDirection =Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        _controller.Move (moveDirection.normalized * Time.deltaTime * _playerSpeed);

        if (direction != Vector3.zero)
        {
            gameObject.transform.forward = direction;
        }

        _animator.SetFloat("VelX", 0);
        _animator.SetFloat("VelZ", direction.magnitude);
    }

    void Jump()
    {
        _isGrounded = Physics.CheckSphere(_sensorPosition.position, _sensorRadius, groundSensor);    
        

        if(Input.GetButtonDown("Jump") && _isGrounded)
        {
            _playerGravity.y += Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        }
        
        _playerGravity.y += _gravity * Time.deltaTime;
        _controller.Move(_playerGravity * Time.deltaTime);
        
        _animator.SetBool("IsJumping", true);
    }
}
