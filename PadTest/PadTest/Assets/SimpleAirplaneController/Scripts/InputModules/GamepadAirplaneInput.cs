using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SimplePlaneController
{
    public class GamepadAirplaneInput : AirplaneInput
    {
        public TextMeshProUGUI tLeftX;
        public TextMeshProUGUI tLeftY;

        float valueLeftX, valueLeftY;

        YokeControls controls;
        PlayerInput playerInput;

        public string pitchAxes = "Vertical";
        public string rollAxes = "Horizontal";
        public string yawAxes = "Airplane Yaw";
        public string throttleAxes = "Airplane Throttle";
        public string flapsAxes = "Airplane Flaps Axes";
        public string brakeAxes = "Airplane Wheel Brake";
        public string cameraAxes = "Airplane Camera Toggle";
        public string engineCutoffAxes = "Airplane Engine Cutoff Toggle";
        public string lightToggleAxes = "Airplane Light Toggle";
        public string langingGearToggleAxes = "Airplane Gear Toggle";

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
            tLeftX.text = "LeftX: " + Truncate(controls.Airplane.LeftX.ReadValue<float>(), 4);
            tLeftY.text = "LeftY: " + Truncate(controls.Airplane.LeftY.ReadValue<float>(), 4);

            //airplaneInputDelegate.SetPitch(controls.Airplane.LeftY.ReadValue<float>());
            //airplaneInputDelegate.SetRoll(-controls.Airplane.LeftX.ReadValue<float>());
            //airplaneInputDelegate.SetYaw(-controls.Airplane.RightX.ReadValue<float>());
        }

        static float Truncate(float value, int digits)
        {
            double mult = Math.Pow(10.0, digits);
            double result = Math.Truncate(mult * value) / mult;
            return (float)result;
        }

        public override void GetInput()
        {
            pitch = EvaluateAxes(pitchAxes);
            roll = EvaluateAxes(rollAxes);
            //yaw = EvaluateAxes(yawAxes); 
            yaw = -controls.Airplane.RightX.ReadValue<float>();

            throttle = EvaluateAxes(throttleAxes);
            ApplyStickyThrottle();

            brake = Mathf.Clamp01(EvaluateAxes(brakeAxes));

            if(EvaluateAxes(flapsAxes) > 0.1f) {
                flaps++;
            } else if (EvaluateAxes(flapsAxes) < -0.1f){
                flaps++;
            }

            flaps = Mathf.Clamp(flaps, 0, maxFlaps);

            cameraSwitch = EvaluateAxes(cameraAxes) > 0.1f ? true : false;
            engineCutoff = EvaluateAxes(engineCutoffAxes) > 0.1f ? true : false;
            lightToggle = EvaluateAxes(lightToggleAxes) > 0.1f ? true : false;
            landingGearToggle = EvaluateAxes(langingGearToggleAxes) > 0.1f ? true : false;

            ApplyAutoBrake();
        }

        private float EvaluateAxes(string name)
        {
            if (AxesExists(name))
            {
                return Input.GetAxis(name);
            }
            return 0f;
        }

        private bool AxesExists(string name)
        {
            try {
                float sample = Input.GetAxis(name);
                return true;
            } catch (System.ArgumentException ex){
                Debug.Log("Airplane Controller:" + ex.Message);
                return false;
            }
        }
    }
}