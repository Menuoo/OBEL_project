using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;
using System;

public class SceneSaveState : MonoBehaviour
{
    [SerializeField] SceneObjectSO thisScene;
    [SerializeField] ItemInScene[] sceneItems;

    public Dictionary<int, SceneItemState> itemStates;

    private void Awake()
    {
        itemStates = new Dictionary<int, SceneItemState>();

        if (DataVariables.data.SceneStates.TryGetValue(thisScene.GetId(), out SerializedSceneState sceneState))
        {
            itemStates = sceneState.sceneItems;

            foreach (var it in sceneItems)
            {
                it.item.state = itemStates[it.id];
            }
        }

        else 
        {
            foreach (var item in sceneItems)
            {
                item.item.state.itemId = item.id;
                itemStates.Add(item.id, item.item.state);
            }
            DataVariables.data.SceneStates.Add(thisScene.GetId(), new SerializedSceneState { sceneItems = this.itemStates});
        }
    }
}

[Serializable]
public struct ItemInScene
{
    public int id;
    public SceneItemSave item;
}

public struct SerializedSceneState
{
    public Dictionary<int, SceneItemState> sceneItems;
}
