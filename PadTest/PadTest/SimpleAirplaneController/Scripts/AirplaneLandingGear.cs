using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePlaneController {
    public class AirplaneLandingGear : MonoBehaviour
    {

        public Vector3 rotationAxis = Vector3.forward;
        public float maxAngle = 90f;
        public float smoothSpeed = 2f;
        public Transform surfaceObject;
        public AirplaneWheel wheel;

        private float goalAngle;
        private Quaternion startRotation;

        private bool shouldRetract = false;

        void Start()
        {
            if (surfaceObject)
            {
                startRotation = surfaceObject.localRotation;
            }
        }

        void Update()
        {
            if (surfaceObject)
            {
                Vector3 finalRotationAxis = rotationAxis * goalAngle;
                surfaceObject.localRotation = Quaternion.Slerp(surfaceObject.localRotation, startRotation * Quaternion.Euler(finalRotationAxis), Time.deltaTime * smoothSpeed);
            }

            if (wheel){
                if (!wheel.GetWheelRetracted() && shouldRetract) {
                    wheel.SetWheelRetracted(true);
                } else if(wheel.GetWheelRetracted() && !shouldRetract && Quaternion.Angle(surfaceObject.localRotation, startRotation) < 2f){
                    wheel.SetWheelRetracted(false);
                }
            }
        }

        public void UpdateLandingAngle(AirplaneInput input)
        {
            goalAngle = maxAngle * (input.LandingGear ? 1f : 0f);
            shouldRetract = input.LandingGear;
        }
    }
}
