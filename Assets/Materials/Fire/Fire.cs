using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] Light fireLight;
    [SerializeField] float period = 1f;
    [SerializeField] float shiftSpeed = 1f;
    [SerializeField] float initIntensity = 1f;
    float targetIntensity = 1f;
    bool change = true;

    void Start()
    {
        //initIntensity = fireLight.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (change)
        {
            Invoke("NewVal", 0.5f);
        }
        fireLight.intensity = Mathf.Lerp(fireLight.intensity, targetIntensity, Time.deltaTime * shiftSpeed);
    }

    public void NewVal()
    { 
        targetIntensity = initIntensity + initIntensity * (0.2f * Random.value) ;
    }
}
