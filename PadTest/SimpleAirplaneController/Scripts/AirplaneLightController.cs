using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePlaneController {
	public class AirplaneLightController : MonoBehaviour {
		public bool enablePulse;
		public AnimationCurve pulseCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 1f);
		public float pulseTime = 1f;
		public float minIntensity = 0f;

		private Light attachedLight;
		private float maxIntensity;
		private float lastTime;

		public void Start(){
			if(GetComponent<Light>()){
				attachedLight = GetComponent<Light>();
				maxIntensity = attachedLight.intensity;

				if(minIntensity > maxIntensity){
					minIntensity = maxIntensity;
				}

				if(pulseTime < 0f){
					pulseTime = 0.1f;
				}

				lastTime = Time.time;

			}
		}

		public void Update(){
			if(attachedLight != null){
				if(enablePulse){
					UpdateIntensityPulse();
				}
			}
		}

		private void UpdateIntensityPulse(){
			float intensityMultiplier = pulseCurve.Evaluate((Time.time - lastTime) / pulseTime);

			if(Time.time - lastTime > pulseTime){
				lastTime = Time.time;
			}

			attachedLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, intensityMultiplier);
		}
	}
}