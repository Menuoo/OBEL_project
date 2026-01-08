using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField] Light lamp;
    [SerializeField] GameObject emiss;
    [SerializeField] float onStrength = 0f;
    [SerializeField] float offStrength = 1f;

    bool changing = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (onStrength != 0 && !changing)
        {
            if (lamp.enabled)
            {
                StartCoroutine(ChangeLamp(Random.value * onStrength, false));
                changing = true;
            }
            else
            {
                StartCoroutine(ChangeLamp(Random.value * offStrength, true));
                changing = true;
            }
        }
    }

    public IEnumerator ChangeLamp(float timeF, bool state)
    { 
        yield return new WaitForSeconds(timeF);

        lamp.enabled = state;
        emiss.SetActive(state);
        changing = false;
    }
}
