using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionLogic : MonoBehaviour
{
    public float transTime = 0.1f;
    [SerializeField] Image image;

    public static TransitionLogic instance { get; private set; }
    public static event Action<bool> TransitionEvent;  

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(Transition());
        }*/
    }

    public void TransitionScene(bool state)
    {
        if (state)
            StartCoroutine(TransitionOne());
        else
            StartCoroutine(TransitionZero());
    }

    public IEnumerator TransitionOneZero()
    {
        Color clr = image.color;
        for (float i = 0; i < transTime; i += Time.unscaledDeltaTime)
        {
            clr.a = Mathf.Min(i / transTime, 1f);
            image.color = clr;

            yield return null;
        }

        clr.a = 1f;
        image.color = clr;

        TransitionEvent?.Invoke(true);

        StartCoroutine(TransitionZero());
    }

    public IEnumerator LightsOut()
    {
        Color clr = image.color;
        for (float i = 0; i < transTime; i += Time.unscaledDeltaTime)
        {
            clr.a = Mathf.Min(i / transTime, 1f);
            image.color = clr;

            yield return null;
        }

        clr.a = 1f;
        image.color = clr;

        Application.Quit();
    }

    IEnumerator TransitionOne()
    {
        Color clr = image.color;
        for (float i = 0; i < transTime; i += Time.unscaledDeltaTime)
        {
            clr.a = Mathf.Min(i / transTime, 1f);
            image.color = clr;

            yield return null;
        }

        clr.a = 1f;
        image.color = clr;

        TransitionEvent?.Invoke(true);
    }

    IEnumerator TransitionZero()
    {
        Color clr = image.color;
        for (float i = 0; i < transTime; i += Time.unscaledDeltaTime)
        {
            clr.a = Mathf.Max(1f - i / transTime, 0f);
            image.color = clr;

            yield return null;
        }

        clr.a = 0f;
        image.color = clr;

        TransitionEvent?.Invoke(false);
    }

    public static float GetTime() => instance.transTime;
}
