using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextObject : IInteractable
{
    [SerializeField] string[] displayText;

    public override void OnInteract(PlayerInteractions interactions)
    {
        interactions.DisplayMessage(displayText);
    }
}
