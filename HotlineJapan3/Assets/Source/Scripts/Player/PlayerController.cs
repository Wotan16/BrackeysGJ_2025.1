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
    private float moveSpeed;

    [Header("Running")]
    [SerializeField] private float runningSpeed;
    private bool runButtonPressed;
    private bool running;

    [Header("Attack")]
    [SerializeField] private float attackCooldown;
    private float attackCDDelta;
    private bool canAttack => attackCDDelta <= 0;
    private bool attacking;
    [SerializeField] private float hitboxDuration;
    private float hitboxDurationDelta;
    private bool hitboxActive => hitboxDurationDelta > 0;
    [SerializeField] private SwordHitbox swordHitbox;

    [Header("Parry")]
    [SerializeField] private float parryDuration;
    private bool parrying;
    [SerializeField] private float parryCooldown;
    private float parryCDDelta;
    private bool canParry => parryCDDelta <= 0;

    private bool isBusy => attacking;

    [Header("Other References")]
    [SerializeField] private PlayerAnimator animator;
    [SerializeField] private PlayerCameraTarget cameraTargetController;
    [SerializeField] private Collider2D projectileCollider;
    public Transform cameraTarget => cameraTargetController.CameraTarget;

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
        HandleRunning();
        HandleSpeedChange();
        animator.SetMoving(inputDireciton != Vector2.zero);
        HandleMovement();
    }

    private void HandleRunning()
    {
        running = !isBusy && inputDireciton != Vector2.zero && runButtonPressed;
        animator.SetRunning(running);
        projectileCollider.enabled = !running;
    }

    private void HandleSpeedChange()
    {
        if (running)
        {
            moveSpeed = runningSpeed;
            return;
        }
        moveSpeed = parrying ? parryingMoveSpeed : defautMoveSpeed;
    }

    private void FixedUpdate()
    {
        HandleRotation();
    }

    private void HandleCooldowns()
    {
        if(parryCDDelta > 0)
        {
            parryCDDelta -= Time.deltaTime;
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

    private void HandleMovement()
    {
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
        InterruptRun();
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

    private void Parry()
    {
        if(isBusy || !canParry)
            return;

        InterruptRun();
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

    //Vector2 in Action<Vector2> is the direction where player is looking
    public bool TakeDamage(AttackHitInfo hitInfo, Action<Vector2> OnParried)
    {
        if(IsDead)
            return false;

        if(hitInfo.type == AttackHitInfo.AttackType.Projectile && hitboxActive)
            return false;

        if (parrying)
        {
            OnParried?.Invoke(transform.up);
            Vector3 parryDireciton = hitInfo.attackerTransform.position - transform.position;
            Quaternion vfxRotation = Quaternion.FromToRotation(Vector2.up, parryDireciton);
            float vfxOffset = 0.5f;
            VFXManager.CreateParryVFX(transform.position + parryDireciton.normalized * vfxOffset, vfxRotation);
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

    private void InterruptRun()
    {
        runButtonPressed = false;
        running = false;
        animator.SetRunning(running);
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
        {
            runButtonPressed = false;
            return;
        }

        if (context.started)
        {
            runButtonPressed = true;
        }

        if (context.canceled)
        {
            runButtonPressed = false;
        }
    }

    #endregion
}
