using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CounterFPS : MonoBehaviour
{
    TMP_Text text;
    float time = 0;
    int counter = 0;
    bool works = true;

    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPeriod))
        {
            works = !works;
        }

        counter++;
        time += Time.unscaledDeltaTime;

        if (counter == 100)
        {
            float frameRate = counter / time;
            text.text = works ? frameRate.ToString("#.00") : "";

            counter = 0;
            time = 0;
        }
    }
}
