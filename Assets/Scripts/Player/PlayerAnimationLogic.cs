using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationLogic : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    PlayerController playerController;
    PlayerInput playerInput;

    bool rightHandHold = false;


    private static int rightHandHoldHash = Animator.StringToHash("rightHandHold");


    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
