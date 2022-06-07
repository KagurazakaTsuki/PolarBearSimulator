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
    protected static PlayerController s_Instance;

    public static PlayerController instance => s_Instance;


    protected CharacterController m_CharCtrl; // Reference used to actually move the character.
    protected Rigidbody m_Rigidbody;
    protected PlayerInput m_PlayerInput;
    protected Animator m_Animator;

    public float m_Speed = 8f;
    public float m_JumpSpeed = 10f;


    void Awake()
    {
        m_CharCtrl = GetComponent<CharacterController>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_PlayerInput = GetComponent<PlayerInput>();
        m_Animator = GetComponent<Animator>();

        s_Instance = this;
    }

    private Vector2 m_Look;
    private Vector2 m_Move;
    private float m_Fire;
    private float m_Jump;

    private void Update()
    {
        m_Move = m_PlayerInput.actions["Move"].ReadValue<Vector2>();
        m_Look = m_PlayerInput.actions["Look"].ReadValue<Vector2>();
        m_Fire = m_PlayerInput.actions["Fire"].ReadValue<float>();
        m_Jump = m_PlayerInput.actions["Jump"].ReadValue<float>();

        var movement = new Vector3(m_Move.x, 0f, m_Move.y);
        if (Mathf.Approximately(m_Jump, 1f) // Jump key down
            && Mathf.Approximately(m_Rigidbody.velocity.y, 0f) // grounded, not perfect
           )
        {
            m_Rigidbody.velocity = Vector3.up * m_JumpSpeed;
            m_Animator.Play("Jump");
        }

        else if (!Mathf.Approximately(m_Move.magnitude,0f)) // moving
        {
            m_Rigidbody.MovePosition(transform.position + movement * (Time.deltaTime * m_Speed));
            m_Animator.Play("Walk Cycle");
        }
        else
        {
            m_Animator.Play("Rest");
        }
    }

    // 'Fire' input action has been triggered. For 'Fire' we want continuous
    // action (i.e. firing) while the fire button is held such that the action
    // gets triggered repeatedly while the button is down. We can easily set this
    // up by having a "Press" interaction on the button and setting it to repeat
    // at fixed intervals.
    public void OnFire()
    {
        Debug.Log("fire");
    }

    // 'Look' input action has been triggered.
    public void OnLook(InputValue value)
    {
        m_Look = value.Get<Vector2>();
        Debug.Log($"look {m_Move}");
    }
}