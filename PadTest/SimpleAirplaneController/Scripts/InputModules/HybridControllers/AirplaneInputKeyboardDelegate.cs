using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePlaneController {
    public class AirplaneInputKeyboardDelegate : AirplaneInputDelegate {
        public override void GetInput()
        {
            pitch = ApplyAxisInput(pitch, pitchUpKey, pitchDownKey) + pitchOverride;
            roll = ApplyAxisInput(roll, rollLeftKey, rollRightKey) + rollOverride;
            yaw = ApplyAxisInput(yaw, yawLeftKey, yawRightKey) + yawOverride;

            throttle = ApplyAxisInput(throttle, throttleUpKey, throttleDownKey);
            ApplyStickyThrottle();

            brake = Input.GetKey(brakeKey) ? 1f : 0f;
            brake += brakeOverride;

            if (Input.GetKeyDown(flapsDownKey)) {
                flaps++;
            }

            if (Input.GetKeyDown(flapsUpKey)) {
                flaps--;
            }

            flaps = Mathf.Clamp(flaps + flapsOverride, 0, maxFlaps);

            cameraSwitch = Input.GetKeyDown(cameraSwitchKey) || cameraSwitchOverride;
            engineCutoff = Input.GetKeyDown(engineCutoffKey) || engineCutoffOverride;
            lightToggle = Input.GetKeyDown(lightToggleKey) || lightToggleOverride;

            if (Input.GetKeyDown(langingGearToggleKey) || landingGearToggleOverride) {
                landingGearToggle = !landingGearToggle;
            }

            ApplyAutoBrake();

            DisableToggles();

        }
    }
}
