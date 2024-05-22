﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using TMPro;
using System;
using SimplePlaneController;

namespace SimplePlaneController {
    public class AirplaneInput : MonoBehaviour {

        /* Variables */
        protected float pitch = 0f;
        protected float roll = 0f;
        protected float yaw = 0f;
        protected float throttle = 0f;
        protected int flaps = 0;
        protected float brake = 0f;
        protected float stickyThrottle;
        protected bool cameraSwitch = false;
        protected bool engineCutoff = false;
        protected bool lightToggle = false;
        protected bool landingGearToggle = false;

        protected float timeSinceLastTick = 0f;

        public int maxFlaps = 3;
        public float inputSensitivity = 0.1f;
        public float throttleStepSize = 0.1f;
        public float startingThrottle = 0f;
        public bool autoBrake = false;

        public KeyCode pitchUpKey = KeyCode.W;
        public KeyCode pitchDownKey = KeyCode.S;
        public KeyCode rollLeftKey = KeyCode.A;
        public KeyCode rollRightKey = KeyCode.D;
        public KeyCode yawLeftKey = KeyCode.Z;
        public KeyCode yawRightKey = KeyCode.X;
        public KeyCode throttleUpKey = KeyCode.Joystick1Button5;
        public KeyCode throttleDownKey = KeyCode.Joystick1Button6;
        public KeyCode flapsDownKey = KeyCode.F;
        public KeyCode flapsUpKey = KeyCode.G;
        public KeyCode brakeKey = KeyCode.Space;
        public KeyCode cameraSwitchKey = KeyCode.C;
        public KeyCode engineCutoffKey = KeyCode.K;
        public KeyCode lightToggleKey = KeyCode.L;
        public KeyCode langingGearToggleKey = KeyCode.Q;

        YokeControls controls;
        protected PlayerInput playerInput;
        float throttlePlus, throttleMinus, yawLeft, yawRight;

        /* Properties */
        public float Pitch {
            get {
                return pitch;
            }
        }

        public float Roll {
            get {
                return roll;
            }
        }

        public float Yaw {
            get {
                return yaw;
            }
        }

        public float Throttle {
            get {
                return throttle;
            }
        }

        public float Flaps {
            get {
                return flaps;
            }
        }

        public bool LandingGear { 
            get {
                return landingGearToggle;
            }
        }

        public float FlapsNormalized {
            get{
                return Mathf.InverseLerp(0, maxFlaps, flaps);
            }
        }

        public float Brake {
            get {
                return brake;
            }
        }

        public float StickyThrottle{
            get {
                return stickyThrottle;
            }
        }

        public bool CameraSwitch{
            get {
                return cameraSwitch;
            }
        }

        public bool EngineCutoff{
            get {
                return engineCutoff;
            }
        }

        public bool LightToggle{
            get {
                return lightToggle;
            }
        }

        /* Methods */
        void Awake()
        {
            controls = new YokeControls();
            playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }

        void Start() {
            if(startingThrottle > 0.01f){
                stickyThrottle = Mathf.Clamp01(startingThrottle);
            }
        }

        void Update(){
            GetInput();
        }

        public virtual void GetInput(){
            //pitch = ApplyAxisInput(pitch, pitchUpKey, pitchDownKey);
            pitch = controls.Airplane.Pitch.ReadValue<float>();
            //roll = ApplyAxisInput(roll, rollLeftKey, rollRightKey);
            roll = -controls.Airplane.Roll.ReadValue<float>();
            //yaw = ApplyAxisInput(yaw, yawLeftKey, yawRightKey);
            //yawLeft = controls.Airplane.YawLeft.ReadValue<float>();
            //yawRight = controls.Airplane.YawRight.ReadValue<float>();
            //yaw = (yawLeft > yawRight ? -yawLeft : yawRight);
            yaw = -controls.Airplane.Yaw.ReadValue<float>();

            //throttle = ApplyAxisInput(throttle, throttleUpKey, throttleDownKey);

            //throttlePlus = controls.Airplane.ThrottlePlus.ReadValue<float>();
            //throttleMinus = controls.Airplane.ThrottleMinus.ReadValue<float>();
            //throttle = (throttlePlus > throttleMinus ? throttlePlus : -throttleMinus);
            throttle = controls.Airplane.Throttle.ReadValue<float>();
            ApplyStickyThrottle();

            //brake = Input.GetKey(brakeKey) ? 1f : 0f;
            brake = controls.Airplane.Brake.ReadValue<float>();

            //if (controls.Airplane.Brake.WasPressedThisFrame())
            //{
            //    Debug.Log("bober");
            //}

            //if(Input.GetKeyDown(flapsDownKey)){
            //    flaps ++;
            //}

            //if(Input.GetKeyDown(flapsUpKey)){
            //    flaps --;
            //}            
            
            if(controls.Airplane.FlapsIncrease.WasPressedThisFrame()){
                flaps ++;
            }

            if(controls.Airplane.FlapsDecrease.WasPressedThisFrame())
            {
                flaps --;
            }

            flaps = Mathf.Clamp(flaps, 0, maxFlaps);

            /* Due to sample differences in Fixed Updates, compared to Updates, these may revert too quickly for the accompanying processes to track */
            /* As such the sample timer was introduced to ensure this is maintained, meaning they are left active for longer */
            cameraSwitch = Input.GetKeyDown(cameraSwitchKey);
            engineCutoff = Input.GetKeyDown(engineCutoffKey);
            lightToggle  = Input.GetKeyDown(lightToggleKey);

            if (Input.GetKeyDown(langingGearToggleKey)) {
                Debug.Log("bober");
                landingGearToggle = !landingGearToggle;
            }

            ApplyAutoBrake();

        }

        public float ApplyAxisInput(float axisValue, KeyCode positiveKey, KeyCode negativeKey){
            if(Input.GetKey(positiveKey)){
                axisValue += inputSensitivity;
            } else if(Input.GetKey(negativeKey)){
                axisValue -= inputSensitivity;
            } else {
                //Normalize to 0
                if(axisValue > inputSensitivity){
                    axisValue -= inputSensitivity;
                } else if(axisValue < -inputSensitivity) {
                    axisValue += inputSensitivity;
                } else {
                    axisValue = 0f;
                }
            }

            axisValue = Mathf.Clamp(axisValue, -1f, 1f);
            return axisValue;
        }

        protected void ApplyStickyThrottle(){
            stickyThrottle = stickyThrottle + (throttle * throttleStepSize * Time.deltaTime);
            stickyThrottle = Mathf.Clamp01(stickyThrottle);

            if (engineCutoff){
                // The engine was recently cut, reset sticky
                stickyThrottle = 0f;
            }
        }

        protected void ApplyAutoBrake(){
            if (autoBrake){
                if(stickyThrottle < 0.01f){
                    brake = 1f;
                }
            }
        }

    }
}
