using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePlaneController {

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    public class AirplaneRigidbodyController : MonoBehaviour {

        protected Rigidbody rBody;
        protected AudioSource aSource;

        public virtual void Start() {
            rBody = GetComponent<Rigidbody>();
            aSource = GetComponent<AudioSource>();

            if(aSource){
                aSource.playOnAwake = false;
            }
        }

        // Update is called once per frame
        void FixedUpdate(){
            if(rBody){
                ApplyPhysics();
            }
        }

        protected virtual void ApplyPhysics(){

        }
    }
}