using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextObject : MonoBehaviour, IInteractable
{
    [SerializeField] string[] displayText;

    public void OnInteract(PlayerInteractions interactions)
    {
        interactions.DisplayMessage(displayText);
    }
}

public interface IInteractable
{
    public void OnInteract(PlayerInteractions interactions);
}
