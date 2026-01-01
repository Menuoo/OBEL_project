using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuLogic : MonoBehaviour
{
    // general objects
    [Header("General")]
    [SerializeField] GameObject loadMenu;
    [SerializeField] GameObject settingsMenu;


    // load menu objects
    [Header("Load Menu")]
    [SerializeField] Image saveImage;
    [SerializeField] Button save1;
    [SerializeField] Button save2;
    [SerializeField] Button save3;
    [SerializeField] TMP_Text playtime1;
    [SerializeField] TMP_Text playtime2;
    [SerializeField] TMP_Text playtime3;
    [SerializeField] TMP_Text lastSaved;
    [SerializeField] Button delete;
    [SerializeField] Button loadButton;
    bool isLoaded = false;
    SaveInformation saveInfo1;
    SaveInformation saveInfo2;
    SaveInformation saveInfo3;
    int selection = 0;

    void Start()
    {
        
    }

    public void StartNew()
    {
        /*if (toLoad)
        {
            DataVariables.Load();
        }
        else
        {
            DataVariables.Reset();
        }*/

        DataVariables.Reset();

        SceneManager.LoadScene(DataVariables.data.LastScene);
        SceneManager.LoadScene("PLAYER SCENE", LoadSceneMode.Additive);

        //PlayerInput playerInput = EnemyManager.instance.GetPlayer().GetInput();
        //SceneControl.instance.TransitionScene(1, 0, playerInput);
    }

    public void LoadMenu(bool state) // state: if true, then open menu, otherwise - close it
    { 
        loadMenu.SetActive(state);

        if (state)
        {
            LoadStateButton(0);

            if (!isLoaded)
            {
                saveInfo1 = DataVariables.LoadInfo(1);
                saveInfo1 = DataVariables.LoadInfo(2);
                saveInfo1 = DataVariables.LoadInfo(3);
            }

            playtime1.text = saveInfo1 == null ? "-:--:--" : TimeSpan.FromSeconds(saveInfo1.Playtime).ToString();
            playtime2.text = saveInfo2 == null ? "-:--:--" : TimeSpan.FromSeconds(saveInfo2.Playtime).ToString();
            playtime3.text = saveInfo3 == null ? "-:--:--" : TimeSpan.FromSeconds(saveInfo3.Playtime).ToString();
        }
    }

    public void LoadStateButton(int saveNum)
    {
        saveImage.sprite = null;
        saveImage.color = new Color(0, 0, 0, 0);
        selection = 0;
        lastSaved.gameObject.SetActive(false);
        loadButton.interactable = false;
        delete.interactable = false;
        save1.interactable = true;
        save2.interactable = true;
        save3.interactable = true;

        switch (saveNum)
        {
            case 1: 
                selection = 1; save1.interactable = false;
                if (saveInfo1 != null) 
                {   
                    saveImage.sprite = Sprite.Create(DataVariables.ParseImage(saveInfo1.SaveImage), new Rect(0, 0, 360, 360), new Vector2(140, 0), .01f);
                    saveImage.color = new Color(1, 1, 1, 1);

                    delete.interactable = true; loadButton.interactable = true; 
                    lastSaved.gameObject.SetActive(false); lastSaved.text = saveInfo1.LastSave.ToShortDateString();
                } 
                break;
            case 2:
                selection = 2; save2.interactable = false;
                if (saveInfo1 != null)
                {
                    saveImage.sprite = Sprite.Create(DataVariables.ParseImage(saveInfo2.SaveImage), new Rect(0, 0, 360, 360), new Vector2(140, 0), .01f);
                    saveImage.color = new Color(1, 1, 1, 1);

                    delete.interactable = true; loadButton.interactable = true;
                    lastSaved.gameObject.SetActive(false); lastSaved.text = saveInfo2.LastSave.ToShortDateString();
                }
                break;
            case 3:
                selection = 3; save3.interactable = false;
                if (saveInfo3 != null)
                {
                    saveImage.sprite = Sprite.Create(DataVariables.ParseImage(saveInfo3.SaveImage), new Rect(0, 0, 360, 360), new Vector2(140, 0), .01f);
                    saveImage.color = new Color(1, 1, 1, 1);

                    delete.interactable = true; loadButton.interactable = true;
                    lastSaved.gameObject.SetActive(false); lastSaved.text = saveInfo3.LastSave.ToShortDateString();
                }
                break;
            default: break;
        }
    }

    public void LoadGameButton()
    {
        DataVariables.Load(selection);

        SceneManager.LoadScene(DataVariables.data.LastScene);
        SceneManager.LoadScene("PLAYER SCENE", LoadSceneMode.Additive);
    }


    public void DeleteSaveButton()
    { 
        // Delete save data
    }

    public void SettingsMenu(bool state)
    { 
        settingsMenu.SetActive(state);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
