using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneItemSave : MonoBehaviour
{ 
    public SceneItemState state = new SceneItemState();
}

public class SceneItemState
{
    public int itemId = -1;
    public bool isActive = true;
    public float[] posRot = { 0, 0, 0, 0};
}