using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadFollow : MonoBehaviour
{
    [SerializeField] GameObject headBone;
    PlayerController player;

    private void Start()
    {
        player = EnemyManager.instance.GetPlayer();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        headBone.transform.LookAt(player.transform);
    }
}
