using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraRails : MonoBehaviour
{
    [SerializeField] CameraPosition[] positions;
    public int currentPos = 0;


    PlayerController player;

    private void Start()
    {
        player = EnemyManager.instance.GetPlayer();
    }

    private void Update()
    {
        //CheckPositions();
        Rails();
    }


    void Rails()
    {
        int firstPos = -1;
        Vector3 distVec = positions[0].trigger.position - player.transform.position;
        float dist = distVec.magnitude;

        foreach (var position in positions)
        {
            firstPos++;
            if (dist < position.distThreshold)
                break;
        }

        Debug.Log(dist + " " + firstPos);

        if (firstPos == 0)
            CameraSingle.instance.SetPos(positions[firstPos].transform.position);
        else
        {
            float distDiff = positions[firstPos].distThreshold - positions[firstPos - 1].distThreshold;
            dist = dist - positions[firstPos - 1].distThreshold;

            Vector3 newPos = Vector3.Lerp(positions[firstPos - 1].transform.position, positions[firstPos].transform.position, dist / distDiff);

            CameraSingle.instance.SetPos(newPos);
        }
    }


    void CheckPositions()
    {
        int i = 0;
        int thisTheOne = 0;
        foreach (var position in positions)
        {
            Vector3 distVec = position.trigger.position - player.transform.position;
            float dist = distVec.magnitude;

            if (position.moreThan)
            {
                if (dist > position.distThreshold)
                {
                    thisTheOne = i;
                }
            }
            else if (dist < position.distThreshold)
            {
                thisTheOne = i;
            }

            i++;
        }

        currentPos = thisTheOne;
        CameraSingle.instance.SetPos(positions[currentPos].transform.position);
    }


}

[Serializable]
public class CameraPosition
{
    public Transform transform;
    //public CamTransType transType;

    public Transform trigger;
    public float distThreshold;
    public bool moreThan = false;
}

[Serializable]
public enum CamTransType { Instant, Rail }

[Serializable]
public enum CamTrigType { Distance, Coordinate } // if distance, it will look at x