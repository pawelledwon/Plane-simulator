using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePlaneController {
    public class AirplaneEngine : MonoBehaviour {

        [Header("Primary Settings")]
        public float maxForce = 200f;
        public float maxRPM = 2550f;
        public float shutoffSpeed = 0.3f;
        public AnimationCurve powerCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public AirplanePropeller propeller;

        [Header("Audio Settings")]
        public AudioSource idleSound;
        public AudioSource fullThrottleSound;
        public float maxPitchShift = 1.2f;

        [Range(0.0f, 1.0f)]
        public float maxVolume = 1f;

        [Range(0.0f, 0.5f)]
        public float volumeShutoffFadeSpeed = 0.02f;

        [Header("Optional Settings")]
        [Tooltip("Leave unassigned to disable the need for fuel")]
        public AirplaneFuelTank fuelTank;

        /* Private Vars */
        private float finalIdleVolume;

        private float finalFullVolume;
        private float finalFullPitch;
        private float currentRPM;

        private bool engineOff = false;
        private float lastThrottleValue;

        /* Properties */
        public float RPM{
            get{
                return currentRPM;
            }
        }

        public bool EngineOff { 
            get{
                return engineOff;
            }
        }

        /* Methods */
        void Start(){
            if(idleSound){
                idleSound.loop = true;
                if(idleSound.volume > maxVolume){
                    finalIdleVolume = maxVolume;
                    idleSound.volume = finalIdleVolume;
                }
            }

            if(fullThrottleSound){
                fullThrottleSound.loop = true;
                fullThrottleSound.volume = 0;
            }

            if(fuelTank){
                fuelTank.InitFuel();
            }
        }

        public Vector3 CalculateForce(float throttle){

            float finalThrottle = Mathf.Clamp01(throttle);

            if(!engineOff){
                finalThrottle = powerCurve.Evaluate(finalThrottle);
                lastThrottleValue = finalThrottle;
            } else {
                lastThrottleValue -= Time.deltaTime * shutoffSpeed;
                lastThrottleValue = Mathf.Clamp01(lastThrottleValue);
                finalThrottle = powerCurve.Evaluate(lastThrottleValue);
            }

            UpdateAudio(finalThrottle);

            currentRPM = finalThrottle * maxRPM;
            if(propeller){
                propeller.UpdatePropeller(this);
            }

            HandleFuel(finalThrottle);

            float finalPower = finalThrottle * maxForce;

            Vector3 finalForce = transform.forward * finalPower;
            return finalForce;
        }


        public void CheckCutoffSwitch(bool cutoff){
            if(cutoff){
                engineOff = !engineOff;
            }
        }

        void UpdateAudio(float throttle){
            if(idleSound){
                if (engineOff) {
                    /* Engine is off, probably this should totally stop playing */
                    if(finalIdleVolume > volumeShutoffFadeSpeed){
                        finalIdleVolume -= volumeShutoffFadeSpeed;
                    } else {
                        finalIdleVolume = 0f;
                    }
                } else {
                    if(finalIdleVolume < (maxVolume - volumeShutoffFadeSpeed)){
                        finalIdleVolume += volumeShutoffFadeSpeed;
                    } else {
                        finalIdleVolume = maxVolume;
                    }
                }

                idleSound.volume = finalIdleVolume;
            }

            if(fullThrottleSound){
                finalFullVolume = Mathf.Lerp(0f, maxVolume, throttle);
                finalFullPitch = Mathf.Lerp(1f, maxPitchShift, throttle);

                fullThrottleSound.volume = finalFullVolume;
                fullThrottleSound.pitch = finalFullPitch;
            }
        }

        void HandleFuel(float finalThrottle){
            if(fuelTank){
                fuelTank.UpdateFuel(finalThrottle);

                if(fuelTank.CurrentFuel <= 0f){
                    engineOff = true;
                }
            }
        }
    }
}
