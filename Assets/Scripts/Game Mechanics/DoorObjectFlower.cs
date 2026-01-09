using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorObjectFlower : IInteractable
{
    [SerializeField] Transform playerLoc;
    [SerializeField] int targetScene;
    [SerializeField] int targetDoor;

    [SerializeField] int conditionItem = -1; // if -1, then no condition, the door just works

    [SerializeField] string[] lockedText;
    [SerializeField] string[] unlockText;

    [SerializeField] DoorFlower flower1;
    [SerializeField] DoorFlower flower2;
    [SerializeField] Material wither;
    float health = 2;

    int state = 0;

    private void Start()
    {
        if (DataVariables.data.DoorStates.TryGetValue(conditionItem, out bool val))
        {
            flower1.Activate(-1);
            flower1.Wither(wither);
            flower2.Wither(wither);

            state = val ? 1 : 0;

            if (DataVariables.data.DoorStates.TryGetValue(conditionItem + 1, out bool val2))
            { 
                state = val2 ? 2 : 0;
                if (val2)
                {
                    flower1.gameObject.SetActive(false);
                    flower2.gameObject.SetActive(false);
                }
            }
        }
    }

    public override void OnInteract(PlayerInteractions interactions)
    {
        if (targetScene <= 0)
        {
            return;
        }

        // add a unlock check which sets state to 1
        if (state == 0 && conditionItem != -1)
        {
            state = interactions.CheckKey(conditionItem) ? 1 : 0;         // checks for chemicals

            flower1.Activate(-1);
            flower1.Wither(wither);
            flower2.Wither(wither);
        }

        if (!flower1.gameObject.active && !flower2.gameObject.active)
        {
            state = 2;
            DataVariables.data.DoorStates.TryAdd(conditionItem + 1, true);
        }

        if (state == 2 || conditionItem == -1)
        {
            //Debug.Log("scene changed");
            SceneControl.instance.TransitionScene(targetScene, targetDoor, interactions.GetInput());
        }
        else if (state == 0)
        {
            interactions.DisplayMessage(lockedText);
        }
        else if (state == 1)
        {
            interactions.DisplayMessage(unlockText);
            state = 1;
        }
    }

    /*public Vector4 GetVec4()
    { 
        Vector4 vec = playerLoc.TransformPoint(playerLoc.localPosition);
        vec.w = playerLoc.rotation.eulerAngles.y;

        Debug.Log(vec);

        return vec;
    }*/
}
