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
        float x = 0.3f;
        for (float i = 0; i < x; i += Time.unscaledDeltaTime)
        {
            this.transform.position = orig + transform.up * i * 0.05f;

            yield return null;
        }

        for (float i = x; i < 0f; i -= Time.unscaledDeltaTime)
        {
            this.transform.position = orig + transform.up * i * -0.05f;

            yield return null;
        }

        this.transform.position = orig;
    }
}
