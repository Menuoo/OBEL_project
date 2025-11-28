using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadFollow : MonoBehaviour
{
    [SerializeField] GameObject headBone;

    [SerializeField] bool lockX = false;
    [SerializeField] bool lockY = false;
    [SerializeField] bool lockZ = false;

    PlayerController player;
    Vector3 eulerRot;

    private void Start()
    {
        player = EnemyManager.instance.GetPlayer();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        eulerRot = headBone.transform.rotation.eulerAngles;
        headBone.transform.LookAt(player.transform);

        eulerRot = new Vector3(lockX ? eulerRot.x : headBone.transform.rotation.eulerAngles.x,
                               lockY ? eulerRot.y : headBone.transform.rotation.eulerAngles.y,
                               lockZ ? eulerRot.z : headBone.transform.rotation.eulerAngles.z);

        headBone.transform.rotation = Quaternion.Euler(eulerRot);
    }
}
