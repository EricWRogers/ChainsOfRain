using Unity.Cinemachine;
using UnityEngine;
using KinematicCharacterControler;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace KinematicCharacterControler{}
    public enum Stance
    {
        Standing,
        Crouching,
        Sliding,
        Grinding
    }
public class PlayerMovement : MovementEngine
{
    public static PlayerMovement instance;
    [Header("Current State")]
    public Stance currentStance = Stance.Standing;
    public Stance prevStance = Stance.Standing;

    [Header("Movement")]
    public GameObject speedLines;
    public float speed = 5f;
    public float runSpeed = 10f;
    public float sprintFOV = 70f;
    public float walkFOV = 60f;
    private float currFOV = 60f;
    public bool canSprint = true;
    public KeyCode sprintKey = KeyCode.LeftShift;
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
    public float wallRideSpeed = 8f;
    public float wallRideGravity = -1f;
    public float wallCheckDistance = 1f;
    public float wallStickForce = 5f;
    public float maxWallRideTime = 2f;
    public bool canWallRide = true;
    public LayerMask wallLayer;

    private bool isWallRiding = false;
    private Vector3 wallNormal;
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


    [Header("Sliding")]
    public KeyCode slideKey = KeyCode.LeftControl;
    public bool isSliding = false;
    public bool canSlide = false;
    public float startSlideSpeed = 25;
    public float endSlideSpeed = 15;
    private Vector3 m_slideDirection;
    public float maxSlideAngle = 70;
    public float slideForce;
    private float m_slideSpeed;


    public float slideSpeedMultiplier = 1.5f;
    public float slideMinSpeed = 12f;
    public float slideMaxSpeed = 35f;
    public float slideAcceleration = 25f;
    public float slideDeceleration = 8f;
    public float slopeSlideBonus = 20f;
    public float slideJumpHeight = 8f;
    public float slideJumpForward = 15f;
    public float slideTurnSpeed = 180f;
    public float slideHeightMultiplier = 0.4f;
    public bool preserveAirMomentum = true;
    public float momentumDecayRate = 0.95f;

    // Internal slide state
    private float currentSlideSpeed = 0f;
    private Vector3 slideVelocity = Vector3.zero;
    private bool wasGroundedLastFrame = false;
    private float slideStartTime = 0f;

    [Header("Rail Grinding")]
    public LayerMask railLayer;
    public float railDetectionRadius = 1.5f;
    public float railSnapDistance = 2f;
    public float minGrindSpeed = 3f;
    public float grindExitForce = 8f;

    [Header("Grinding Input")]
    public KeyCode grindKey = KeyCode.LeftControl;

    // Grinding state
    public bool isGrinding { get; private set; }
    public Rail currentRail;
    public float railProgress;
    public float grindSpeed;
    public Vector3 grindVelocity;
    public bool grindInputHeld;
    public float m_railDir = 1f;
    [SerializeField] private Transform m_railDetectionPoint;

