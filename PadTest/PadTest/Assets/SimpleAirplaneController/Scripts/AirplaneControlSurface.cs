using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePlaneController {
    public enum ControlSurfaceType {
        RUDDER,
        ELEVATOR,
        FLAP,
        AILERON
    }

    public class AirplaneControlSurface : MonoBehaviour {
        public ControlSurfaceType type = ControlSurfaceType.RUDDER;
        public Vector3 rotationAxis = Vector3.right;
        public float maxAngle = 30f;
        public float smoothSpeed = 2f;
        public Transform surfaceObject;

        private float goalAngle;
        private Quaternion startRotation;

        void Start(){
            if(surfaceObject){
                startRotation = surfaceObject.localRotation;
            }
        }

        void Update(){
            if(surfaceObject){
                Vector3 finalRotationAxis = rotationAxis * goalAngle;
                surfaceObject.localRotation = Quaternion.Slerp(surfaceObject.localRotation, startRotation * Quaternion.Euler(finalRotationAxis), Time.deltaTime * smoothSpeed);
            }
        }

        public void UpdateControlSurface(AirplaneInput input){
            float inputValue = 0f;
            switch(type){
                case ControlSurfaceType.RUDDER:
                    inputValue = input.Yaw;
                    break;
                case ControlSurfaceType.ELEVATOR:
                    inputValue = input.Pitch;
                    break;
                case ControlSurfaceType.FLAP:
                    inputValue = input.Flaps;
                    break;
                case ControlSurfaceType.AILERON:
                    inputValue = input.Roll;
                    break;
            }

            goalAngle = maxAngle * inputValue;



        }
    }
}