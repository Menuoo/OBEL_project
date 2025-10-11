using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIcontroller : MonoBehaviour
{
    [SerializeField] GameObject textBox;
    [SerializeField] TMP_Text text;
    [SerializeField] float textDisplaySpeed = 30f;

    bool textMenuOpen = false;
    string[] textToDisplay;
    int textLength = 0;
    int index = 0;
    float charAmount = 0;



    [SerializeField] PlayerInputUI inputUI;
    [SerializeField] PauseMenu pauseMenu;


    private void OnEnable()
    {
        PlayerInput.PauseGameEvent += EnablePauseMenu;
    }

    private void OnDisable()
    {
        PlayerInput.PauseGameEvent -= EnablePauseMenu;
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
        inputUI.input.SwapControls(1);

        textBox.SetActive(false);
        textMenuOpen = false;
    }


    public void EnablePauseMenu(bool state)
    { 
        pauseMenu.gameObject.SetActive(state);
    }
}
