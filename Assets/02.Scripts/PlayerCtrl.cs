using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    PlayerInputActions inputAction;
    Rigidbody2D rb;
    Vector2 vec;

    private int moveSpeed = 3;
    private int jumpForce = 5;

    private bool isJumped = false;

    void Awake()
    {
        inputAction = new PlayerInputActions();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        inputAction.Player.Enable();
        inputAction.Player.Move.performed += OnMove;
        inputAction.Player.Move.canceled += OnMove;

        inputAction.Player.Jump.performed += OnJump;
        inputAction.Player.Jump.canceled += OnJump;
    }

    private void OnDisable()
    {
        inputAction.Player.Jump.canceled -= OnJump;
        inputAction.Player.Jump.performed -= OnJump;

        inputAction.Player.Move.canceled -= OnMove;
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Disable();
    }

    void OnMove(InputAction.CallbackContext context)
    {
        vec = context.ReadValue<Vector2>();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed && !isJumped)
        {
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
            isJumped = true;
        }
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * vec);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("GROUND"))
        {
            isJumped = false;
        }
    }
}
