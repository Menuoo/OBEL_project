using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nauya : MonoBehaviour
{
    [SerializeField] GameObject arm;
    [SerializeField] GameObject bandage;
    [SerializeField] GameObject notBandage;

    [SerializeField] Material bodyMat;

    int currentState = 0;

    private void Start()
    {
        currentState = DataVariables.data.NPCs.Nauya;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            currentState += 1;
        }

        if (currentState <= 0) // no arm, bandageless
        { 
            // default state
        }

        if (currentState == 1) // bandage on
        { 
            bandage.SetActive(true);
        }

        if (currentState == 2) // arm back, researcher MUST be dead for this to trigger
        {
            bandage.SetActive(false);
            notBandage.SetActive(false);
            arm.SetActive(true);
        }

        if (currentState >= 3) // final choice
        {
            bandage.SetActive(false);
            notBandage.SetActive(false);
            arm.SetActive(true);
        }
    }
}
