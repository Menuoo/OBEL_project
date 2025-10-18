using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandling : MonoBehaviour
{
    [SerializeField] PlayerAnimationLogic playerAnim;
    [SerializeField] PlayerController playerController;

    public void ResetSlice()
    {
        playerAnim.ResetSlice();
    }

    public void ResetAdditionalAction()
    {
        playerAnim.ResetAdd();

        playerAnim.ResetSlice();
    }

    public void SetAddAvailable()
    {
        playerAnim.SetAddAvailability(true);
    }

    public void SetAddUnavailable()
    {
        playerAnim.SetAddAvailability(false);
    }


    public void Action_End()
    {
        playerController.SetAnimationLock(false, false);
        playerAnim.ResetActions();
        playerAnim.SetInAction(false);
    }


    public void Slash1_Start()
    {
        if (playerAnim.actionIntention)
        {
            playerController.SetAnimationLock(true, true);
            playerAnim.SetInAction(true);

            playerAnim.SetKnife(true);
        }
    }

    public void Slash2_Start()
    {
        if (playerAnim.actionIntention)
        {
            playerAnim.SetAddAvailability(false);
            playerController.SetAnimationLock(true, true);
            playerAnim.SetInAction(true);

            playerAnim.SetKnife(true);
        }
    }

    public void Shoot_Start()
    {
        if (playerAnim.actionIntention)
        {
            playerAnim.SetAddAvailability(false);
            playerController.SetAnimationLock(true, true);
            playerAnim.SetInAction(true);
        }
    }




    // Control locking

    /*public void LockRotationTrue()
    {
        playerController.SetAnimationLock(playerController.inMovementLock, true);
    }

    public void LockRotationFalse() 
    {
        playerController.SetAnimationLock(playerController.inMovementLock, false);
    }

    public void LockMovementTrue()
    {
        playerController.SetAnimationLock(true, playerController.inRotationLock);
    }

    public void LockMovementFalse()
    {
        playerController.SetAnimationLock(false, playerController.inRotationLock);
    }*/
}
