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
    public float m_SprintMultiplier = 2f;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_PlayerInput = GetComponent<PlayerInput>();
        m_Animator = GetComponent<Animator>();
        m_Camera = Camera.main;
    }

    private Vector2 m_Move;
    private bool m_Sprint;
    private Vector2 m_Look;
    private bool m_Fire;
    private bool m_Jump;
    private bool m_Pause;

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
        m_Sprint = Mathf.Approximately(m_PlayerInput.actions["Sprint"].ReadValue<float>(), 1f);
        m_Look = m_PlayerInput.actions["Look"].ReadValue<Vector2>();
        m_Fire = Mathf.Approximately(m_PlayerInput.actions["Fire"].ReadValue<float>(), 1f);
        m_Jump = Mathf.Approximately(m_PlayerInput.actions["Jump"].ReadValue<float>(), 1f);
        m_Pause = Mathf.Approximately(m_PlayerInput.actions["Pause"].ReadValue<float>(), 1f);
        
        if(m_Fire)
            Cursor.lockState = CursorLockMode.Locked;
        
        if(m_Pause)
            Cursor.lockState = CursorLockMode.None;

        var cameraForward = m_Camera.transform.forward;
        // var cameraRight = m_Camera.transform.right;
        cameraForward.y = 0;
        cameraForward.Normalize();


        if (isGrounded)
        {
            if (Mathf.Approximately(m_Move.y, 1f)) // Only rotate when Forward key is down
            {
                m_Rigidbody.transform.forward = Vector3.Lerp(transform.forward, cameraForward, 0.1f);
            }
            
            // Moving in the direction where the polar bear facing front
            var movement = m_Move.y * m_Rigidbody.transform.forward;
            movement.Normalize();

            if (m_Sprint)
            {
                movement *= m_SprintMultiplier;
            }

            m_Animator.SetFloat("Forward", movement.magnitude);
            
            
            if (m_Jump) // Jump key down
            {
                m_Rigidbody.velocity += Vector3.up * m_JumpSpeed;
                m_Animator.SetTrigger("Jump");
            }

            m_Rigidbody.MovePosition(transform.position + movement * (Time.deltaTime * m_Speed));
        }
    }
}