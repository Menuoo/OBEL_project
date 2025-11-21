using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SceneObject")]
public class SceneObjectSO : ScriptableObject
{
    [SerializeField] Vector3[] doorLocations;
    [SerializeField] float[] doorRotations;

    public Vector4 GetDoor(int id)
    {
        if (id >= doorLocations.Length)
            return Vector4.zero;

        return new Vector4(doorLocations[id].x, doorLocations[id].y, doorLocations[id].z, doorRotations[id]);
    }
}