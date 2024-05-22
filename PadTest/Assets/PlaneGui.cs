using SimplePlaneController;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaneGui : AirplaneInput
{
    public TextMeshProUGUI textMode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textMode.text = "Mode: " + playerInput.currentControlScheme;
    }
}
