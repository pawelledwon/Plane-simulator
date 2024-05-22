using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SimplePlaneController {
    [CustomEditor(typeof(AirplaneInputKeyboardDelegate))]
    public class AirplaneInputKeyboardDelegate_Editor : Editor {
        private AirplaneInputKeyboardDelegate targetScript;

        void OnEnable() {
            targetScript = (AirplaneInputKeyboardDelegate) target;
        }

        public override void OnInspectorGUI() {

            EditorGUILayout.LabelField("Keyboard Delegate Input (For Developers)", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("This is a hybrid of the keyboard and delegate inputs merged together. This is experimental", MessageType.Info);
            GUILayout.Space(5);

            string inputLogs = "";
            inputLogs += "Pitch: " + targetScript.Pitch + "\n";
            inputLogs += "Roll: " + targetScript.Roll + "\n";
            inputLogs += "Yaw: " + targetScript.Yaw + "\n";
            inputLogs += "Throttle Input: " + targetScript.Throttle + "\n";
            inputLogs += "Throttle Value: " + targetScript.StickyThrottle + "\n";
            inputLogs += "Flaps: " + targetScript.Flaps + "\n";
            inputLogs += "Brakes: " + targetScript.Brake;

            EditorGUILayout.LabelField("Current Inputs:", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(inputLogs, MessageType.None);
            GUILayout.Space(5);



            EditorGUILayout.LabelField("Settings:", EditorStyles.boldLabel);
            GUILayout.Space(5);

            targetScript.maxFlaps = EditorGUILayout.IntField("Max Flap Steps", targetScript.maxFlaps);
            targetScript.inputSensitivity = EditorGUILayout.Slider("Input Sensitivity", targetScript.inputSensitivity, 0.1f, 0.5f);
            targetScript.throttleStepSize = EditorGUILayout.Slider("Throttle Step Size", targetScript.throttleStepSize, 0.1f, 1f);

            targetScript.startingThrottle = EditorGUILayout.Slider("Starting Throttle Position", targetScript.startingThrottle, 0f, 1f);
            targetScript.autoBrake = EditorGUILayout.Toggle("Auto Brake (Wheels)", targetScript.autoBrake);
            if (targetScript.autoBrake)
            {
                EditorGUILayout.HelpBox("Auto brake will be applied to all wheels which can brake, when throttle is in closed positon", MessageType.Info);
            }


            GUILayout.Space(5);

            EditorGUILayout.LabelField("Keyboard Mapping:", EditorStyles.boldLabel);
            GUILayout.Space(5);

            targetScript.pitchUpKey = (KeyCode)EditorGUILayout.EnumPopup("Pitch Up", targetScript.pitchUpKey);
            targetScript.pitchDownKey = (KeyCode)EditorGUILayout.EnumPopup("Pitch Down", targetScript.pitchDownKey);
            targetScript.rollLeftKey = (KeyCode)EditorGUILayout.EnumPopup("Roll Left", targetScript.rollLeftKey);
            targetScript.rollRightKey = (KeyCode)EditorGUILayout.EnumPopup("Roll RIght", targetScript.rollRightKey);
            targetScript.yawLeftKey = (KeyCode)EditorGUILayout.EnumPopup("Yaw Left", targetScript.yawLeftKey);
            targetScript.yawRightKey = (KeyCode)EditorGUILayout.EnumPopup("Yaw Right", targetScript.yawRightKey);
            targetScript.throttleUpKey = (KeyCode)EditorGUILayout.EnumPopup("Throttle Increase", targetScript.throttleUpKey);
            targetScript.throttleDownKey = (KeyCode)EditorGUILayout.EnumPopup("Throttle Decrease", targetScript.throttleDownKey);
            targetScript.flapsDownKey = (KeyCode)EditorGUILayout.EnumPopup("Flaps Increase", targetScript.flapsDownKey);
            targetScript.flapsUpKey = (KeyCode)EditorGUILayout.EnumPopup("Flaps Decrease", targetScript.flapsUpKey);
            targetScript.brakeKey = (KeyCode)EditorGUILayout.EnumPopup("Apply Brakes", targetScript.brakeKey);
            targetScript.cameraSwitchKey = (KeyCode)EditorGUILayout.EnumPopup("Camera Switch", targetScript.cameraSwitchKey);
            targetScript.engineCutoffKey = (KeyCode)EditorGUILayout.EnumPopup("Engine Cutoff", targetScript.engineCutoffKey);
            targetScript.lightToggleKey = (KeyCode)EditorGUILayout.EnumPopup("Toggle Lights", targetScript.lightToggleKey);
            targetScript.langingGearToggleKey = (KeyCode)EditorGUILayout.EnumPopup("Landing Gear Toggle", targetScript.langingGearToggleKey);

            GUILayout.Space(20);

            EditorGUILayout.LabelField("Overrides:", EditorStyles.boldLabel);
            GUILayout.Space(5);

            targetScript.pitchOverride = EditorGUILayout.Slider("Pitch Override", targetScript.pitchOverride, -1f, 1f);
            targetScript.rollOverride = EditorGUILayout.Slider("Roll Override", targetScript.rollOverride, -1f, 1f);
            targetScript.yawOverride = EditorGUILayout.Slider("Yaw Override", targetScript.yawOverride, -1f, 1f);
            targetScript.throttleOverride = EditorGUILayout.Slider("Throttle Override", targetScript.throttleOverride, -1f, 1f);
            targetScript.flapsOverride = EditorGUILayout.IntField("Flaps Override", targetScript.flapsOverride);
            targetScript.brakeOverride = EditorGUILayout.Slider("Wheel Brake Override", targetScript.brakeOverride, 0f, 1f);

            targetScript.cameraSwitchOverride = EditorGUILayout.Toggle("Toggle Camera", targetScript.cameraSwitchOverride);
            targetScript.engineCutoffOverride = EditorGUILayout.Toggle("Toggle Engine", targetScript.engineCutoffOverride);
            targetScript.lightToggleOverride = EditorGUILayout.Toggle("Toggle Lights", targetScript.lightToggleOverride);
            targetScript.landingGearToggleOverride = EditorGUILayout.Toggle("Toggle Landing Gear Assemblies", targetScript.landingGearToggleOverride);

            Repaint();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(targetScript);
            }

        }
    }
}
