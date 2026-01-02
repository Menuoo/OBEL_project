using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObjectRoot : IInteractable
{
    [SerializeField] Transform playerLoc;
    [SerializeField] int targetScene;
    [SerializeField] int targetDoor;

    [SerializeField] int conditionItem = -1; // if -1, then no condition, the door just works

    [SerializeField] string[] lockedText;
    [SerializeField] string[] unlockText;

    [SerializeField] GameObject fires;
    [SerializeField] GameObject roots1;
    [SerializeField] GameObject roots2;

    int state = 0;

    private void Start()
    {
        if (DataVariables.data.DoorStates.TryGetValue(conditionItem, out bool val))
        {
            state = val ? 2 : 0;
            if (val)
            {
                Branches();
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
            state = interactions.CheckKey(conditionItem) ? 1 : 0;         // checks for gasoline
            if (state == 1)
                state = interactions.CheckKey(conditionItem - 1) ? 1 : 0; // checks for matches
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
            state = 2;

            Fire();
        }
    }

    void Fire() 
    {
        if (!fires.gameObject.activeSelf)
        {
            fires.gameObject.SetActive(true);
            Invoke("Fire", 5f);
            Invoke("Branches", 2.5f);
        }
        else 
        {
            fires.gameObject.SetActive(false);
        }
    }

    void Branches()
    {
        roots1.transform.Rotate(new Vector3 (4f, 0, 0f));
        roots2.transform.Rotate(new Vector3 (4f, 0, 0f));
    }

    public Vector4 GetVec4()
    { 
        Vector4 vec = playerLoc.TransformPoint(playerLoc.localPosition);
        vec.w = playerLoc.rotation.eulerAngles.y;

        Debug.Log(vec);

        return vec;
    }
}
