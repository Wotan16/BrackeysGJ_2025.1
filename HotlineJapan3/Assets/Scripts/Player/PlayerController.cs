using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance {  get; private set; }

    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb2D;
    private Vector2 mousePosition;
    private Vector2 inputDireciton;
    public bool ControlledByPlayer = true;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        rb2D.linearVelocity = inputDireciton * moveSpeed;
    }

    private void HandleRotation()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = (mouseWorldPosition - transform.position).normalized;
        transform.up = direction;
    }

    #region InputEvents

    public void OnMove_Input(InputAction.CallbackContext context)
    {
        if(ControlledByPlayer)
            inputDireciton = context.ReadValue<Vector2>();
        else
            inputDireciton = Vector2.zero;
    }

    public void OnLook_Input(InputAction.CallbackContext context)
    {
        if (!ControlledByPlayer)
            return;

        mousePosition = context.ReadValue<Vector2>();
    }

    public void OnAttack_Input(InputAction.CallbackContext context)
    {
        if (!ControlledByPlayer)
            return;

        if (!context.started)
            return;
    }

    public void OnParry_Input(InputAction.CallbackContext context)
    {
        if (!ControlledByPlayer)
            return;

        if (!context.started)
            return;
    }

    public void OnDash_Input(InputAction.CallbackContext context)
    {
        if (!ControlledByPlayer)
            return;

        if (!context.started)
            return;
    }

    #endregion
}
