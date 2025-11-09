using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance { get; private set; }

    [SerializeField] GameObject[] particles;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
        {
            instance = this;
        }
    }

    public GameObject GetParticles(int id)
    {
        if (id < particles.Length)
            return particles[id];
        else return null;
    }
}
