using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOR_SCREENSHOTS : MonoBehaviour
{
    [SerializeField] Material posterizer;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            posterizer.SetFloat("_ColourAmount", posterizer.GetFloat("_ColourAmount") + 1f);
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            posterizer.SetFloat("_ColourAmount", posterizer.GetFloat("_ColourAmount") - 1f);
        }
    }
}
