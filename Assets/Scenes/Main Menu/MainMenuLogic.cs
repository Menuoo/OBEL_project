using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    SaveInformation saveInfo1 = null;
    SaveInformation saveInfo2 = null;
    SaveInformation saveInfo3 = null;
    int selection = 0;

    void Start()
    {
        saveInfo1 = null;
        saveInfo2 = null;
        saveInfo3 = null;
    }

    public void StartNew()
    {
        SoundManager.instance.PlaySound(12);
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
        SoundManager.instance.PlaySound(12);
        loadMenu.SetActive(state);

        if (state)
        {
            LoadStateButton(0);

            if (!isLoaded)
            {
                isLoaded = true;
                saveInfo1 = DataVariables.LoadInfo(1);
                saveInfo2 = DataVariables.LoadInfo(2);
                saveInfo3 = DataVariables.LoadInfo(3);
            }

            playtime1.text = saveInfo1 == null ? "-:--:--" : TimeSpan.FromSeconds(saveInfo1.Playtime).ToString(@"h\:mm\:ss");
            playtime2.text = saveInfo2 == null ? "-:--:--" : TimeSpan.FromSeconds(saveInfo2.Playtime).ToString(@"h\:mm\:ss");
            playtime3.text = saveInfo3 == null ? "-:--:--" : TimeSpan.FromSeconds(saveInfo3.Playtime).ToString(@"h\:mm\:ss");
        }
    }

    public void LoadStateButton(int saveNum)
    {
        SoundManager.instance.PlaySound(12);

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
                    Texture2D tex2D = DataVariables.ParseImage(saveInfo1.SaveImage);
                    tex2D.filterMode = FilterMode.Point;

                    saveImage.sprite = Sprite.Create(tex2D, new Rect(0, 0, 640, 360), new Vector2(0, 0), .01f);
                    saveImage.color = new Color(1, 1, 1, 1);

                    delete.interactable = true; loadButton.interactable = true; 
                    lastSaved.gameObject.SetActive(true); lastSaved.text = "last saved on: " + saveInfo1.LastSave.ToShortDateString();
                } 
                break;
            case 2:
                selection = 2; save2.interactable = false;
                if (saveInfo2 != null)
                {
                    Texture2D tex2D = DataVariables.ParseImage(saveInfo2.SaveImage);
                    tex2D.filterMode = FilterMode.Point;

                    saveImage.sprite = Sprite.Create(tex2D, new Rect(0, 0, 640, 360), new Vector2(0, 0), .01f);
                    saveImage.color = new Color(1, 1, 1, 1);

                    delete.interactable = true; loadButton.interactable = true;
                    lastSaved.gameObject.SetActive(true); lastSaved.text = "last saved on: " + saveInfo2.LastSave.ToShortDateString();
                }
                break;
            case 3:
                selection = 3; save3.interactable = false;
                if (saveInfo3 != null)
                {
                    Texture2D tex2D = DataVariables.ParseImage(saveInfo3.SaveImage);
                    tex2D.filterMode = FilterMode.Point;

                    saveImage.sprite = Sprite.Create(tex2D, new Rect(0, 0, 640, 360), new Vector2(0, 0), .01f);
                    saveImage.color = new Color(1, 1, 1, 1);

                    delete.interactable = true; loadButton.interactable = true;
                    lastSaved.gameObject.SetActive(true); lastSaved.text = "last saved on: " + saveInfo3.LastSave.ToShortDateString();
                }
                break;
            default: break;
        }
    }

    public void LoadGameButton()
    {
        SoundManager.instance.PlaySound(12);
        DataVariables.Load(selection);

        SceneManager.LoadScene(DataVariables.data.LastScene);
        SceneManager.LoadScene("PLAYER SCENE", LoadSceneMode.Additive);
    }


    public void DeleteSaveButton()
    {
        SoundManager.instance.PlaySound(12);
        // Delete save data
    }

    public void SettingsMenu(bool state)
    {
        SoundManager.instance.PlaySound(12);
        settingsMenu.SetActive(state);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
