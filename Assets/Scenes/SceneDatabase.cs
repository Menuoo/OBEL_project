using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SceneDatabase")]
public class SceneDatabase : ScriptableObject
{
    //[SerializeField] SceneDatabaseEntry[] scenes;
    [SerializeField] SceneObjectSO[] scenes;

    public SceneObjectSO GetScene(int id)
    {
        foreach (var scene in scenes)
        { 
            if (scene.GetId() == id)
                return scene;
        }

        return null;
    }
}

[Serializable]
public struct SceneDatabaseEntry
{
    public int sceneId;
    public SceneObjectSO sceneObject;
}