using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;


public static class DataVariables
{
    public static int playerID = 68000;
    public static DataVars data = new DataVars();
    public static string path = Application.persistentDataPath + "/who-are-you" + ".obel";
    public static string deathPath = Application.persistentDataPath + "/" + playerID + ".mirtys";

    static WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();
    static DateTime runningTime = new DateTime();


    /// <summary>
    /// Specifically for use with appending Death images,  - OR -  for use with Save State images
    /// </summary>
    /// <param name="death"></param>
    /// <returns></returns>
    public static IEnumerator TakeScreenshot(bool death, int saveNum)              // possible memory leak
    {
        yield return frameEnd;

        RenderTexture temp = Camera.main.activeTexture;
        RenderTexture main = RenderTexture.active;
        RenderTexture.active = temp;

        Texture2D tex = new Texture2D(temp.width, temp.height, TextureFormat.RGBA64, false);
        tex.filterMode = FilterMode.Point;

        tex.ReadPixels(new Rect(0, 0, temp.width, temp.height), 0, 0);

        Color[] pixels = tex.GetPixels();
        for (int p = 0; p < pixels.Length; p++) // gamma bullshit
        {
            pixels[p] = pixels[p].gamma;
        }
        tex.SetPixels(pixels);

        tex.Apply();

        byte[] bytes = tex.EncodeToJPG();
        string base64 = Convert.ToBase64String(bytes);

        if (death)
        {
            File.AppendAllText(Application.persistentDataPath + "/" + playerID + ".mirtys", base64 + "\n");
            Debug.Log("Data written to: " + Application.persistentDataPath);
        }
        else
        {
            data.SaveInfo.SaveImage = base64;
            Save(saveNum); // arbitrary number for now
        }

        RenderTexture.active = main;
    }

    public static Texture2D GrabImage(int num)
    {
        string[] lines = File.ReadAllLines(deathPath);

        return ParseImage(lines[num]);
    }

    public static Texture2D ParseImage(string base64)
    {
        byte[] bytes = Convert.FromBase64String(base64);

        Texture2D newImg = new Texture2D(0, 0);
        if (ImageConversion.LoadImage(newImg, bytes))
            return newImg;
        else
            return null;
    }

    public static void Save(int saveNum)
    {
        data.SaveInfo.Playtime += (float) DateTime.Now.Subtract(runningTime).TotalSeconds;

        File.WriteAllText(path + "" + saveNum, MakeJson(true));
        Debug.Log("Data written to: " + Application.persistentDataPath);

        runningTime = DateTime.Now;
    }

    public static void Reset()
    {
        data = new DataVars();
        playerID = data.SaveInfo.PlayerID;

        runningTime = DateTime.Now;
    }

    public static void Load(int saveNum)
    { 
        DataVars newData = JsonConvert.DeserializeObject<DataVars>(File.ReadAllText(path + "" + saveNum));
        data = newData;
        playerID = data.SaveInfo.PlayerID;

        runningTime = DateTime.Now;
    }

    public static SaveInformation LoadInfo(int saveNum)
    {
        SaveInformation possibleInfo = null;
        try
        {
            possibleInfo = JsonConvert.DeserializeObject<DataVars>(File.ReadAllText(path + "" + saveNum)).SaveInfo;
        }
        catch (Exception e)
        {
            //Debug.LogError(e);
            return null;
        }

        return possibleInfo;
    }

    public static string MakeJson(bool format)
    {
        string json = JsonConvert.SerializeObject(DataVariables.data, format ? Formatting.Indented : Formatting.None);

        return json;
    }
}

public class DataVars
{
    public SaveInformation SaveInfo = new SaveInformation();
    public int LastScene = 1;
    public PlayerVariables PlayerVars = new PlayerVariables(true);
    public Dictionary<int, SerializedSceneState> SceneStates = new Dictionary<int, SerializedSceneState>();
    public Dictionary<int, bool> DoorStates = new Dictionary<int, bool>();
    public NPC_States NPCs = new NPC_States();
}


public class SaveInformation
{
    public string SaveImage = null;
    public int PlayerID = (int)(UnityEngine.Random.value * 99999);
    public DateTime LastSave = DateTime.Now;
    public float Playtime = 0f;
    public bool EndingA = false; // main
    public bool EndingB = false; // bad
    public bool EndingC = false; // nauya
    public bool EndingD = false; // true
}


public class NPC_States
{
    public int Nauya = 0;
    public int Researcher = 0;
}

public struct PlayerVariables
{
    public int health;
    public Dictionary<int, int> inventory;
    public CurrentWeapon currWeap;
    public bool flashlightOn;
    public float[] rotPos;

    public PlayerVariables(bool def)
    {
        health = 100;   // make sure this is the same as max health (in the PlayerInformation script)

        inventory = new Dictionary<int, int>();
        inventory.Add(2001, 1); // knife
        inventory.Add(2002, 10); // gun
        inventory.Add(2003, 30); // ammo
        inventory.Add(2999, 1); // flashlight

        currWeap = CurrentWeapon.None;
        flashlightOn = false;

        rotPos = new float[4];
        rotPos[0] = 0f;
        rotPos[1] = 1f;
        rotPos[2] = 1f;
    }
}