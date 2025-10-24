using Unity.Cinemachine;
using UnityEngine;
using KinematicCharacterControler;

public class PlayerMovement : MovementEngine
{
    public static PlayerMovement instance;
    private PlayerInputActions.PlayerActions m_inputActions;

    [Header("Movement")]
    public GameObject speedLines;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float airSpeed = 10f;
    public float airAcelleration = 170;
    private float m_speed = 5F;
    public float sprintFOV = 70f;
    public float walkFOV = 60f;
    private float currFOV = 60f;
    public bool canSprint = true;
    public float rotationSpeed = 5f;
    public float maxWalkAngle = 60f;
    public GameObject player;
    public GameObject camPoint;
    public CinemachineCamera ciniCamera;
    public float zoomSpeed = 5;
    private Transform m_orientation;
    public Transform cam;

    public Vector3 spwanPos;


    [Header("Wall Ride Settings")]
    public float wallRideSpeed = 12f;
    public float wallRideGravity = -1f;
    public float wallCheckDistance = 1f;
    public float wallStickForce = 5f;
    public float maxWallRideTime = 2f;
    public bool canWallRide = true;
    public LayerMask wallLayer;

    [SerializeField, ReadOnly] private bool m_isWallRiding = false;
    private Vector3 m_wallNormal = Vector3.zero;
    private Vector3 m_wallRunDir = Vector3.zero;
    private bool m_wasWallRiding = false;
    private float wallRideTimer;

    [Header("Dashing")]
    public float dashForce = 0f;
    public float dashDuration = 0.2f;
    public float dashCoolDown = 2f;
    public bool canDash = true;
    public KeyCode dashKey = KeyCode.Tab;
    public float dashFOV = 80f;
    public AnimationCurve dashCurve;

    protected bool m_isDashing = false;
    private float dashTime = 0f;
    private float m_dashCooldownTimer = 0f;
    private Vector3 m_dashDirecton;
    private float m_currTime = 0f;



    [Header("Crouch")]
    public KeyCode crouchKey = KeyCode.LeftControl;
    public bool isCrouching;
    public float crouchSpeed = 5f;
    private bool m_requestedCrouch = false;
    public float crouchHeight = 1.5f;
    public bool canCrouch = true;


    [Header("Physics")]
    public Vector3 gravity = new Vector3(0, -9, 0);
    private float m_elapsedFalling;
    private Vector3 m_velocity;
    public bool lockCursor = true;
    private Vector2 mouseInput;
    private bool m_prevGrounded = false;

    [Header("Jump Settings")]
    public bool canJump = true;
    public float jumpForce = 5.0f;
    public float maxJumpAngle = 80f;
    public float jumpCooldown = 0.25f;
    public bool canDoubleJump = true;
    public int maxJumpCount = 1;
    public int jumpCount = 1;
    public float jumpInputElapsed = Mathf.Infinity;
    private float m_timeSinceLastJump = 0.0f;
    private bool m_jumpInputPressed = false;
    private float m_jumpBufferTime = 0.25f;
    private bool m_Jumping = false;


    [Header("Sliding")]
    public bool isSliding = false;
    public bool canSlide = true;
    public float startSlideSpeed = 8;
    public float endSlideSpeed = 4;
    private Vector3 m_slideDir;
    public float maxSlideAngle = 70;
    public float slideForce;
    private float m_slideSpeed;
    private bool m_slideInputPressd = false;


    public float slopeSlidingInfluence = 1f;
    public float slideMinSpeed = 4f;
    public float slideMaxSpeed = 20f;
    public float slideJumpHeight = 8f;

    public float slideFriction = 0.95f;

    public bool onLadder = false;
    public float ladderUpForce = 5f;
    public bool groundedLastFrame = false;

