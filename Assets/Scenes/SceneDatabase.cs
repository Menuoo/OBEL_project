using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SceneDatabase")]
public class SceneDatabase : ScriptableObject
{
    [SerializeField] SceneObject[] scenes;

    public SceneObject GetScene(int id)
    {
        if (id >= scenes.Length)
            return null;

        return scenes[id];
    }
}