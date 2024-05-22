using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePlaneController {
	public class AirplaneLightManager : MonoBehaviour {

		public enum LightState {ON, OFF};

		public AirplaneInput input;
	    public LightState lightState = LightState.OFF;

	    public List<Light> lights = new List<Light>();

	    public void Start(){
            SetLightState(lightState);
        }

        public void Update(){
            if(input){
                if(input.LightToggle){
                    SetLightState(lightState == LightState.ON ? LightState.OFF : LightState.ON);
                }
            }
        }

        public void SetLightState(LightState state){
        	lightState = state;
        	if(lights.Count > 0){
        		foreach(Light light in lights){
        			light.enabled = lightState == LightState.ON ? true : false;
        		}
        	}
        }

	}
}
