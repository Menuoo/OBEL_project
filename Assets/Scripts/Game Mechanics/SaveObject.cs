using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject : IInteractable
{
    public override void OnInteract(PlayerInteractions interactions)
    {
        interactions.Save();
    }
}
