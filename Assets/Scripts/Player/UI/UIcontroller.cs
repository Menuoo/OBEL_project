using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIcontroller : MonoBehaviour
{
    [SerializeField] GameObject textBox;
    [SerializeField] TMP_Text text;

    [SerializeField] PlayerInputUI inputUI;

    [SerializeField] float textDisplaySpeed = 30f;

    string[] textToDisplay;
    int textLength = 0;
    int index = 0;
    float charAmount = 0;


    void Start()
    {
        textBox.SetActive(false);
    }

    void Update()
    {
        if (textLength > charAmount)
            charAmount += Time.deltaTime * textDisplaySpeed;

        text.maxVisibleCharacters = (int)charAmount;


        if (inputUI.ConfirmPressed)
        { 
            ContinueTextBox();
        }
    }

    public void StartTextBox(string[] texts)
    {
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
    }
}
