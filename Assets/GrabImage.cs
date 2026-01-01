using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrabImage : MonoBehaviour
{
    [SerializeField] int imgToDisplay=0;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Image>().sprite = 
            Sprite.Create(DataVariables.GrabImage(imgToDisplay), new Rect(0, 0, 640, 360), new Vector2(0, 0), .01f);
        //DataVariables.GrabImage(new string(" "));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
