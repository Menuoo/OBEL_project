using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationLogic : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    PlayerController playerController;
    PlayerInput playerInput;
    WeaponControls weaponControls;

    public static event Action<bool> KnifeAction;


    float intentionTimer = 0f;

    float flinchTime = 0f;
    bool isFlinching = false;


    bool rightHandHold = true;
    bool addAvailable = false;
    public bool actionIntention { get; private set; }


    private static int rightHandHash = Animator.StringToHash("rightHandHold");
    private static int walkingHash = Animator.StringToHash("isWalking");
    private static int walkSpeedHash = Animator.StringToHash("walkSpeed");
    private static int aimingHash = Animator.StringToHash("isAiming");
    private static int aimHeightHash = Animator.StringToHash("aimHeight");
    private static int slicingHash = Animator.StringToHash("isSlicing");
    private static int additionalActionHash = Animator.StringToHash("additionalAction");
    private static int inActionHash = Animator.StringToHash("inAction");
    private static int flinchingHash = Animator.StringToHash("flinching");

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerInput = GetComponent<PlayerInput>();
        weaponControls = GetComponent<WeaponControls>();
    }


    void Update()
    {
        if (intentionTimer > 0f)
        {
            intentionTimer -= Time.deltaTime;

            if (intentionTimer <= 0f)
                ResetIntention();
        }


        if (isFlinching)
        {
            if (flinchTime > 0)
            {
                flinchTime -= Time.deltaTime;
            }
            else 
            { 
                FinishFlinch();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Flinch();
        }

        //playerAnimator.SetBool(rightHandHash, rightHandHold);

        playerAnimator.SetBool(walkingHash, playerController.walkDir.magnitude != 0);
        playerAnimator.SetFloat(walkSpeedHash, playerController.animSpeed);
    }


    public void WeaponAction(CurrentWeapon weapon)
    {
        actionIntention = true;
        if (intentionTimer <= 0f)
            intentionTimer = 0.3f;

        if (weapon == CurrentWeapon.Knife)
        {
            playerAnimator.SetBool(slicingHash, true);

            if (addAvailable)
            {
                playerAnimator.SetBool(additionalActionHash, true);
            }
        }

        if (weapon == CurrentWeapon.Pistol)
        {
            if (addAvailable && !isFlinching)
            {
                weaponControls.GetPistol().HandleShot(playerInput);
                //playerAnimator.SetBool(additionalActionHash, true);
            }
        }
    }

    public void ResetIntention()
    {
        if (!playerAnimator.GetBool(inActionHash))
        {
            Debug.Log("intention reset");
            actionIntention = false;
            ResetActions();
        }
    }


    public void Aiming(bool state)
    {
        playerAnimator.SetBool(aimingHash, state);

        float verticalValue = weaponControls.GetPistol().GetTargetValue();
        playerAnimator.SetFloat(aimHeightHash, verticalValue);

        if (state)
            playerController.SetAnimationLock(state, playerController.inRotationLock);
        else if (!playerAnimator.GetBool(inActionHash))
            playerController.SetAnimationLock(false, false);
    }





    public void SetInAction(bool state)
    {
        playerAnimator.SetBool(inActionHash, state);
    }

    public void SetKnife(bool state)
    {
        KnifeAction?.Invoke(state);
    }

    public void ResetActions()
    {
        playerAnimator.SetBool(additionalActionHash, false);
        playerAnimator.SetBool(slicingHash, false);
        playerAnimator.SetBool(inActionHash, false);
        actionIntention = false;
        addAvailable = false;

        SetKnife(false);
    }





    public void Flinch()
    {
        ResetActions();
        isFlinching = true;
        playerAnimator.SetBool(flinchingHash, true);
        playerController.SetAnimationLock(true, true);

        addAvailable = false;
        flinchTime = 1f;
    }

    public void FinishFlinch()
    {
        ResetActions();
        isFlinching = false;
        playerAnimator.SetBool(flinchingHash, false);
        playerController.SetAnimationLock(false, false);
    }





    public void SetAddAvailability(bool state)
    { 
        addAvailable = state;
    }

    public void ResetAdd()
    {
        playerAnimator.SetBool(additionalActionHash, false);
    }

    public void ResetSlice()
    {
        playerAnimator.SetBool(slicingHash, false);
    }
}
