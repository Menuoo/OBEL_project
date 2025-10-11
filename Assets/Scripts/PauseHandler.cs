using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler
{
    static public bool pauseState = false;

    static public void PauseTime()
    {
        pauseState = true;
        Time.timeScale = 0;
    }

    static public void ResumeTime()
    {
        pauseState = false;
        Time.timeScale = 1;
    }

    static public void FlipTime()
    {
        if (pauseState)
            ResumeTime();
        else 
            PauseTime();
    }
}
