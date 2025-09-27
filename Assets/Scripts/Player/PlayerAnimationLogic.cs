using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationLogic : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    PlayerController playerController;
    PlayerInput playerInput;

    bool rightHandHold = false;


    private static int rightHandHash = Animator.StringToHash("rightHandHold");
    private static int walkingHash = Animator.StringToHash("isWalking");
    private static int walkSpeedHash = Animator.StringToHash("walkSpeed");


    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerInput = GetComponent<PlayerInput>();
    }


    void Update()
    {
        playerAnimator.SetBool(rightHandHash, rightHandHold);
        playerAnimator.SetBool(walkingHash, playerController.walkDir.magnitude != 0);
        playerAnimator.SetFloat(walkSpeedHash, playerController.animSpeed);
    }
}
