using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePlaneController {
    public class AirplanePropeller : MonoBehaviour {

        public float minRotationRPM = 1100f;
        public float minBlurRPM = 300f;

        public float rotationSpeedChange = 20f;
        public GameObject primaryPropeller;
        public GameObject blurredPropeller;

        public Vector3 rotationAxis = Vector3.forward;

        private float currentIdleRPM;

        public void Start(){
            currentIdleRPM = 0;
            SwapPropellers(0f);
        }

        public void UpdatePropeller(AirplaneEngine engine){
            float currentRPM = engine.RPM;

            if(engine.EngineOff){
                if (currentIdleRPM > 0.1f){
                    currentIdleRPM -= rotationSpeedChange;
                } else {
                    currentIdleRPM = 0f;
                }
            } else { 
                if(currentIdleRPM < (minRotationRPM - 0.1f)){
                    currentIdleRPM += rotationSpeedChange;
                } else {
                    currentIdleRPM = minRotationRPM;
                }
            }

            float degreesPerSecond = (((currentRPM * 360f) / 60f) + currentIdleRPM) * Time.deltaTime;
            degreesPerSecond = Mathf.Clamp(degreesPerSecond, 0f, currentIdleRPM);

            transform.Rotate(rotationAxis, degreesPerSecond);

            if(primaryPropeller && blurredPropeller){
                SwapPropellers(currentRPM);
            }
        }

        public void SwapPropellers(float currentRPM){
            if(currentRPM > minBlurRPM){
                blurredPropeller.gameObject.SetActive(true);
                primaryPropeller.gameObject.SetActive(false);
            } else {
                blurredPropeller.gameObject.SetActive(false);
                primaryPropeller.gameObject.SetActive(true);
            }
        }

    }
}
