using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationLogic : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    PlayerController playerController;
    PlayerInput playerInput;
    WeaponControls weaponControls;

    public static event Action<bool> KnifeAction;

    float verticalValue = 0.5f;
    float verticalTarget = 0.5f;
    float verticalSpeed = 1f;


    float intentionTimer = 0f;

    float flinchTime = 0f;
    bool isFlinching = false;


    bool rightHandHold = true;
    bool addAvailable = false;
    public bool actionIntention { get; private set; }

    float nextAngle = 0f;
    float currAngle = 0f;
    float angleSpeed = 20f;

    private static int rightHandHash = Animator.StringToHash("rightHandHold");
    private static int walkingHash = Animator.StringToHash("isWalking");
    private static int walkSpeedHash = Animator.StringToHash("walkSpeed");
    private static int aimingHash = Animator.StringToHash("isAiming");
    private static int aimHeightHash = Animator.StringToHash("aimHeight");
    private static int slicingHash = Animator.StringToHash("isSlicing");
    private static int additionalActionHash = Animator.StringToHash("additionalAction");
    private static int inActionHash = Animator.StringToHash("inAction");
    private static int flinchingHash = Animator.StringToHash("flinching");
    private static int dirXHash = Animator.StringToHash("dirX");
    private static int dirYHash = Animator.StringToHash("dirY");


    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerInput = GetComponent<PlayerInput>();
        weaponControls = GetComponent<WeaponControls>();
    }


    void Update()
    {
        // adjust vertical aim
        //verticalValue = math.lerp(verticalValue, verticalTarget, Time.deltaTime * verticalSpeed);
        verticalValue = verticalValue > verticalTarget ?
            math.max(verticalValue - verticalSpeed * Time.deltaTime, verticalTarget) :
            math.min(verticalValue + verticalSpeed * Time.deltaTime, verticalTarget);
        playerAnimator.SetFloat(aimHeightHash, verticalValue);


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

        playerAnimator.SetBool(rightHandHash, rightHandHold);

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

        // for vertical aim
        verticalTarget = weaponControls.GetPistol().GetTargetValue();

        if (state)
            playerController.SetAnimationLock(false, playerController.inRotationLock);  // movement is NOT locked, currently
        else if (!playerAnimator.GetBool(inActionHash))
            playerController.SetAnimationLock(false, false);
    }
    public bool TryAimWalk(float angle)//Vector3 dir)
    {
        nextAngle = angle;

        float diff = Mathf.Abs(currAngle - nextAngle);

        if (diff > 330f)
        {
            diff = 360f - diff;
            if (nextAngle < currAngle)
            {
                currAngle = Mathf.Lerp(currAngle, currAngle + diff, angleSpeed * Time.deltaTime);
                currAngle = currAngle > 180 ? currAngle - 360f : currAngle;
            }
            else
            {
                currAngle = Mathf.Lerp(currAngle, currAngle - diff, angleSpeed * Time.deltaTime);
                currAngle = currAngle < -180 ? currAngle + 360f : currAngle;
            }
        }
        else
            currAngle = Mathf.Lerp(currAngle, nextAngle, angleSpeed * Time.deltaTime);

        if (!playerAnimator.GetBool(aimingHash))
        {
            return false;
        }

        Vector2 dir = new Vector2(Mathf.Sin(Mathf.Deg2Rad * currAngle), Mathf.Cos(Mathf.Deg2Rad * currAngle));

        playerAnimator.SetFloat(dirXHash, dir.x);
        playerAnimator.SetFloat(dirYHash, dir.y);
        return true;
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
