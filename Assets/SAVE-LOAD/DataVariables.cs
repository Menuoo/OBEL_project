using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class DataVariables
{
    static public PlayerVariables playerVars = new PlayerVariables(true);
    static public Dictionary<int, SceneSaveState> sceneStates = new Dictionary<int, SceneSaveState>();
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
