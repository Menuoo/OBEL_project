using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDoors : MonoBehaviour
{
    PlayerInput input;
    [SerializeField] DoorObject[] doors;

    public static SceneDoors instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
        {
            instance = this;
        }
    }

    public DoorObject GetDoor(int id)
    {
        return doors[id];
    }
}
