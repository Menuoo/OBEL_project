using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartScene(bool toLoad)
    {
        if (toLoad)
        {
            DataVariables.Load();
        }
        else
        {
            DataVariables.Reset();
        }

        SceneManager.LoadScene(DataVariables.data.LastScene);
        SceneManager.LoadScene("PLAYER SCENE", LoadSceneMode.Additive);


        //PlayerInput playerInput = EnemyManager.instance.GetPlayer().GetInput();
        //SceneControl.instance.TransitionScene(1, 0, playerInput);
    }
}
