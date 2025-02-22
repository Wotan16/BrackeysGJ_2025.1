using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public event Action OnPlayerDead;

    public static PlayerController Instance {  get; private set; }

    public bool ControlledByPlayer = true;
    public bool Invulnerable = false;
    public bool IsDead = false;

    [Header("Movement")]
    [SerializeField] private float defautMoveSpeed;
    [SerializeField] private float parryingMoveSpeed;
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
    [SerializeField] private float hitboxDuration;
    private float hitboxDurationDelta;
    private bool hitboxActive => hitboxDurationDelta <= 0;
    [SerializeField] private SwordHitbox swordHitbox;

    [Header("Parry")]
    [SerializeField] private float parryDuration;
    private bool parrying;
    [SerializeField] private float parryCooldown;
    private float parryCDDelta;
    private bool canParry => parryCDDelta <= 0;

    [SerializeField] private PlayerAnimator animator;
    
    private bool isBusy => attacking || dashing;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        InitializeSingleton();
    }

    private void InitializeSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one " + GetType().Name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        animator.OnAttackAnimationEnded += Animator_OnAttackAnimationEnded;

        PauseManager.Instance.OnGamePaused += PauseManager_OnGamePaused;
        PauseManager.Instance.OnGameResumed += PauseManager_OnGameResumed;
    }

    private void PauseManager_OnGameResumed(object sender, EventArgs e)
    {
        ControlledByPlayer = true;
    }

    private void PauseManager_OnGamePaused(object sender, EventArgs e)
    {
        ControlledByPlayer = false;
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
        animator.SetMoving(inputDireciton != Vector2.zero);
        if (dashing)
            return;
        HandleMovement();
    }

    private void FixedUpdate()
    {
        if (dashing)
        {
            HandleDashing();
        }
        HandleRotation();
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

        if (hitboxDurationDelta > 0)
        {
            hitboxDurationDelta -= Time.deltaTime;
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
        float moveSpeed = parrying ? parryingMoveSpeed : defautMoveSpeed;
        rb2D.linearVelocity = inputDireciton.normalized * moveSpeed;
    }

    private void HandleRotation()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = (mouseWorldPosition - rb2D.position).normalized;
        float rotation = Vector2.Angle(Vector2.up, direction);
        rotation = mouseWorldPosition.x < rb2D.position.x ? rotation : -rotation;
        rb2D.MoveRotation(rotation);
    }

    private void Attack()
    {
        if (isBusy || !canAttack)
            return;

        InterruptParry();
        attackCDDelta = attackCooldown;
        attacking = true;
        animator.Attack();
        AudioManager.PlaySound(SoundType.SwordSwing);
        StartCoroutine(EnableAttackHitbox());
    }

    private IEnumerator EnableAttackHitbox()
    {
        swordHitbox.EnableHitbox();
        yield return new WaitForSeconds(hitboxDuration);
        swordHitbox.DisableHitbox();
        attacking = false;
    }

    private void Dash()
    {
        if (isBusy || !canDash || inputDireciton == Vector2.zero)
            return;

        dashDistanceDelta = 0;
        dashCDDelta = dashCooldown;
        InterruptParry();
        dashDirection = inputDireciton.normalized;
        animator.SetDashing(dashing);
    }

    private void Parry()
    {
        if(isBusy || !canParry)
            return;

        parrying = true;
        parryCDDelta = parryCooldown;
        animator.SetParrying(parrying);
        StartCoroutine(InterruptParryAfterTime());
    }

    private IEnumerator InterruptParryAfterTime()
    {
        yield return new WaitForSeconds(parryDuration);
        InterruptParry();
    }

    private void InterruptParry()
    {
        parrying = false;
        animator.SetParrying(parrying);
    }

    //Vector2 in Action is the direction where player is looking
    public bool TakeDamage(Action<Vector2> OnParried)
    {
        if(IsDead)
            return true;

        if (parrying)
        {
            OnParried?.Invoke(transform.up);
            Debug.Log("Parried");
            return false;
        }

        if (Invulnerable)
            return true;

        IsDead = true;
        animator.Die();
        OnPlayerDead?.Invoke();
        AudioManager.PlaySound(SoundType.PlayerDeath);
        ControlledByPlayer = false;
        return true;
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
