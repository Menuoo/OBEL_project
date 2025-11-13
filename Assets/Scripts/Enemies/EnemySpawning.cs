using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    [SerializeField] EnemyToSpawn[] enemySpawns;

    void Start()
    {
        // pass reference to EnemyManager (maybe)
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SpawnAll();
        }
    }

    public void SpawnAll()
    {
        int i = 0;
        foreach (var enemy in enemySpawns)
        {
            SpawnOne(i);
            i++;
        }
    }

    public void SpawnOne(int i)
    {
        EnemyBase toSpawn = enemySpawns[i].enemyType;
        Transform toTrans = enemySpawns[i].location;
        Vector3 addDir = enemySpawns[i].dir;

        Instantiate(EffectManager.instance.GetParticles(1), toTrans);
        EnemyBase newGuy = Instantiate(toSpawn, toTrans.position + addDir, Quaternion.identity); // idk how to rotate rn

        // have to set up a way to actually slide enemies onto the scene here, idea's there tho
    }
}

[Serializable]
public class EnemyToSpawn
{
    public EnemyBase enemyType;
    public Transform location;
    public Vector3 dir;
}
