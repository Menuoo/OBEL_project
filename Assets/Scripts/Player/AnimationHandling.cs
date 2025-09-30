using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandling : MonoBehaviour
{
    [SerializeField] PlayerAnimationLogic playerAnim;

    public void ResetSlice()
    {
        playerAnim.ResetSlice();
    }

    public void ResetAdditionalAction()
    {
        playerAnim.ResetAdd();

        playerAnim.ResetSlice();
    }

    public void AddAvailable()
    {
        playerAnim.SetAddAvailability(true);
    }

    public void AddUnavailable()
    {
        playerAnim.SetAddAvailability(false);
    }
}
