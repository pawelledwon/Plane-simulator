using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePlaneController {
    public class AirplaneCameraController : MonoBehaviour {

        public Transform target;
        public float distance = 5f;
        public float height = 2f;
        public float smoothSpeed = 0.5f;
        public float minHeightFromGround = 2f;

        private Vector3 smoothVel;
        private float originalHeight;

        void Start() {
            originalHeight = height;
        }

        void FixedUpdate() {
            if(target){
                ControlCamera();
            }
        }

        protected virtual void ControlCamera(){

            RaycastHit hit;
            if(Physics.Raycast(transform.position, Vector3.down, out hit)){
                if(hit.distance < minHeightFromGround){
                    float goalHeight = originalHeight + (minHeightFromGround - hit.distance);
                    height = goalHeight;
                }
            }

            Vector3 positionGoal = target.position + (-target.forward * distance) + (Vector3.up * height);
            transform.position = Vector3.SmoothDamp(transform.position, positionGoal, ref smoothVel, smoothSpeed);
            transform.LookAt(target);
        }
    }
}
