using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSingle : MonoBehaviour
{
    public static CameraSingle instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
        {
            instance = this;
        }
    }

    public void SetPos(Vector3 newPos)
    { 
        transform.position = newPos;
    }
}
