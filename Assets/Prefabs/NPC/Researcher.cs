using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Researcher : MonoBehaviour
{
    [SerializeField] Material bodyMat;
    [SerializeField] Animator animator;


    static int deadHash = Animator.StringToHash("dead");
    int currentState = 0;

    private void Start()
    {
        currentState = DataVariables.data.NPCs.Researcher;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            currentState += 1;
        }

        if (currentState <= 0) // initial meeting
        {
        }

        if (currentState == 1) // requests player's help
        {
        }

        if (currentState == 2) // receives player's help -- MISSABLE
        {
        }

        if (currentState >= 3) // dead
        {
            animator.SetBool(deadHash, true);
            GetComponent<HeadFollow>().enabled = false;
        }
    }
}
