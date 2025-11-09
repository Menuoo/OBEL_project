using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class CameraEffects : MonoBehaviour
{
    public static CameraEffects instance { get; private set; }

    [SerializeField] CinemachineImpulseSource src;
    [SerializeField] GameObject postObj;

    PostProcessVolume volume;
    ChromaticAberration aberration;
    float aberrationPower = 0f;
    float aberrationSpeed = 4f;

    //[SerializeField] PostProcessProfile profile;

    private void Start()
    {
        volume = postObj.GetComponent<PostProcessVolume>();
        //volume.profile.TryGetSettings(out aberration);
        volume.profile.TryGetSettings(out aberration);
    }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Slash))
        {
            src.GenerateImpulseWithForce(2f);
            //StartCoroutine(Shake(0.1f, 0.05f)); 
        }*/

        if (aberrationPower > 0f)
        {
            aberrationPower = Mathf.Max(aberrationPower - (Time.deltaTime * aberrationSpeed), 0f);
            //aberration.enabled.value = true;
            aberration.intensity.value = aberrationPower;
        }
    }

    public void Shake(float force)
    { 
        src.GenerateImpulse(force);
    }

    public void Aberrate(float power)
    {
        aberrationPower = power;

        //aberration.enabled.value = true;
        aberration.intensity.value = aberrationPower;
    }
}
