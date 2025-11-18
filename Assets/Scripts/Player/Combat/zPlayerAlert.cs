using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zPlayerAlert : MonoBehaviour
{
    [SerializeField] SphereCollider collisider;
    [SerializeField] PlayerController player;

    float initRad = 1f;

    private void Start()
    {
        initRad = collisider.radius;
    }

    void Update()
    {
        collisider.radius = initRad * player.GetSpeed();
    }
}
