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
            //int i = 0;
            foreach (var item in sceneState.sceneItems)
            {
                itemStates[i] = item;

                foreach (var it in sceneItems)
                {
                    if (itemStates[i].itemId == it.id)
                    {
                        it.item.state = itemStates[i];
                        break;
                    }
                }

                //i++;
            }
            //sceneItems = sceneState.sceneItems;
        }
        else 
        {
            int i = 0;
            foreach (var item in sceneItems)
            {
                item.item.state.itemId = item.id;
                itemStates[i] = item.item.state;
                i++;
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
