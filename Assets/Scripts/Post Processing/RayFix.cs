using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RayFix : GraphicRaycaster
{
    // all the stuff you need for remapping mouse position
    //public RectTransform targetSurface;
    //public Camera targetCamera;
    //public Canvas canvas;

    public GraphicRaycaster gRay;

    protected override void Awake()
    {
        gRay.enabled = false;
    }

    protected override void Start()
    {
        gRay.enabled = true;
    }


    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
        var pos = eventData.position; // save the old mouse position position
        eventData.position = RemapPosition(pos); // this is the part you need to implement
        //eventData.position = pos; // restore old pointer event position so that rest of raycasters don't break

        //Debug.Log(eventData.position);
    }

    public Vector2 RemapPosition(Vector2 oldPos)
    {
        float ratio = 640f / Screen.width;
        return oldPos * ratio;
    }
}
