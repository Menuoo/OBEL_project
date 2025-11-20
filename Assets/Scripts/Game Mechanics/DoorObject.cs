using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObject : IInteractable
{
    [SerializeField] int targetScene;
    [SerializeField] int targetDoor;

    [SerializeField] int conditionItem = -1; // if -1, then no condition, the door just works

    [SerializeField] string[] lockedText;
    [SerializeField] string[] unlockText;

    int state = 0;

    public override void OnInteract(PlayerInteractions interactions)
    {
        // add a unlock check which sets state to 1
        if (state == 0 && conditionItem != -1)
        {
            state = interactions.CheckKey(conditionItem) ? 1 : 0;
        }

        if (state == 2 || conditionItem == -1)
        {
            Debug.Log("scene changed");
            SceneControl.instance.TransitionScene(targetScene, targetDoor, interactions.GetInput());
        }
        else if (state == 0)
        {
            interactions.DisplayMessage(lockedText);
        }
        else if (state == 1)
        {
            interactions.DisplayMessage(unlockText);
            state = 2;
        }
    }
}
