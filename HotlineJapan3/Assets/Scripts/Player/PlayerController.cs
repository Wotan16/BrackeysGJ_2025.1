using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance {  get; private set; }

    public bool ControlledByPlayer = true;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb2D;
    private Vector2 mousePosition;
    private Vector2 inputDireciton;

    [Header("Dash")]
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldown;
    private float dashDistanceDelta;
    private float dashCDDelta;
    private bool canDash => dashCDDelta <= 0;
    private bool dashing => dashDistanceDelta < dashDistance;
    private Vector2 dashDirection;

    [Header("Attack")]
    [SerializeField] private float attackCooldown;
    private float attackCDDelta;
    private bool canAttack => attackCDDelta <= 0;
    private bool attacking;

    [Header("Parry")]
    [SerializeField] private float parryDuration;
    private float parryCDDelta;
    private bool canParry => parryCDDelta <= 0;
    private bool parrying;

    [SerializeField] private PlayerAnimator animator;
    
    private bool isBusy => attacking || dashing;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        animator.OnAttackAnimationEnded += Animator_OnAttackAnimationEnded;
    }

    private void Animator_OnAttackAnimationEnded()
    {
        if (!attacking)
            return;

        attacking = false;
    }

    private void Update()
    {
        HandleCooldowns();
        if (dashing)
            return;
        HandleMovement();
        HandleRotation();
    }

    private void FixedUpdate()
    {
        if (dashing)
        {
            HandleDashing();
        }
    }

    private void HandleCooldowns()
    {
        if(parryCDDelta > 0)
        {
            parryCDDelta -= Time.deltaTime;
        }

        if (dashCDDelta > 0)
        {
            dashCDDelta -= Time.deltaTime;
        }

        if (attackCDDelta > 0)
        {
            attackCDDelta -= Time.deltaTime;
        }
    }

    private void HandleDashing()
    {
        float moveAmount = Time.fixedDeltaTime * dashSpeed;
        Vector2 moveVector = dashDirection * moveAmount;
        rb2D.MovePosition(rb2D.position + moveVector);
        dashDistanceDelta += moveAmount;
    }

    private void HandleMovement()
    {
        rb2D.linearVelocity = inputDireciton.normalized * moveSpeed;
    }

    private void HandleRotation()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = (mouseWorldPosition - transform.position).normalized;
        transform.up = direction;
    }

    private void Attack()
    {
        if (isBusy || !canAttack)
            return;

        parrying = false;

        //animator.Attack();
    }

    private void Dash()
    {
        if (isBusy || !canDash || inputDireciton == Vector2.zero)
            return;

        dashDistanceDelta = 0;
        dashCDDelta = dashCooldown;
        parrying = false;
        dashDirection = inputDireciton.normalized;
        //animator.SetDashing(dashing);
    }

    private void Parry()
    {
        if(isBusy)
            return;

        parrying = true;
        //animator.SetParrying(parrying);
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

        Attack();
    }

    public void OnParry_Input(InputAction.CallbackContext context)
    {
        if (!ControlledByPlayer)
            return;

        if (!context.started)
            return;

        Parry();
    }

    public void OnDash_Input(InputAction.CallbackContext context)
    {
        if (!ControlledByPlayer)
            return;

        if (!context.started)
            return;

        Dash();
    }

    #endregion
}
