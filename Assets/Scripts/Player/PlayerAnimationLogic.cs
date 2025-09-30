using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationLogic : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    PlayerController playerController;
    PlayerInput playerInput;

    bool rightHandHold = true;
    bool addAvailable = false;


    private static int rightHandHash = Animator.StringToHash("rightHandHold");
    private static int walkingHash = Animator.StringToHash("isWalking");
    private static int walkSpeedHash = Animator.StringToHash("walkSpeed");
    private static int aimingHash = Animator.StringToHash("isAiming");
    private static int slicingHash = Animator.StringToHash("isSlicing");
    private static int additionalActionHash = Animator.StringToHash("additionalAction");


    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerInput = GetComponent<PlayerInput>();
    }


    void Update()
    {
        if (!playerController.inAnimationLock)
        {
            playerAnimator.SetBool(rightHandHash, rightHandHold);

            playerAnimator.SetBool(walkingHash, playerController.walkDir.magnitude != 0);
            playerAnimator.SetFloat(walkSpeedHash, playerController.animSpeed);
        }
    }

    public void WeaponAction(CurrentWeapon weapon)
    {
        if (weapon == CurrentWeapon.Knife)
        {
            playerAnimator.SetBool(slicingHash, true);

            if (addAvailable)
                playerAnimator.SetBool(additionalActionHash, true);
        }

        if (weapon == CurrentWeapon.Pistol)
        {
            if (addAvailable)
                playerAnimator.SetBool(additionalActionHash, true);
        }
    }

    public void Aiming(bool state)
    {
        playerAnimator.SetBool(aimingHash, state);

        playerController.SetAnimationLock(state);
    }



    public void ResetAdd()
    {
        playerAnimator.SetBool(additionalActionHash, false);
    }

    public void ResetSlice()
    {
        playerAnimator.SetBool(slicingHash, false);
    }

    public void SetAddAvailability(bool state)
    { 
        addAvailable = state;
    }
}
