using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    PlayerInput input;
    [SerializeField] SceneDatabase scenes;

    public static event Action<bool> SceneChangeEvent;

    public static SceneControl instance { get; private set; }

    int nextId = 0;
    int nextDoor = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
        {
            instance = this;
        }
    }


    public void ChangeScene(bool version)
    {
        if (version)
        {
            SceneManager.LoadScene(nextId);
            DataVariables.data.LastScene = nextId;

            Vector4 vec = scenes.GetScene(nextId).GetDoor(nextDoor);

            input.GetController().SetPosition(vec);
            input.GetController().SetRotation(vec.w);

            SceneChangeEvent(true);

            TransitionLogic.instance.TransitionScene(false);
        }
        else
        {
            TransitionLogic.TransitionEvent -= ChangeScene;

            input.SwapControls(1);
            PauseHandler.ResumeTime();
        }
    }

    // could maybe do an invoke for input?,    ---   ALSO: setup is unideal, cause it depends on another script to finish, may be prone to softlocks
    public void TransitionScene(int id, int door, PlayerInput newInput)
    {
        nextId = id;
        nextDoor = door;
        TransitionLogic.instance.TransitionScene(true);
        TransitionLogic.TransitionEvent += ChangeScene;

        input = newInput; 
        input.SwapControls(-1);
        PauseHandler.PauseTime();
    }
}
