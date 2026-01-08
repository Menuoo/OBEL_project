using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSingle : MonoBehaviour
{
    public static CameraSingle instance { get; private set; }

    [SerializeField] CinemachineVirtualCamera virtCam;
    [SerializeField] Transform playerCam;
    [SerializeField] LayerMask specialLayers;
    LayerMask initialLayers;

    public bool specialCamera = false;
    //public Transform specialTrans = null;

    public Transform transHolder;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
        {
            initialLayers = Camera.main.cullingMask;
            instance = this;
        }
    }

    public void SetPos(Vector3 newPos)
    {
        if (!specialCamera)
        {
            transform.position = newPos;
        }
    }

    public void SetRot(Quaternion newRot)
    {
        if (!specialCamera)
        {
            transform.rotation = newRot;
        }
    }

    public void SpecialCamera(bool state, Transform newTrans)
    {
        specialCamera = state;
        if (state)
        {
            transform.position = newTrans.position;
            transform.rotation = newTrans.rotation;
        }

        SetLookAt(false, specialCamera ? null : playerCam);
        Camera.main.cullingMask = specialCamera ? specialLayers : initialLayers;
    }

    public void SetLookAt(bool playerMode, Transform newLookAt)
    {
        virtCam.LookAt = playerMode ? playerCam : newLookAt;
        virtCam.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
