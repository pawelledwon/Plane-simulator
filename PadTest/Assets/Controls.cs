using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using SimplePlaneController;

public class Controls : MonoBehaviour
{

    public SimplePlaneController.AirplaneInput airplaneInput;

    public TextMeshProUGUI tLeftX;
    public TextMeshProUGUI tLeftY;

    float valueLeftX, valueLeftY;

    YokeControls controls;
    PlayerInput playerInput;


    // Start is called before the first frame update
    void Awake()
    {
        controls = new YokeControls();
        playerInput = GetComponent<PlayerInput>();
        valueLeftX = 0;
        valueLeftX = 0;

        tLeftX.text = "LeftX: " + valueLeftX;
        tLeftY.text = "LeftY: " + valueLeftY;

    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        //tLeftX.text = "LeftX: " + Truncate(controls.Airplane.Pitch.ReadValue<float>(), 4);
//tLeftY.text = "Throttle: " + Truncate(((-controls.Airplane.Roll.ReadValue<float>() + 1f) / 2), 4);

        //airplaneInput.pitch (controls.Airplane.LeftY.ReadValue<float>());
       // airplaneInput.SetRoll(-controls.Airplane.LeftX.ReadValue<float>());
        //airplaneInput.SetYaw(-controls.Airplane.RightX.ReadValue<float>());
       // airplaneInput.SetRawThrottle((-controls.Airplane.Throttle.ReadValue<float>() + 1f) / 2);
      //  airplaneInput.ApplyAxisInput()
    }

    static float Truncate(float value, int digits)
    {
        double mult = Math.Pow(10.0, digits);
        double result = Math.Truncate(mult * value) / mult;
        return (float)result;
    }
}


