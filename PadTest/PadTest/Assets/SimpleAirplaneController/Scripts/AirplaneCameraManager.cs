using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePlaneController {
    public class AirplaneCameraManager : MonoBehaviour {
        public AirplaneInput input;
        public List<Camera> cameras = new List<Camera>();

        private int cameraIndex = 0;

        void Start(){
            DisableAllCameras();
            EnableCamera(0);
        }

        void Update(){
            if(input){
                if(input.CameraSwitch){
                    SwitchCamera();
                }
            }
        }

        public void SwitchCamera(){
            if(cameras.Count > 0){
                DisableAllCameras();

                cameraIndex ++;
                if(cameraIndex >= cameras.Count){
                    cameraIndex = 0;
                }

                EnableCamera(cameraIndex);

            }
        }

        void EnableCamera(int cameraIndex){
            if(cameraIndex >= 0 && cameraIndex < cameras.Count){
                cameras[cameraIndex].enabled = true;
                if(cameras[cameraIndex].GetComponent<AudioListener>() != null){
                    cameras[cameraIndex].GetComponent<AudioListener>().enabled = true;
                }
            }
        }

        void DisableAllCameras(){
            if(cameras.Count > 0){
                foreach(Camera cam in cameras){
                    cam.enabled = false;
                    if(cam.GetComponent<AudioListener>() != null){
                        cam.GetComponent<AudioListener>().enabled = false;
                    }
                }
            }
        }
    }
}
