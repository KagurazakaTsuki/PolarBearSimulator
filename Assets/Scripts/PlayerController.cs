using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    protected Rigidbody m_Rigidbody;
    protected PlayerInput m_PlayerInput;
    protected Animator m_Animator;
    protected Camera m_Camera;


    public float m_Speed = 4f;
    public float m_JumpSpeed = 0.4f;

    private Vector3? startingCameraForward = null;
    private Vector3? startingCameraRight = null;


    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_PlayerInput = GetComponent<PlayerInput>();
        m_Animator = GetComponent<Animator>();
        m_Camera = Camera.main;
    }

    private Vector2 m_Look;
    private Vector2 m_Move;
    private float m_Fire;
    private float m_Jump;

    protected bool isGrounded = true;

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            m_Animator.SetFloat("Forward", 0f);
        }
    }

    private void Update()
    {
        m_Move = m_PlayerInput.actions["Move"].ReadValue<Vector2>();
        m_Look = m_PlayerInput.actions["Look"].ReadValue<Vector2>();
        m_Fire = m_PlayerInput.actions["Fire"].ReadValue<float>();
        m_Jump = m_PlayerInput.actions["Jump"].ReadValue<float>();

        var cameraForward = m_Camera.transform.forward;
        // var cameraRight = m_Camera.transform.right;
        cameraForward.y = 0;
        cameraForward.Normalize();


        if (isGrounded)
        {
            var movement = m_Move.y * cameraForward
                // + m_Move.x * cameraRight // side way movement
                ;
            movement.Normalize();

            if (Mathf.Approximately(m_Move.y, 1f)) // Only rotating when Forward key down
            {
                m_Rigidbody.transform.forward = Vector3.Lerp(transform.forward, cameraForward, 0.03f);
            }

            m_Animator.SetFloat("Forward", movement.magnitude);

            if (Mathf.Approximately(m_Jump, 1f)) // Jump key down
            {
                m_Rigidbody.velocity += Vector3.up * m_JumpSpeed;
                m_Animator.SetTrigger("Jump");
            }

            var startPosition = transform.position;
            var endPosition = startPosition + movement * (Time.deltaTime * m_Speed);

            m_Rigidbody.MovePosition(endPosition);
        }
    }
}