using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePlaneController
{
    public class AirplaneInputAxes : AirplaneInput
    {
        public string pitchAxes = "Vertical";
        public string rollAxes = "Horizontal";
        public string yawAxes = "Airplane Yaw";
        public string throttleAxes = "Airplane Throttle";
        public string flapsAxes = "Airplane Flaps Axes";
        public string brakeAxes = "Airplane Wheel Brake";
        public string cameraAxes = "Airplane Camera Toggle";
        public string engineCutoffAxes = "Airplane Engine Cutoff Toggle";
        public string lightToggleAxes = "Airplane Light Toggle";
        public string langingGearToggleAxes = "Airplane Gear Toggle";

       
        public override void GetInput()
        {
            pitch = EvaluateAxes(pitchAxes);
            roll = EvaluateAxes(rollAxes);
            yaw = EvaluateAxes(yawAxes);

            throttle = EvaluateAxes(throttleAxes);
            ApplyStickyThrottle();

            brake = Mathf.Clamp01(EvaluateAxes(brakeAxes));

            if(EvaluateAxes(flapsAxes) > 0.1f) {
                flaps++;
            } else if (EvaluateAxes(flapsAxes) < -0.1f){
                flaps++;
            }

            flaps = Mathf.Clamp(flaps, 0, maxFlaps);

            cameraSwitch = EvaluateAxes(cameraAxes) > 0.1f ? true : false;
            engineCutoff = EvaluateAxes(engineCutoffAxes) > 0.1f ? true : false;
            lightToggle = EvaluateAxes(lightToggleAxes) > 0.1f ? true : false;
            landingGearToggle = EvaluateAxes(langingGearToggleAxes) > 0.1f ? true : false;

            ApplyAutoBrake();
        }

        private float EvaluateAxes(string name)
        {
            if (AxesExists(name))
            {
                return Input.GetAxis(name);
            }
            return 0f;
        }

        private bool AxesExists(string name)
        {
            try {
                float sample = Input.GetAxis(name);
                return true;
            } catch (System.ArgumentException ex){
                Debug.Log("Airplane Controller:" + ex.Message);
                return false;
            }
        }
    }
}