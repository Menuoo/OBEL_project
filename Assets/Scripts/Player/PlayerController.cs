using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.InputSystem.Processors;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera playerCamera;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] WeaponControls weaponControls;
    [SerializeField] PlayerAnimationLogic playerAnim;

    [Header("Movement")]
    [SerializeField] float movementSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] float jumpStrength;
    [SerializeField] float gravity;

    [Header("Other")]
    [SerializeField] bool cameraRotEnabled = false;
    [SerializeField] Vector2 lookSensitivity;
    [SerializeField] float lookLimitV;
    [SerializeField] LayerMask groundLayers;

    public bool dead = false;

    public Vector3 walkDir { get; private set; }
    public float animSpeed { get; private set; }
    float currentSpeed = 0f;


    // ROTATION   ---   Player and Camera
    private float playerRotation = 0f;
    private Vector2 cameraRotation = Vector2.zero;

    // For Instant Camera Snaps
    Transform lastTrans;
    bool stillLast = false;


    float verticalVelocity = 0;
    bool isGrounded = false;
    float groundedCheckConstant = 0;

    public bool inMovementLock { get; private set; }
    public bool inRotationLock { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        playerRotation = this.transform.eulerAngles.y;
        lastTrans = CameraSingle.instance.transHolder.transform;

        inMovementLock = false;
        inRotationLock = false;

        lookSensitivity.Scale(new Vector2(0.01f, 0.01f));
        groundedCheckConstant = - characterController.height / 2f - characterController.skinWidth + characterController.radius - 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.Walk.magnitude <= 0.1f)
        {
            stillLast = false; // might change to something else tbh
        }

        CheckState();

        HandleVertical();
        Move();
    }

    private void LateUpdate()
    {
        MoveRotate(walkDir);
        Look();
    }


    void CheckState()
    { 
        isGrounded = IsGroundedWhileGrounded();
    }


    void Move()
    {
        Transform camTrans = playerCamera.transform;
        if (stillLast)
        {
            camTrans = lastTrans;
        }

        Vector3 cameraForward = new Vector3(camTrans.forward.x, 0f, camTrans.forward.z).normalized;
        Vector3 cameraRight = new Vector3(camTrans.right.x, 0f, camTrans.right.z).normalized;

        walkDir = cameraForward * playerInput.Walk.y + cameraRight * playerInput.Walk.x;

        float rotationFactor = 1f - Vector3.Angle(walkDir, transform.forward) / 180f;
        rotationFactor = math.max(rotationFactor, 0.1f);



        if (playerAnim.TryAimWalk(Vector3.SignedAngle(this.transform.forward, walkDir, Vector3.up))) // check if aiming, constant rotation factor
        {
            rotationFactor = 0.3f;
        }

        Vector3 velocity = walkDir * (playerInput.SprintPressed ? sprintSpeed : movementSpeed);
        velocity *= math.lerp(0f, 1f, rotationFactor);

        animSpeed = playerInput.SprintPressed ? 2f : 1.3f;


        if (inMovementLock)
        {
            velocity = Vector3.up * velocity.y;
        }

        currentSpeed = velocity.magnitude;

        velocity.y += verticalVelocity;
        characterController.Move(velocity * Time.deltaTime);
    }


    void MoveRotate(Vector3 walkDir)
    {
        if (inRotationLock)
            return;

        Vector3 nextDir = walkDir.normalized;

        if (weaponControls.isAiming)
        {
            nextDir = (playerInput.mousePosition - transform.position);
            nextDir.y = 0f;
            nextDir = nextDir.normalized;
        }



        float angle = Vector3.Angle(nextDir, transform.forward);
        float crossY = Vector3.Cross(transform.forward, nextDir).y;

        float rotationAmount = math.min(angle, rotateSpeed * Time.deltaTime);

        playerRotation += rotationAmount * math.sign(crossY);

        transform.rotation = Quaternion.Euler(0f, playerRotation, 0f);
    }


    void HandleVertical()
    {
        verticalVelocity -= gravity * Time.deltaTime;
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = 0;
        }
    }

    void Look()
    {
        if (!cameraRotEnabled)
        { 
            playerCamera.transform.rotation = Camera.main.transform.rotation;
            return;
        }

        cameraRotation.x += lookSensitivity.x * playerInput.Look.x;
        cameraRotation.y = math.clamp(cameraRotation.y - lookSensitivity.y * playerInput.Look.y,
                                -lookLimitV, lookLimitV);

        playerCamera.transform.rotation = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0f); // camera rotation application
    }


    private bool IsGroundedWhileGrounded()
    {
        Vector3 spherePos = new Vector3(transform.position.x,
            transform.position.y + groundedCheckConstant,
            transform.position.z);

        bool grounded = Physics.CheckSphere(spherePos, characterController.radius, groundLayers, QueryTriggerInteraction.Ignore);

        return grounded;
    }


    public void SetAnimationLock(bool movementState, bool rotationState)
    {
        inMovementLock = movementState;
        inRotationLock = rotationState;
    }

    public void LockDir()
    {
        stillLast = true;
        CameraSingle.instance.transHolder.transform.position = playerCamera.transform.position;
        CameraSingle.instance.transHolder.transform.rotation = playerCamera.transform.rotation;
        CameraSingle.instance.transHolder.transform.localScale = playerCamera.transform.localScale;

        Debug.Log("direction locked");
    }

    public float GetSpeed() => currentSpeed;

    public PlayerInput GetInput() => playerInput;
    public void SetPosition(Vector3 vec)
    { 
        characterController.enabled = false;
        transform.position = vec;
        characterController.enabled = true;
    }
    public void SetRotation(float rot)
    {
        playerRotation = rot;
    }


    public void Die()
    {
        dead = true;
        playerAnim.Die();

        playerInput.SwapControls(-1);
        this.enabled = false;
        characterController.enabled = false;
    }
}