    // Knockback
    private Vector3 kbDir;
    private float kbStrength;
    private bool isTakingKB = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        m_inputActions = new PlayerInputActions().Player;
        m_inputActions.Enable();
    }
    void Start()
    {
        m_orientation = cam;
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
        }
        speedLines = GameObject.Find("SpeedLinesGo");
        spwanPos = transform.position;
    }

    void Update()
    {
        //HandleCursor();\
        CheckIfGrounded(out _);
        GetInput();
        HandleFOV();



        m_prevGrounded = groundedState.isGrounded;

    }

    void FixedUpdate()
    {
        CheckIfGrounded(out _);
        Move();
        if (groundedState.isGrounded)
        {
            jumpCount = maxJumpCount;
        }
    }

    void Move()
    {
        if (isTakingKB)
        {
            HandleKnockBack();
        }
        else if (WallRun()) { }

        else if (HandleSliding()) { }

        else if (m_isDashing)
            HandleDashing(Time.deltaTime);
        else
        {
            HandleRegularMovement();
        }

        transform.position = MovePlayer(m_velocity * Time.deltaTime);
        m_wasWallRiding = m_isWallRiding;
    }

    void GetInput()
    {

        mouseInput = new Vector2(m_inputActions.Move.ReadValue<Vector2>().x, m_inputActions.Move.ReadValue<Vector2>().y);
        m_jumpInputPressed = m_inputActions.Jump.WasPressedThisFrame();

        if (m_jumpInputPressed)
            jumpInputElapsed = 0.0f;
        else
            jumpInputElapsed += Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.P))
        {
            transform.position = spwanPos;
        }

        m_speed = m_inputActions.Sprint.IsPressed() ? runSpeed : walkSpeed;


        if (m_inputActions.Dash.WasPressedThisFrame() && m_dashCooldownTimer <= 0f && !m_isDashing && dashForce > 0f)
        {
            Vector3 inputDir = transform.TransformDirection(new Vector3(mouseInput.x, 0, mouseInput.y));
            if (inputDir.magnitude < 0.1f)
                inputDir = transform.forward; // default forward dash

            m_dashDirecton = inputDir.normalized;
            m_isDashing = true;
            dashTime = dashDuration;
            speedLines.GetComponent<SpeedLines>().speedLinesOn = true;

            m_dashCooldownTimer = dashCoolDown;

        }

        m_slideInputPressd = m_inputActions.Slide.WasPressedThisFrame();


    }

    void HandleRegularMovement()
    {
        if (ciniCamera.Lens.Dutch != 0)
        {
            ciniCamera.Lens.Dutch = Mathf.Lerp(ciniCamera.Lens.Dutch, 0f, Time.deltaTime * 8f);
        }

        Vector3 inputDir = transform.TransformDirection(new Vector3(mouseInput.x, 0, mouseInput.y));


        bool onGround = CheckIfGrounded(out RaycastHit groundHit) && m_velocity.y <= 0.0f;
        bool falling = !(onGround && maxWalkAngle >= Vector3.Angle(Vector3.up, groundHit.normal));

        // In air Movment
        if (falling)
        {

            if (inputDir.sqrMagnitude > 0f && !onLadder)
            {

                var movmentForce = inputDir * airAcelleration * Time.deltaTime;
                var planarVelocity = new Vector3(m_velocity.x, 0, m_velocity.z);
                var targetVelocity = planarVelocity + movmentForce;

                targetVelocity = Vector3.ClampMagnitude(targetVelocity, airSpeed);

                m_velocity.x = targetVelocity.x;
                m_velocity.z = targetVelocity.z;


            }

            m_velocity += gravity * Time.deltaTime;
            m_elapsedFalling += Time.deltaTime;

        }
        else if (onGround)
        {
            m_velocity = Vector3.zero;
            m_velocity += inputDir * m_speed;
            m_elapsedFalling = 0;
            jumpCount = maxJumpCount;
        }

        bool shouldJump = ((onGround && groundedState.angle <= maxJumpAngle) || (canDoubleJump && jumpCount > 0))
                            && canJump && m_timeSinceLastJump >= jumpCooldown;

        bool attemptingJump = jumpInputElapsed <= m_jumpBufferTime;


        if (shouldJump && attemptingJump && !onLadder)
        {
            jumpCount -= 1;
            m_velocity.y = jumpForce;
            m_timeSinceLastJump = 0.0f;
            jumpInputElapsed = Mathf.Infinity;
        }
        else if ( attemptingJump && onLadder)
        {
            m_velocity += ladderUpForce * Vector3.up;
            m_velocity.y = Mathf.Clamp(m_velocity.y, 5, 10);
        }
        else
            m_timeSinceLastJump += Time.deltaTime;

        transform.position = MovePlayer(m_velocity * Time.deltaTime);
        transform.rotation = new Quaternion(transform.rotation.x, cam.transform.rotation.y, transform.rotation.z, cam.rotation.w);
        //m_velocity = new Vector3(0, m_velocity.y, 0);

        if (m_dashCooldownTimer > 0)
        {
            m_dashCooldownTimer -= Time.deltaTime;
        }

        groundedLastFrame = groundedState.isGrounded;

        if (onGround && !attemptingJump)
            SnapPlayerDown();
    }

    public void KnockBack(Vector3 _dir, float _strenght)
    {
        kbDir = _dir.normalized;
        kbStrength = _strenght;
        isTakingKB = true;
    }

    void HandleKnockBack()
    {
        transform.position = MovePlayer(kbStrength * Time.deltaTime * kbDir);
        kbStrength *= 0.9f;

        if (kbStrength <= 0.5f)
        {
            isTakingKB = false;
        }

    }

    #region wall Running
    bool WallRun()
    {
        if (!m_isWallRiding && m_wasWallRiding) return false;

        if ((m_isWallRiding && m_inputActions.Jump.WasPressedThisFrame()) || wallRideTimer > 2f)
        {
            m_velocity = m_wallNormal * jumpForce + Vector3.up * jumpForce + transform.forward * jumpForce;
            transform.position = MovePlayer(m_velocity * Time.deltaTime);
            m_isWallRiding = false;
            jumpCount = maxJumpCount;
            wallRideTimer = 0f;
            return false;
        }

        Vector3 inputDir = new Vector3(mouseInput.x, 0, mouseInput.y);

        if (inputDir.z > 0)
        {
            if (!m_isWallRiding && !groundedState.isGrounded)
            {

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.right, out hit, wallCheckDistance, wallLayer) ||
                     Physics.Raycast(transform.position, -transform.right, out hit, wallCheckDistance, wallLayer))
                {
                    m_isWallRiding = true;
                    m_wallNormal = hit.normal;
                    wallRideTimer = 0f;
                }
            }
        }

        if (m_isWallRiding)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -m_wallNormal, out hit, wallCheckDistance, wallLayer))
            {
                m_wallNormal = hit.normal;
                Vector3 wallNormalNoY = new Vector3(m_wallNormal.x, 0, m_wallNormal.z);
                m_wallRunDir = Vector3.Cross(wallNormalNoY, Vector3.up).normalized;

                if (Vector3.Dot(m_wallRunDir, transform.forward) < 0)
                    m_wallRunDir *= -1;

                m_velocity = m_wallRunDir * wallRideSpeed;
                ciniCamera.Lens.Dutch = Mathf.Lerp(ciniCamera.Lens.Dutch, (Vector3.Dot(transform.right, m_wallNormal) > 0) ? -10f : 10f, Time.deltaTime * 8f);
                wallRideTimer += Time.deltaTime;
                return true;
            }
            else
            {
                m_isWallRiding = false;
                m_velocity = m_wallNormal * jumpForce + Vector3.up * jumpForce;
                jumpCount = maxJumpCount;
                wallRideTimer = 0f;
            }
        }

        m_isWallRiding = false;
        return false;
    }
    #endregion
    void HandleFOV()
    {
        float targetFOV = walkFOV;

        if (m_isDashing)
        {
            targetFOV = dashFOV;
        }
        else if (m_inputActions.Sprint.IsPressed() && canSprint)
        {
            targetFOV = sprintFOV;
        }

        float lerpSpeed = m_isDashing ? zoomSpeed * 1.5f : zoomSpeed;
        currFOV = Mathf.Lerp(currFOV, targetFOV, Time.deltaTime * lerpSpeed);

        ciniCamera.Lens.FieldOfView = currFOV;
    }

    void HandleDashing(float _delta)
    {
        m_currTime += _delta;
        Vector3 vertical = new Vector3(0, m_velocity.y, 0); // keep jump/gravity
        Vector3 finalVelocity = m_dashDirecton * dashForce * dashCurve.Evaluate(m_currTime) + vertical;

        transform.position = MovePlayer(finalVelocity * _delta);

        dashTime -= _delta;
        if (dashTime <= 0f)
        {
            m_isDashing = false;
            ciniCamera.Lens.FieldOfView = Mathf.Lerp(dashFOV, walkFOV, _delta * zoomSpeed);
            m_currTime = 0f;
            speedLines.GetComponent<SpeedLines>().speedLinesOn = false;
        }

    }

    bool HandleSliding()
    {
        
        bool onGround = CheckIfGrounded(out RaycastHit groundHit);

        if (!isSliding && m_slideInputPressd && onGround && m_velocity.magnitude >= slideMinSpeed)
        {
            isSliding = true;


            Vector3 horiz = new Vector3(m_velocity.x, 0f, m_velocity.z);
            m_slideDir = (horiz.magnitude > 0.5f) ? horiz.normalized : transform.forward;
            m_slideDir = Vector3.ProjectOnPlane(m_slideDir, groundHit.normal).normalized;

            float startSpeed = Mathf.Max(horiz.magnitude, startSlideSpeed);
            startSpeed = Mathf.Min(startSpeed, slideMaxSpeed);

            m_velocity = new Vector3(m_slideDir.x * startSpeed, 0f, m_slideDir.z * startSpeed);
        }

        if (!isSliding) return false;




        return false;
    }



    void OnDestroy()
    {
        m_inputActions.Disable();
    }
}
