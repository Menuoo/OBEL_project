using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;


public static class DataVariables
{
    public static DataVars data = new DataVars();

    public static string path = Application.persistentDataPath + "/who-are-you" + ".obel";

    public static void Save()
    {
        File.WriteAllText(path, MakeJson(true));
        Debug.Log("Data written to: " + Application.persistentDataPath);

        MakeImage();
    }

    public static void Reset()
    {
        data = new DataVars();
    }

    public static void Load()
    { 
        DataVars newData = JsonConvert.DeserializeObject<DataVars>(File.ReadAllText(path));
        data = newData;
    }

    public static string MakeJson(bool format)
    {
        string json = JsonConvert.SerializeObject(DataVariables.data, format ? Formatting.Indented : Formatting.None);

        return json;
    }

    public static void MakeImage()
    {
        string json = MakeJson(false);
        var bytes = System.Text.Encoding.UTF8.GetBytes(json);
        int imgSize = (int)Mathf.Sqrt(bytes.Length + 1);

        //imgSize = 1000;

        var header = new byte[54]
        {
            //Antraštė
            0x42, 0x4d,
            0x0, 0x0, 0x0, 0x0, //0x3e, 0xf4, 0x1, 0x0,
            0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0,
            //Antraštės informacija
            0x28, 0x0, 0x0, 0x0,
            0xe8, 0x3, 0x0, 0x0, // width
            0xe8, 0x3, 0x0, 0x0, // height
            0x1, 0x0,
            0x18, 0x0,            // colour depth
            0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0,
        };

        //Pataisome paveikslėlio plotį į imgSize
        Array.Copy(BitConverter.GetBytes((int)imgSize), 0, header, 0x12, sizeof(int));
        //Pataisome paveikslėlio aukštį į imgSize
        Array.Copy(BitConverter.GetBytes((int)imgSize), 0, header, 0x16, sizeof(int));

        using (FileStream file = new FileStream(Application.persistentDataPath + "/image.bmp", FileMode.Create, FileAccess.Write))
        {
            file.Write(header);

            var t = new byte[imgSize * imgSize * 3];

            int i = 1;
            foreach (var baitas in bytes)
            {
                t[t.Length - i++] = baitas;
            }

            file.Write(t);
            file.Close();
        }
    }
}

public class DataVars
{
    public int LastScene = 1;
    public PlayerVariables PlayerVars = new PlayerVariables(true);
    public Dictionary<int, SerializedSceneState> SceneStates = new Dictionary<int, SerializedSceneState>();
    public Dictionary<int, bool> DoorStates = new Dictionary<int, bool>();
    public NPC_States NPCs = new NPC_States();
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
        inventory.Add(2002, 15); // gun
        inventory.Add(2003, 30); // ammo

        currWeap = CurrentWeapon.None;
        flashlightOn = false;

        rotPos = new float[4];
        rotPos[0] = 0f;
        rotPos[1] = 1f;
        rotPos[2] = 1f;
    }
}