using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nauya : IInteractable
{
    [SerializeField] GameObject arm;
    [SerializeField] GameObject bandage;
    [SerializeField] GameObject notBandage;

    [SerializeField] Material bodyMat;
    [SerializeField] float existence = 0;

    int currentState = 0;
    int currentLine = 0;


    public override void OnInteract(PlayerInteractions interactions)
    {
        switch (currentState)
        {
            case 0: DoState0(interactions);  break;
            case 1: DoState1(interactions, false);  break;
            case 2: DoState1(interactions, true);  break;
            case 3: DoState3(interactions);  break;
            default: break;
        }
    }

    void DoState0(PlayerInteractions interactions)
    {
        string[] texts1 = { "hey.. i'm a little hurt, as you can see.", "would appreciate if you could help me.. out.. here.",
            "of course, only if you have the means.. i need something to treat this with.",
            "things are a little rough out here, for everyone.. i understand if you can't help me."};

        string[] texts2 = { "hey.. have you a bandage?" };

        if (currentLine == 0)
        {
            interactions.DisplayMessage(texts1);
            currentLine++;
        }
        else 
        {
            interactions.DisplayMessage(texts2);
        }
    }

    void DoState1(PlayerInteractions interactions, bool state2)
    {
        string[] texts1 = { "thanks.", "hey.. you look like you've been around. any idea where i could lay low?", "the last room i entered, well.",
        "i'm sure you're aware what happened.", "it'd be nice if there were a safe place. even better if it had any survivors."};

        string[] texts2 = { "hey.. you have a place in mind?" };

        string[] texts3 = { "'preciate it." };

        if (state2)
        {
            interactions.DisplayMessage(texts1);
        }
        else if (currentLine == 0)
        {
            interactions.DisplayMessage(texts1);
            currentLine++;
        }
        else
        {
            interactions.DisplayMessage(texts2);
        }
    }

    void DoState3(PlayerInteractions interactions)
    {
        string[] texts1 = { "hey.... they.. got me.. good..", "can i.. ask for another favour?..", "could you.. hold my hand?" };

        string[] texts2 = { "..will you?" };

        if (currentLine == 0)
        {
            interactions.DisplayMessage(texts1);
            currentLine++;
        }
        else
        {
            interactions.DisplayMessage(texts2);
        }
    }


    private void Start()
    {
        currentState = DataVariables.data.NPCs.Nauya;

        if (currentState == 0 || currentState == 1)
        {
            if (existence != 0)
            { 
                this.gameObject.SetActive(false);
            }
        }

        if (currentState == 2)
        {
            if (existence != 2)
            {
                this.gameObject.SetActive(false);
            }
        }

        if (currentState == 3)
        {
            if (existence != 3)
            {
                this.gameObject.SetActive(false);
            }
        }
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
