using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera playerCamera;
    [SerializeField] PlayerInput input;

    [Header("Movement")]
    [SerializeField] float movementSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float jumpStrength;
    [SerializeField] float gravity;

    [Header("Other")]
    [SerializeField] Vector2 lookSensitivity;
    [SerializeField] float lookLimitV;
    [SerializeField] LayerMask groundLayers;


    private Vector2 cameraRotation = Vector2.zero;
    float verticalVelocity = 0;
    bool isGrounded = false;
    float groundedCheckConstant = 0;


    // Start is called before the first frame update
    void Start()
    {
        lookSensitivity.Scale(new Vector2(0.01f, 0.01f));
        groundedCheckConstant = - characterController.height / 2f - characterController.skinWidth + characterController.radius - 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
        Look();

        HandleVertical();
        Move();
    }


    void CheckState()
    { 
        isGrounded = IsGroundedWhileGrounded();
    }


    void Move()
    {
        Vector3 cameraForward = new Vector3(playerCamera.transform.forward.x, 0f, playerCamera.transform.forward.z).normalized;
        Vector3 cameraRight = new Vector3(playerCamera.transform.right.x, 0f, playerCamera.transform.right.z).normalized;

        Vector3 walkDir = cameraForward * input.Walk.y + cameraRight * input.Walk.x;


        float rotationFactor = MoveRotate(walkDir);

        Vector3 velocity = walkDir * (input.SprintPressed ? sprintSpeed : movementSpeed);
        velocity *= math.lerp(0f, 1f, rotationFactor);

        velocity.y += verticalVelocity;

        characterController.Move(velocity * Time.deltaTime);
    }

    float MoveRotate(Vector3 walkDir)
    {
        float factor = 1f;

        float angle = Vector3.Angle(walkDir, transform.forward);
        float dot = Vector3.Cross(transform.forward, walkDir).y;

        float rotationAmount = math.min(angle, rotateSpeed * Time.deltaTime);


        transform.Rotate(Vector3.up * math.sign(dot), rotationAmount);

        factor = factor - Vector3.Angle(walkDir, transform.forward) / 180f;
        return factor;
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
        cameraRotation.x += lookSensitivity.x * input.Look.x;
        cameraRotation.y = math.clamp(cameraRotation.y - lookSensitivity.y * input.Look.y,
                                -lookLimitV, lookLimitV);

        //playerCamera.transform.rotation = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0f); // camera rotation application
    }


    private bool IsGroundedWhileGrounded()
    {
        Vector3 spherePos = new Vector3(transform.position.x,
            transform.position.y + groundedCheckConstant,
            transform.position.z);

        bool grounded = Physics.CheckSphere(spherePos, characterController.radius, groundLayers, QueryTriggerInteraction.Ignore);

        return grounded;
    }
}