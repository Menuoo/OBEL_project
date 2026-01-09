using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;

public class KeyButton : MonoBehaviour
{
    Vector3 orig;

    private void Start()
    {
        orig = transform.position;
    }

    public IEnumerator HandlePress()
    {
        float x = 0.15f;
        for (float i = 0; i < x * 2f; i += Time.unscaledDeltaTime)
        {
            this.transform.position = i < x ? orig + transform.up * i * 0.05f : orig + transform.up * (2f * x - i) * 0.05f;
            Debug.Log("run1");

            yield return null;
        }

        SoundManager.instance.PlaySound(13);

        for (float i = x; i < 0f; i -= Time.unscaledDeltaTime)
        {
            this.transform.position = orig + transform.up * i * 0.05f;
            Debug.Log("run2");

            yield return null;
        }

        this.transform.position = orig;
    }
}
