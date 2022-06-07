using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Parts of the script contain codes from Unity's 3D Game Kit
// https://assetstore.unity.com/packages/templates/tutorials/3d-game-kit-115747

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    protected Rigidbody m_Rigidbody;
    protected PlayerInput m_PlayerInput;
    protected Animator m_Animator;

    public float m_Speed = 2f;
    public float m_JumpSpeed = 0.8f;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_PlayerInput = GetComponent<PlayerInput>();
        m_Animator = GetComponent<Animator>();
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

        if (isGrounded)
        {
            var movement = new Vector3(m_Move.x, 0f, m_Move.y);
            m_Animator.SetFloat("Forward", movement.magnitude);

            if (Mathf.Approximately(m_Jump, 1f)) // Jump key down
            {
                m_Rigidbody.velocity += Vector3.up * m_JumpSpeed;
                m_Animator.SetTrigger("Jump");
            }

            m_Rigidbody.MovePosition(transform.position + movement * (Time.deltaTime * m_Speed));
        }
    }
}