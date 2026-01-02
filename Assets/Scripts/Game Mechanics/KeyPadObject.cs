using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyPadObject : IInteractable
{
    public Transform cameraPivot;
    [SerializeField] LayerMask interactLayer;
    [SerializeField] int doorValue = 0;

    [SerializeField] int passcode = 1000;
    int[] input = new int[4];
    int currentNum = 0;

    [SerializeField] TMP_Text displayText;
    string defaultText = "- - - -";
    public bool isActive = false;


    // Start is called before the first frame update
    void Start()
    {
        if (DataVariables.data.DoorStates.TryGetValue(doorValue, out bool val))
        {
            if (val)
            {
                this.enabled = false;
            }
        }
    }

    private void Update()
    {
        if (isActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                float fac = 640f / Screen.width;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition * fac);
                bool mouseHit = Physics.Raycast(ray, out hit, 1000f, interactLayer);

                if (mouseHit)
                {
                    KeyButton obj = hit.collider.gameObject.GetComponent<KeyButton>();
                    if (obj != null)
                    {
                        StartCoroutine(obj.HandlePress());

                        if (obj.name == "Enter")
                            EnterCode();
                        else if (obj.name == "Reset")
                            DeleteLast();
                        else 
                        {
                            int inNum = int.Parse(obj.name);
                            NewInput(inNum);
                        }
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Close();
            }
        }
    }

    void NewInput(int num)
    {
        Debug.Log(num);
    }

    void EnterCode()
    { 
        
    }

    void DeleteLast()
    {

    }

    public void Open()
    {
        CameraSingle.instance.SpecialCamera(true, cameraPivot);
        isActive = true;
    }

    public void Close()
    {
        CameraSingle.instance.SpecialCamera(false, null);
        isActive = false;
    }

    public override void OnInteract(PlayerInteractions interactions)
    {
        interactions.StartKeypad(this);
    }
}