    private Vector3 kbDir;
    private float kbStrength;
    private bool isTakingKB = false;
    private Vector3 lasPos;

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
    }
    void Start()
    {
        player = GameObject.Find("Player");
        m_orientation = cam;
        lasPos = transform.position;
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

    public void ChangeState(Stance newState)
    {
        if (currentStance == newState) return;

        prevStance = currentStance;

        switch (newState)
        {
            case Stance.Standing:
                currentStance = newState;
                break;
            case Stance.Sliding:
                currentStance = newState;
                break;
            case Stance.Grinding:
                currentStance = newState;
                break;

        }
    }
    void Update()
    {
        //HandleCursor();
        UpdateGrindInput();
        HandleInput();
        HandleFOV();

        if (!isGrinding)
        {
            HandleRegularMovement();
            //TryStartGrinding();
        }
        else
        {
            //ContinueGrinding();
        }
        lasPos = transform.position;
    }
    void HandleCursor()
    {
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
    }

    void UpdateGrindInput()
    {
        grindInputHeld = Input.GetKey(grindKey);
    }

    void HandleInput()
    {
        mouseInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetKey(KeyCode.Space))
            m_jumpInputPressed = true;
        else
            m_jumpInputPressed = false;

        if (m_jumpInputPressed)
            jumpInputElapsed = 0.0f;
        else
            jumpInputElapsed += Time.deltaTime;

        if (Input.GetKeyDown(crouchKey) && mouseInput.magnitude > 0.01)
        {
            StartSliding();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = spwanPos;
        }


        if (Input.GetKeyDown(dashKey) && m_dashCooldownTimer <= 0f && !m_isDashing && dashForce > 0f)
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

    }
    void HandleRegularMovement()
    {
        if (isTakingKB)
        {
            HandleKnockBack();
            return;
        }

        if (m_isDashing)
        {
            HandleDashing(Time.deltaTime);
            return;
        }
        else if (isWallRiding)
        {
            HandleWallRide();
            return;
        }
        else if (isSliding)
        {
            HandleSliding();
            return;
        }

        ciniCamera.Lens.Dutch = 0f;


        if (!isWallRiding && CheckForWall(transform.position, wallCheckDistance, out RaycastHit _wallHit))
        {
            if (m_jumpInputPressed && canWallRide)
            {
                StartWallRide(_wallHit);
            }
        }



        Vector3 inputDir = transform.TransformDirection(new Vector3(mouseInput.x, 0, mouseInput.y));


        bool onGround = CheckIfGrounded(out RaycastHit groundHit) && m_velocity.y <= 0.0f;
        bool falling = !(onGround && maxWalkAngle >= Vector3.Angle(Vector3.up, groundHit.normal));

        // Handle gravity and falling
        if (falling)
        {
            m_velocity += gravity * Time.deltaTime;
            m_elapsedFalling += Time.deltaTime;
        }
        else if (onGround && !isSliding)
        {
            m_velocity = Vector3.zero;
            m_elapsedFalling = 0;
            jumpCount = maxJumpCount;
        }

        // Handle jumping
        bool shouldJump = ((onGround && groundedState.angle <= maxJumpAngle) || (canDoubleJump && jumpCount > 0))
                            && canJump && m_timeSinceLastJump >= jumpCooldown;

        bool attemptingJump = jumpInputElapsed <= m_jumpBufferTime;


        if (shouldJump && attemptingJump)
        {
            jumpCount -= 1;
            m_velocity = Vector3.up * jumpForce;
            m_timeSinceLastJump = 0.0f;
            jumpInputElapsed = Mathf.Infinity;
        }
        else
        {
            m_timeSinceLastJump += Time.deltaTime;
        }

        Vector3 finalDir;

        if (Input.GetKey(sprintKey))
        {
            finalDir = inputDir * runSpeed;

        }
        else if (isCrouching)
        {
            finalDir = inputDir * crouchSpeed;

        }
        else
        {
            finalDir = inputDir * speed;
        }

        m_velocity += finalDir;
        // Apply movement
        //transform.position = MovePlayer(finalDir * Time.deltaTime);



        transform.position = MovePlayer(m_velocity * Time.deltaTime);
        transform.rotation = new Quaternion(transform.rotation.x, cam.transform.rotation.y, transform.rotation.z, cam.rotation.w);
        m_velocity = new Vector3(0, m_velocity.y, 0);

        if (m_dashCooldownTimer > 0)
        {
            m_dashCooldownTimer -= Time.deltaTime;
        }

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

    void HandleFOV()
    {
        float targetFOV = walkFOV;

        if (m_isDashing)
        {
            targetFOV = dashFOV;
        }
        else if (Input.GetKey(sprintKey))
        {
            targetFOV = sprintFOV;
        }
        else if (isCrouching)
        {
            targetFOV = walkFOV;
        }

        float lerpSpeed = m_isDashing ? zoomSpeed * 1.5f : zoomSpeed;
        currFOV = Mathf.Lerp(currFOV, targetFOV, Time.deltaTime * lerpSpeed);

        ciniCamera.Lens.FieldOfView = currFOV;
    }

    void StartWallRide(RaycastHit _wallHit)
    {
        isWallRiding = true;
        wallNormal = _wallHit.normal;
        wallRideTimer = maxWallRideTime;
        m_velocity.y = 0;
    }

    void HandleWallRide()
    {
        if (CheckForWall(transform.position, wallCheckDistance, out RaycastHit hit))
        {
            wallNormal = hit.normal;
        }
        else
        {
            ExitWallRide();
        }

        if (Physics.Raycast(transform.position, transform.right, out _, wallCheckDistance, wallLayer))
        {
            ciniCamera.Lens.Dutch = 10;
        }
        else
        {
            ciniCamera.Lens.Dutch = -10;
        }

        Vector3 wallDirection = Vector3.Cross(wallNormal, Vector3.up).normalized;

        if (Vector3.Dot(wallDirection, transform.forward) < 0)
            wallDirection *= -1;

        Vector3 horizontal = wallDirection * wallRideSpeed;
        Vector3 vertical = new Vector3(0, m_velocity.y, 0);

        m_velocity = horizontal + vertical;
        m_velocity.y += wallRideGravity * Time.deltaTime;

        transform.position = MovePlayer(m_velocity * Time.deltaTime);

        wallRideTimer -= Time.deltaTime;
        if (wallRideTimer <= 0f || CheckIfGrounded(out _))
        {
            ExitWallRide();
        }
    }

    void ExitWallRide()
    {
        isWallRiding = false;
        ciniCamera.Lens.Dutch = 0;

    }


    bool CheckForWall(Vector3 _pos, float _dist, out RaycastHit _hit)
    {
        if (Physics.Raycast(_pos, transform.right, out _hit, wallCheckDistance, wallLayer))
            return true;
        if (Physics.Raycast(_pos, -transform.right, out _hit, wallCheckDistance, wallLayer))
            return true;

        return false;
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
    void HandleCrouch()
    {
        if (m_requestedCrouch && currentStance == Stance.Standing)
        {
            currentStance = Stance.Crouching;
            capsule.height = crouchHeight;
            capsule.center = new Vector3(0, -0.25f, 0);
            isCrouching = true;
            camPoint.transform.position -= new Vector3(0, 0.5f, 0);
            return;

        }

        if ((m_requestedCrouch || m_jumpInputPressed) && currentStance == Stance.Crouching)
        {
            currentStance = Stance.Standing;
            capsule.height = capsuleHeight;
            capsule.center = Vector3.zero;
            isCrouching = false;
            camPoint.transform.position += new Vector3(0, 0.5f, 0);
        }
    }


    void StartSliding()
    {

    }
    void HandleSliding()
    {
        bool onGround = CheckIfGrounded(out RaycastHit groundHit);
        /*
        if (!isSliding)
        {
            // Start back slide
            isSliding = true;
            isCrouching = false;
            isTransitioningToSlide = true;
            isTransitioningFromSlide = false;

            // Store original rotation
            originalRotation = transform.rotation;

            // Calculate target rotation (rotate around local X-axis to lie on back)
            slideTargetRotation = originalRotation * Quaternion.Euler(backSlideRotation);

            // Get current horizontal velocity
            Vector3 horizontalVel = new Vector3(m_velocity.x, 0, m_velocity.z);
            float currentHorizontalSpeed = horizontalVel.magnitude;

            // Determine slide direction
            Vector3 inputDir = transform.TransformDirection(new Vector3(mouseInput.x, 0, mouseInput.y));

            if (currentHorizontalSpeed > 1f)
            {
                m_slideDirection = horizontalVel.normalized;
            }
            else if (inputDir.magnitude > 0.1f)
            {
                m_slideDirection = inputDir.normalized;
            }
            else
            {
                m_slideDirection = transform.forward;
            }

            // Set initial slide speed
            currentSlideSpeed = Mathf.Max(currentHorizontalSpeed * slideSpeedMultiplier, slideMinSpeed);

            ChangeState(Stance.Sliding);

            // DON'T change capsule height/center - we're using rotation instead
        }

        // Handle rotation transitions
        if (isTransitioningToSlide)
        {
            // Smoothly rotate to slide position
            transform.rotation = Quaternion.Slerp(transform.rotation, slideTargetRotation, 
                                                slideRotationSpeed * Time.deltaTime);

            // Check if rotation is complete
            if (Quaternion.Angle(transform.rotation, slideTargetRotation) < 5f)
            {
                transform.rotation = slideTargetRotation;
                isTransitioningToSlide = false;
            }
        }
        else if (isTransitioningFromSlide)
        {
            // Smoothly rotate back to standing
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, 
                                                slideRotationSpeed * Time.deltaTime);

            // Check if rotation is complete
            if (Quaternion.Angle(transform.rotation, originalRotation) < 5f)
            {
                transform.rotation = originalRotation;
                isTransitioningFromSlide = false;
            }
        }

        // Get input for direction changes
        Vector3 inputDir = transform.TransformDirection(new Vector3(mouseInput.x, 0, mouseInput.y));

        if (onGround)
        {
            Vector3 groundNormal = groundHit.normal;
            float slopeAngle = Vector3.Angle(Vector3.up, groundNormal);

            // Project slide direction onto ground
            Vector3 projectedSlideDir = Vector3.ProjectOnPlane(m_slideDirection, groundNormal).normalized;

            // Directional control
            if (inputDir.magnitude > 0.1f)
            {
                Vector3 targetDir = Vector3.ProjectOnPlane(inputDir.normalized, groundNormal).normalized;
                float turnRate = slideTurnSpeed * Time.deltaTime;
                m_slideDirection = Vector3.Slerp(projectedSlideDir, targetDir, turnRate / 180f).normalized;
            }
            else
            {
                m_slideDirection = projectedSlideDir;
            }

            // Speed management (same as before)
            if (slopeAngle > 10f && slopeAngle <= maxSlideAngle)
            {
                Vector3 slopeDown = Vector3.ProjectOnPlane(Vector3.down, groundNormal);
                float slopeInfluence = Vector3.Dot(m_slideDirection, slopeDown.normalized);

                if (slopeInfluence > 0)
                {
                    float speedGain = slopeSlideBonus * slopeInfluence * Time.deltaTime;
                    currentSlideSpeed += speedGain;
                }
                else
                {
                    currentSlideSpeed -= slideDeceleration * 0.5f * Time.deltaTime;
                }
            }
            else
            {
                if (currentSlideSpeed < slideMaxSpeed)
                {
                    currentSlideSpeed += slideAcceleration * Time.deltaTime;
                }
                else
                {
                    currentSlideSpeed -= slideDeceleration * 0.3f * Time.deltaTime;
                }
            }

            currentSlideSpeed = Mathf.Clamp(currentSlideSpeed, slideMinSpeed, slideMaxSpeed);

            // Set velocity
            slideVelocity = m_slideDirection * currentSlideSpeed;
            m_velocity = new Vector3(slideVelocity.x, 0f, slideVelocity.z);

            // IMPORTANT: Modified ground snapping for rotated player
            SnapPlayerDownRotated(groundHit);
            wasGroundedLastFrame = true;
        }
        else
        {
            // Air handling (mostly same as before)
            if (wasGroundedLastFrame && preserveAirMomentum)
            {
                slideVelocity = m_slideDirection * currentSlideSpeed;
            }

            m_velocity += gravity * Time.deltaTime;
            m_velocity = new Vector3(slideVelocity.x, m_velocity.y, slideVelocity.z);

            if (inputDir.magnitude > 0.1f)
            {
                Vector3 airControl = inputDir * speed * 0.2f * Time.deltaTime;
                slideVelocity += new Vector3(airControl.x, 0, airControl.z);
                slideVelocity = Vector3.ClampMagnitude(slideVelocity, slideMaxSpeed);
            }

            slideVelocity *= momentumDecayRate;
            currentSlideSpeed = slideVelocity.magnitude;
            wasGroundedLastFrame = false;
        }

        // Apply movement
        transform.position = MovePlayer(m_velocity * Time.deltaTime);

        // Exit conditions
        bool shouldExit = false;

            if (m_jumpInputPressed)
            {
                if (canJump)
                {
                    Vector3 jumpVel = Vector3.up * slideJumpHeight + m_slideDirection * slideJumpForward;
                    m_velocity = jumpVel;

                    if (onGround)
                    {
                        jumpCount -= 1;
                        m_timeSinceLastJump = 0.0f;
                        jumpInputElapsed = Mathf.Infinity;
                    }
                }
                shouldExit = true;
            */


        void ExitSlide()
        {
            if (!isSliding) return;

            isSliding = false;


            capsule.height = capsuleHeight;
            capsule.center = Vector3.zero;


            if (currentSlideSpeed > speed)
            {
                float momentumKeep = 0.8f;
                Vector3 exitVelocity = m_slideDirection * currentSlideSpeed * momentumKeep;


                m_velocity = new Vector3(exitVelocity.x, m_velocity.y, exitVelocity.z);
            }

            ChangeState(Stance.Standing);

            // Reset slide state
            currentSlideSpeed = 0f;
            slideVelocity = Vector3.zero;
            wasGroundedLastFrame = false;
        }


        // RAIL GRINDING SYSTEM
        void TryStartGrinding()
        {


            // Check for nearby rails
            Collider[] railColliders = Physics.OverlapSphere(m_railDetectionPoint.position, railDetectionRadius, railLayer);

            Rail closestRail = null;
            float closestDistance = float.MaxValue;
            float bestProgress = 0f;

            foreach (var collider in railColliders)
            {
                Rail rail = collider.GetComponent<Rail>();
                if (rail == null) continue;

                // Find closest point on this rail
                float progress;
                Vector3 closestPoint = GetClosestPointOnRail(rail, transform.position, out progress);
                float distance = Vector3.Distance(transform.position, closestPoint);

                if (distance < closestDistance && distance <= railSnapDistance)
                {
                    closestDistance = distance;
                    closestRail = rail;
                    bestProgress = progress;
                }
            }

            if (closestRail != null)
            {
                StartGrinding(closestRail, bestProgress);
            }
        }

        Vector3 GetClosestPointOnRail(Rail rail, Vector3 position, out float progress)
        {
            progress = 0f;
            if (rail.railPoints == null || rail.railPoints.Length < 2) return Vector3.zero;

            Vector3 closestPoint = rail.railPoints[0].position;
            float closestDistance = Vector3.Distance(position, closestPoint);

            for (int i = 0; i < rail.railPoints.Length - 1; i++)
            {
                Vector3 lineStart = rail.railPoints[i].position;
                Vector3 lineEnd = rail.railPoints[i + 1].position;

                Vector3 pointOnLine = GetClosestPointOnLine(lineStart, lineEnd, position);
                float distance = Vector3.Distance(position, pointOnLine);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = pointOnLine;

                    float segmentLength = Vector3.Distance(lineStart, lineEnd);
                    float distanceAlongSegment = Vector3.Distance(lineStart, pointOnLine);
                    float localProgress = segmentLength > 0 ? distanceAlongSegment / segmentLength : 0f;

                    progress = (i + localProgress) / (rail.railPoints.Length - 1);
                }
            }

            return closestPoint;
        }

        Vector3 GetClosestPointOnLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            Vector3 lineDirection = lineEnd - lineStart;
            float lineLength = lineDirection.magnitude;
            lineDirection.Normalize();

            Vector3 toPoint = point - lineStart;
            float projectedDistance = Vector3.Dot(toPoint, lineDirection);
            projectedDistance = Mathf.Clamp(projectedDistance, 0f, lineLength);

            return lineStart + lineDirection * projectedDistance;
        }

        void StartGrinding(Rail rail, float progress)
        {
            isGrinding = true;
            currentRail = rail;
            railProgress = progress;

            ;
            grindSpeed = speed;

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 inputDir = m_orientation.forward * vertical + m_orientation.right * horizontal;

            // Get rail direction at this point
            Vector3 railDir = rail.GetDirectionOnRail(progress);

            // Determine which direction along the rail matches player's movement better
            float forwardDot = Vector3.Dot(transform.forward, railDir);
            float backwardDot = Vector3.Dot(transform.forward, -railDir);

            m_railDir = forwardDot > backwardDot ? 1f : -1f;

            Vector3 railPosition = rail.GetPointOnRail(progress);
            transform.position = railPosition;

            m_velocity = Vector3.zero;
        }

        void ContinueGrinding()
        {
            if (currentRail == null)
            {
                ExitGrinding();
                return;
            }

            if (m_jumpInputPressed)
            {
                ExitGrinding();
                return;
            }

            if (!currentRail.isLoop)
            {
                // Exit if at end of rail
                if (railProgress >= 1f || railProgress <= 0f)
                {
                    ExitGrinding();
                    return;
                }
            }

            // Calculate movement along rail
            Vector3 railDirection = currentRail.GetDirectionOnRail(railProgress) * m_railDir;


            Vector3 railMovement = railDirection * grindSpeed * Time.deltaTime;


            Vector3 currentPos = transform.position;
            Vector3 newPosition = MovePlayer(railMovement);
            transform.position = newPosition;

            // Update rail progress based on actual movement achieved along rail direction
            Vector3 actualMovement = transform.position - currentPos;
            float actualDistance = Vector3.Dot(actualMovement, railDirection);

            float railLength = currentRail.GetRailLength();
            if (railLength > 0)
            {
                float progressDelta = (actualDistance / railLength) * m_railDir;
                railProgress += progressDelta;
            }



            Vector3 idealRailPosition = currentRail.GetPointOnRail(railProgress);
            Vector3 currentRailPosition = transform.position;

            // Only correct position if we've drifted too far from the rail
            float driftDistance = Vector3.Distance(currentRailPosition, idealRailPosition);
            if (driftDistance > 0.5f) // Allow some tolerance
            {
                // Gradually pull back to rail instead of snapping
                Vector3 correctionDirection = (idealRailPosition - currentRailPosition).normalized;
                Vector3 correction = correctionDirection * Mathf.Min(driftDistance, 2f * Time.deltaTime);
                transform.position += correction;
            }

            // Store grind velocity for potential exit
            grindVelocity = railDirection * grindSpeed;
        }

        void ExitGrinding()
        {
            if (!isGrinding) return;

            isGrinding = false;

            // Give player exit velocity
            if (currentRail != null)
            {
                Vector3 railDirection = currentRail.GetDirectionOnRail(railProgress) * m_railDir;
                m_velocity = railDirection * grindSpeed;

                m_velocity.y = grindExitForce;
            }

            currentRail = null;
            railProgress = 0f;
            grindSpeed = 0f;
        }

        // Visualization
        void OnDrawGizmos()
        {
            if (isGrinding && currentRail != null)
            {
                Gizmos.color = Color.yellow;
                Vector3 railPos = currentRail.GetPointOnRail(railProgress);
                Gizmos.DrawWireSphere(railPos, 0.5f);

                Vector3 railDir = currentRail.GetDirectionOnRail(railProgress) * m_railDir;
                Gizmos.DrawRay(railPos, railDir * 2f);
            }

            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(m_railDetectionPoint.position, railDetectionRadius);

        }
    }

    
}
