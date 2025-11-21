using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public static class DataVariables
{
    public static DataVars data = new DataVars();
}

public class DataVars
{
    public PlayerVariables PlayerVars = new PlayerVariables(true);
    public Dictionary<int, SerializedSceneState> SceneStates = new Dictionary<int, SerializedSceneState>();
    public Dictionary<int, bool> DoorStates = new Dictionary<int, bool>();
}



public struct PlayerVariables
{
    public int health;
    public Dictionary<int, int> inventory;
    public CurrentWeapon currWeap;
    public bool flashlightOn; 

    public PlayerVariables(bool def)
    {
        health = 100;
        inventory = new Dictionary<int, int>();
        currWeap = CurrentWeapon.None;
        flashlightOn = false;
    }
}
