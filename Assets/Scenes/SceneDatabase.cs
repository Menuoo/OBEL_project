using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SceneDatabase")]
public class SceneDatabase : ScriptableObject
{
    [SerializeField] SceneDatabaseEntry[] scenes;

    public SceneObjectSO GetScene(int id)
    {
        foreach (var scene in scenes)
        { 
            if (scene.sceneId == id)
                return scene.sceneObject;
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