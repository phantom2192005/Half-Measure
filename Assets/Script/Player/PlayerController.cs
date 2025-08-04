using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using Unity.Mathematics;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    public State currentState;
    public Rigidbody rb;

    [Header("Movement Settings")]
    public Vector2 MoveInput;
    public Vector2 lastMoveInput;
    public float crouchDeceleration = 5.0f;
    public float turnSmoothTime = 0.5f;
    public float turnSmoothVelocity = 2.0f;

    public float airMutiplier;
    public bool isRunningHeld = false;
    public float moveSpeed = 15;
    public float moveAcceleration = 10.0f; // Force applied for movement
    public float runAcceleration = 5f;

    public float walkSpeed;
    public float runSpeed;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    [SerializeField]
    LayerMask slopeLayerMask;

    [Header("Jump Setting")]
    public float _jumpForce = 5.0f;
    public float jumpTime = 2.0f;

    public int jumpMaxCount = 2;
    public int jumpCounter = 0;
    public float jumpTimer;

    [Header("Dash Setting")]

    public float _dashForce = 10.0f;
    public float dashTimer;
    public float dashTime = 0.2f;

    public int dashMaxCount = 2;
    public int dashCounter = 0;

    [Header("Crouch Settings")]
    public float crouchSpeed;
    public float StartScaleY;
    public float CrouchScaleY;
    public CapsuleCollider HurtBox;
    public Transform body;

    [Header("GroundPound Settings")]

    public float groundPoundForce = 50.0f;

    public float delayGPTime = 0.1f;
    public float delayGPTimer;
    public float delayOutGPTimer;

    [Header("Check Box")]

    public float playerHeight = 2.0f;
    public bool onGround = false;
    public bool onSlope = false;
    public bool isGravityEffect = true;
    public float gravityMultiplier = 2.0f;
    public float groundDrag;
    public bool overHeadChecker = false;
    public bool shouldFaceMoveDirection = false;

    private InputAction walkAction;
    public InputAction jumpAction;
    public InputAction runAction;

    public InputAction crouchAction;

    private PlayerInput playerInput;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void ChangeState(State nextState)
    {
        if (currentState != null)
        {
            currentState.Exit(this);
        }
        currentState = nextState;
        currentState.Enter(this);
    }

    private void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        Vector3 center = HurtBox.center;
        float height = HurtBox.height;
        currentState = new IdleState();
        currentState.Enter(this);

        PrepareInputActions();
        RegisterInputActions();

        StartScaleY = transform.localScale.y;
    }
    void PrepareInputActions()
    {
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component missing!");
            return;
        }

        walkAction = playerInput.actions.FindAction("Walk");
        jumpAction = playerInput.actions.FindAction("Jump");
        runAction = playerInput.actions.FindAction("Run");
        crouchAction = playerInput.actions.FindAction("Crouch");
    }

    void RegisterInputActions()
    {
        walkAction.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        walkAction.canceled += ctx => MoveInput = Vector2.zero;
        runAction.performed += ctx => isRunningHeld = true;
        runAction.canceled += ctx => isRunningHeld = false;
    }

    private void Update()
    {
        if (MoveInput != Vector2.zero)
        {
            lastMoveInput = MoveInput;
        }
        currentState.Update(this);
        SpeedControl();
        if (OnSlope())
        {
            onSlope = true;
        }
        else
        {
            onSlope = false;
        }
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate(this);
        GravityEffect();
        if (isRunningHeld) OnRun(); // Gọi liên tục khi giữ nút chạy
    }



    public Vector3 CaculateMoveDirection()
    {
        // Tính hướng di chuyển theo camera
        Vector3 inputDir = new Vector3(lastMoveInput.x, 0f, lastMoveInput.y).normalized;
        float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

        // Xoay nhân vật mượt theo góc
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        // Tính vector hướng thực sự để áp lực lên player
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        return moveDir;
    }

    public void MoveCharacter()
    {
        Vector3 moveDirection = CaculateMoveDirection();

        if (MoveInput != Vector2.zero)
        {
            //on Slope 
            if (OnSlope())
            {
                rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
                if (rb.velocity.y > 0)
                {
                    rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Force);
                    //Debug.Log($"[Slope] Full velocity: {rb.velocity.magnitude}, Horizontal only: {new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude}");
                }
            }
            // Thêm lực theo hướng di chuyển
            if (onGround && !OnSlope())
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * moveAcceleration, ForceMode.Force);
            }
            else if (!onGround && !OnSlope())
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * moveAcceleration * airMutiplier, ForceMode.Force);
            }

        }
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f, slopeLayerMask))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(CaculateMoveDirection(), slopeHit.normal).normalized;
    }

    public void SpeedControl()
    {
        if (currentState is DashState) return;

        // phân biệt việc giới hạn tốc độ đi bộ hay chạy
        if (currentState is RunState)
        {
            moveSpeed = runSpeed;
        }
        else if (currentState is CrouchState)
        {
            moveSpeed = crouchSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }

        if (OnSlope())
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            
            // Lấy vận tốc ngang (không tính trục Y)
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);


            // Nếu vận tốc ngang vượt quá giới hạn, giới hạn lại
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
                //Debug.Log($"[Ground] Full velocity: {rb.velocity.magnitude}, Horizontal only: {flatVel.magnitude}");
            }
        }

    }


    public void GravityEffect()
    {
        if (isGravityEffect)
        {
            Vector3 force = rb.mass * Physics.gravity * gravityMultiplier;
            rb.AddForce(force, ForceMode.Acceleration);
        }
    }

    void OnJump()
    {
        if (overHeadChecker == true) { return; }
        jumpCounter++;
        jumpTimer = jumpTime;
        if (jumpCounter <= jumpMaxCount)
        {
            ChangeState(new JumpState());
        }
    }
    void OnDash()
    {
        if (overHeadChecker == true || currentState is CrouchState) { return; }
        dashCounter++;
        if (dashCounter <= dashMaxCount)
        {
            ChangeState(new DashState());
        }
    }
    void OnCrouch()
    {
        if (currentState is DashState)
            return;

        // Trên không
        if (!onGround && !OnSlope() && currentState is not GroundPoundState ) 
        {
            if (delayGPTimer == 0 && delayOutGPTimer == 0)
            {
                ChangeState(new GroundPoundState());
                return;
            }
        }
        if (currentState is not CrouchState)
        {
            ChangeState(new CrouchState());
        }
    }



    void OnRun()
    {
        if (currentState is RunState) { return; }

        if (currentState is CrouchState && onGround && !overHeadChecker)
        {
            ChangeState(new RunState());
        }
        if (currentState is WalkState && onGround)
        {
            ChangeState(new RunState());
        }
    }


    [System.Serializable]
    public struct CapsuleSetting
    {
        public Vector3 center;
        public float height;
        public float radius;

        public CapsuleSetting(Vector3 center, float height, float radius)
        {
            this.center = center;
            this.height = height;
            this.radius = radius;
        }
    }

    Dictionary<string, CapsuleSetting> colliderSettings = new Dictionary<string, CapsuleSetting>()
{
    { "Idle",   new CapsuleSetting(new Vector3(0, 0, 0), 2f, 0.5f) },
    { "Crouch", new CapsuleSetting(new Vector3(0, -0.3f, 0), 1.2f, 0.5f) },
};
    public void ApplyHurtBoxSetting(string stateName)
    {
        if (colliderSettings.TryGetValue(stateName, out CapsuleSetting setting))
        {
            HurtBox.height = setting.height;
            HurtBox.center = setting.center;
        }
    }
    void DebugDrag()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 20;
        style.normal.textColor = Color.yellow;

        string dragText = $"Drag: {rb.drag:F2}, AngularDrag: {rb.angularDrag:F2}";
        GUI.Label(new Rect(1, 830, 400, 30), dragText, style);

        float speed = rb.velocity.magnitude;
        float dragForce = rb.drag * speed;
        string dragEffectText = $"Estimated Drag Deceleration: {dragForce:F2} (based on velocity)";
        GUI.Label(new Rect(1, 860, 600, 30), dragEffectText, style);
    }
    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 20;
        style.normal.textColor = Color.white;

        Vector3 velocity = rb.velocity;
        string velocityText = $"Velocity: ({velocity.x:F2}, {velocity.y:F2}, {velocity.z:F2})";
        string velocityMagnitude = $"Velocity Magnitude: {new Vector3 (rb.velocity.x, 0, rb.velocity.z).magnitude}";
        string moveDirection = $"moveDirection: {CaculateMoveDirection()}";

        GUI.Label(new Rect(1, 750, 400, 30), velocityText, style);
        GUI.Label(new Rect(1, 700, 400, 30), velocityMagnitude, style);
        GUI.Label(new Rect(1, 650, 400, 30), moveDirection, style);
        DebugDrag();
    }
}
