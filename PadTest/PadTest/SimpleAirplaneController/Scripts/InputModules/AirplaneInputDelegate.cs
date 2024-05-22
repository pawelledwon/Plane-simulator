using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePlaneController
{
    public class AirplaneInputDelegate : AirplaneInput
    {
        /* Variables */
        public float pitchOverride = 0f;
        public float rollOverride = 0f;
        public float yawOverride = 0f;
        public float throttleOverride = 0f;
        public int flapsOverride = 0;
        public float brakeOverride = 0f;
        public bool cameraSwitchOverride = false;
        public bool engineCutoffOverride = false;
        public bool lightToggleOverride = false;
        public bool landingGearToggleOverride = false;

        public override void GetInput()
        {
            pitch = pitchOverride;
            roll = rollOverride;
            yaw = yawOverride;

            throttle = throttleOverride;

            ApplyStickyThrottle();

            brake = brakeOverride;
            flaps = Mathf.Clamp(flapsOverride, 0, maxFlaps);

            cameraSwitch = cameraSwitchOverride;
            engineCutoff = engineCutoffOverride;
            lightToggle = lightToggleOverride;
            landingGearToggle = landingGearToggleOverride;

            DisableToggles();

            ApplyAutoBrake();
        }

        public void SetPitch(float sPitch)
        {
            pitchOverride = Mathf.Clamp(pitchOverride, -1f, 1f);
            pitchOverride = sPitch;
        }

        public void SetRoll(float sRoll)
        {
            sRoll = Mathf.Clamp(sRoll, -1f, 1f);
            rollOverride = sRoll;
        }

        public void SetYaw(float sYaw)
        {
            sYaw = Mathf.Clamp(sYaw, -1f, 1f);
            yawOverride = sYaw;
        }

        public void SetThrottle(float sThrottle)
        {
            sThrottle = Mathf.Clamp(sThrottle, -1f, 1f);
            throttleOverride = sThrottle;
        }

        public void SetRawThrottle(float sThrottle)
        {
            sThrottle = Mathf.Clamp01(sThrottle);
            stickyThrottle = sThrottle;
        }

        public void SetBrake(float sBrake)
        {
            sBrake = Mathf.Clamp(sBrake, 0f, 1f);
            brakeOverride = sBrake;
        }

        public void SetFlaps(int sFlaps)
        {
            flapsOverride = sFlaps;
        }

        public void ToggleCamera()
        {
            cameraSwitchOverride = !cameraSwitchOverride;
        }

        public void ToggleEngine()
        {
            engineCutoffOverride = !engineCutoffOverride;
        }

        public void ToggleLights()
        {
            lightToggleOverride = !lightToggleOverride;
        }

        public void ToggleLandingGearAssembly()
        {
            landingGearToggleOverride = !landingGearToggleOverride;
        }

        public void DisableToggles()
        {
            if (cameraSwitchOverride) {
                cameraSwitchOverride = false;
            }

            if (engineCutoffOverride) {
                engineCutoffOverride = false;
            }

            if (lightToggleOverride) {
                lightToggleOverride = false;
            }

            if (landingGearToggleOverride) {
                landingGearToggleOverride = false;
            }
        }
    }
}
