using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace SimplePlaneController
{
    [CustomEditor(typeof(AirplaneInputDelegate))]
    public class AirplaneInputDelegate_Editor : Editor
    {
        private AirplaneInputDelegate targetScript;
        void OnEnable()
        {
            targetScript = (AirplaneInputDelegate)target;
        }

        public override void OnInspectorGUI()
        {

            EditorGUILayout.LabelField("Delegate Input (For Developers)", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Provides manual overrides, exposed as public variables, and methods which can be publicly called", MessageType.Info);
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
            targetScript.throttleStepSize = EditorGUILayout.Slider("Throttle Step Size", targetScript.throttleStepSize, 0.1f, 1f);

            targetScript.startingThrottle = EditorGUILayout.Slider("Starting Throttle Position", targetScript.startingThrottle, 0f, 1f);
            targetScript.autoBrake = EditorGUILayout.Toggle("Auto Brake (Wheels)", targetScript.autoBrake);
            if (targetScript.autoBrake){
                EditorGUILayout.HelpBox("Auto brake will be applied to all wheels which can brake, when throttle is in closed positon", MessageType.Info);
            }

            GUILayout.Space(5);

            EditorGUILayout.LabelField("Input Mapping:", EditorStyles.boldLabel);
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

            GUILayout.Space(20);

            Repaint();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(targetScript);
            }

        }
    }
}
