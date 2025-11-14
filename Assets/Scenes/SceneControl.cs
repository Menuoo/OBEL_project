using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    [SerializeField] PlayerInput input;

    public static SceneControl instance { get; private set; }

    int nextId = 0;

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
    public void TransitionScene(int id, PlayerInput newInput)
    {
        nextId = id;
        TransitionLogic.instance.TransitionScene(true);
        TransitionLogic.TransitionEvent += ChangeScene;

        input = newInput; 
        input.SwapControls(-1);
        PauseHandler.PauseTime();
    }
}
