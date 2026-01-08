using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class UIcontroller : MonoBehaviour
{
    // text speech thing
    [Header("Text Box")]
    [SerializeField] GameObject textBox;
    [SerializeField] TMP_Text text;
    [SerializeField] float textDisplaySpeed = 30f;
    bool textMenuOpen = false;

    // item pickup thing
    [Header("Item Box")]
    [SerializeField] GameObject itemBox;
    [SerializeField] TMP_Text itemText;
    [SerializeField] UnityEngine.UI.Image itemImage;
    bool itemMenuOpen = false;
    FieldItem current = null;

    string[] textToDisplay;
    int textLength = 0;
    int index = 0;
    float charAmount = 0;

    [Header("Components")]
    [SerializeField] PlayerInteractions interactions;
    [SerializeField] ItemDatabase ItemDatabase;
    [SerializeField] PlayerInputUI inputUI;
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] SaveMenu saveMenu;

    bool pauseState = false;


    private void OnEnable()
    {
        PlayerInput.PauseGameEvent += TransitionPauseMenu;
    }

    private void OnDisable()
    {
        PlayerInput.PauseGameEvent -= TransitionPauseMenu;
    }

    private void Awake()
    {
        ItemDatabase.InitializeList();
    }

    void Start()
    {
        textBox.SetActive(false);
    }

    void Update()
    {
        // have to add a check for whether we're even doing this in the first place

        if (textMenuOpen)
        {
            if (textLength > charAmount)
                charAmount += Time.deltaTime * textDisplaySpeed;

            text.maxVisibleCharacters = (int)charAmount;


            if (inputUI.ConfirmPressed)
            {
                ContinueTextBox();
            }
        }

        if (itemMenuOpen)
        {
            if (inputUI.ConfirmPressed)
            {
                current.Confirm(interactions);
                EndItemBox();
            }

            //    TO    IMPLEMENT
            //if (cancel pressed)
        }
    }

    public void StartItemBox(int id, int amnt, FieldItem fieldItem)
    {
        current = fieldItem;

        inputUI.input.SwapControls(0);
        itemBox.SetActive(true);

        InventoryItem item = ItemDatabase.GetItem(id);

        itemText.text = item.GetName() + " [x" + amnt + "]";
        itemImage.sprite = ItemDatabase.GetItem(id).GetSprite();

        itemMenuOpen = true;
    }

    public void EndItemBox()
    {
        inputUI.input.SwapControls(1);

        itemBox.SetActive(false);
        itemMenuOpen = false;
    }


    public void StartTextBox(string[] texts)
    {
        textMenuOpen = true;
        textToDisplay = texts;
        index = 0;
        textLength = textToDisplay[index].Length;
        charAmount = 0;
        text.maxVisibleCharacters = 0;

        inputUI.input.SwapControls(0);

        textBox.SetActive(true);
        text.text = textToDisplay[index];
    }

    public void ContinueTextBox()
    {
        if (charAmount < textLength)
        {
            charAmount = textLength + 1;
            text.maxVisibleCharacters = textLength + 1;
            return;
        }


        index++;
        if (index >= textToDisplay.Length)
        {
            EndTextBox();
            return;
        }

        textLength = textToDisplay[index].Length;
        charAmount = 0;
        text.maxVisibleCharacters = 0;

        text.text = textToDisplay[index];
    }

    public void EndTextBox()
    {
        if (textMenuOpen)
        {
            inputUI.input.SwapControls(1);

            textBox.SetActive(false);
            textMenuOpen = false;
        }
    }

    public void EnableSaveMenu(bool state)
    {
        if (state)
        {
            TransitionLogic.instance.StartCoroutine(TransitionLogic.instance.TransitionOneZero());
            inputUI.input.SwapControls(-1);

            StartCoroutine(RealInvoke(EnableControls, TransitionLogic.instance.transTime * 2f));
            StartCoroutine(RealInvoke(SaveMenu, TransitionLogic.instance.transTime));

            pauseState = true;
            PauseHandler.PauseTime();
        }
        else
        {
            TransitionLogic.instance.StartCoroutine(TransitionLogic.instance.TransitionOneZero());
            inputUI.input.SwapControls(-1);

            StartCoroutine(RealInvoke(EnableControls, TransitionLogic.instance.transTime * 2f));
            StartCoroutine(RealInvoke(SaveMenu, TransitionLogic.instance.transTime));

            pauseState = false;
        }
    }

    public void SaveMenu()
    { 
        saveMenu.gameObject.SetActive(pauseState ? true : false);
        Debug.Log("this executes");
    }

    public void EnableControls()
    {
        inputUI.input.SwapControls(pauseState ? 0 : 1);
        if (!pauseState)
            PauseHandler.ResumeTime();
    }



    public void EnablePauseMenu(bool state)
    { 
        pauseMenu.gameObject.SetActive(state);
    }

    public void PauseMenuChange(bool state)
    {
        if (state)
        {
            Debug.Log(pauseState);
            EnablePauseMenu(pauseState);
            TransitionLogic.instance.TransitionScene(false);
        }
        else
        {
            TransitionLogic.TransitionEvent -= PauseMenuChange;

            inputUI.input.SwapControls(pauseState ? 0 : 1);

            if (!pauseState)
            {
                PauseHandler.ResumeTime();
            }
        }
    }

    public void TransitionPauseMenu(bool value)
    {
        pauseState = value;

        TransitionLogic.instance.TransitionScene(true);
        TransitionLogic.TransitionEvent += PauseMenuChange;

        inputUI.input.SwapControls(-1);
        if (pauseState)
            PauseHandler.PauseTime();
    }


    public IEnumerator RealInvoke(System.Action action, float Delay)
    {
        Debug.Log("we here");
        yield return new WaitForSecondsRealtime(Delay);
        if (action != null)
            action();
    }
}
