using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePlaneController {

    [RequireComponent(typeof(WheelCollider))]
    public class AirplaneWheel : MonoBehaviour {

        public Transform wheelObject;
        public bool canBrake = true;
        public float brakeForce = 200f;

        public bool canSteer = false;
        public float steerAngle = 20f;
        public float steerSmoothSpeed = 2f;
        public bool invertSteering = true;

        private WheelCollider wheelCol;
        private Vector3 worldPos;
        private Quaternion worldRot;
        private float finalBrakeForce;
        private float finalSteerAngle;

        private bool wheelRetracted = false;
        private Vector3 localStartingPos;
        private Quaternion localStartingRot;

        const float baseMotorToque = 0.000000000000001f;

        void Start(){
            wheelCol = GetComponent<WheelCollider>();
            if (wheelObject){
                localStartingPos = wheelObject.localPosition;
                localStartingRot = wheelObject.localRotation;
            }
        }

        public void InitWheel(){
            if(wheelCol){
                wheelCol.motorTorque = baseMotorToque;
            }
        }

        public void UpdateWheel(AirplaneInput input){
            if(wheelCol && !wheelRetracted){
                wheelCol.GetWorldPose(out worldPos, out worldRot);
                if(wheelObject){
                    wheelObject.rotation = worldRot;
                    wheelObject.position = worldPos;
                }

                if(canBrake){
                    if(input.Brake > 0.1f){
                        finalBrakeForce = Mathf.Lerp(finalBrakeForce, input.Brake * brakeForce, Time.deltaTime);
                        wheelCol.brakeTorque = finalBrakeForce;
                    } else {
                        finalBrakeForce = 0f;
                        wheelCol.brakeTorque = finalBrakeForce;
                        wheelCol.motorTorque = baseMotorToque;
                    }
                }

                if(canSteer){
                    finalSteerAngle = Mathf.Lerp(finalSteerAngle, input.Yaw * (invertSteering ? -steerAngle : steerAngle), Time.deltaTime * steerSmoothSpeed);
                    wheelCol.steerAngle = finalSteerAngle;
                }
            } else if(wheelRetracted && wheelObject){
                wheelObject.localPosition = localStartingPos;
            }
        }

        public void SetWheelRetracted(bool state)
        {
            wheelRetracted = state;
            if (wheelCol){
                wheelCol.enabled = !state;
            }
        }

        public bool GetWheelRetracted()
        {
            return wheelRetracted;
        }
    }
}
