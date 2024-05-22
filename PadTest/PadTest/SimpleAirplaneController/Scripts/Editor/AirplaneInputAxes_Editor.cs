using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace SimplePlaneController
{
    [CustomEditor(typeof(AirplaneInputAxes))]
    public class AirplaneInputAxes_Editor : Editor
    {
        private AirplaneInputAxes targetScript;
        void OnEnable()
        {
            targetScript = (AirplaneInputAxes)target;
        }

        public override void OnInspectorGUI()
        {

            EditorGUILayout.LabelField("Native Axes Input", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Uses the Unity Input system. These are controlled under Edit -> Project Settings -> Input.\n\nPlease setup any missing axes or change the names below to preferred axes names.\n\nIf an axes is not setup, it will be ignored", MessageType.Info);
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
            
            if (targetScript.autoBrake){
                EditorGUILayout.HelpBox("Auto brake will be applied to all wheels which can brake, when throttle is in closed positon", MessageType.Info);
            }



            GUILayout.Space(5);

            EditorGUILayout.LabelField("Input Mapping:", EditorStyles.boldLabel);
            GUILayout.Space(5);

            targetScript.pitchAxes = EditorGUILayout.TextField("Pitch Axes", targetScript.pitchAxes);
            targetScript.rollAxes = EditorGUILayout.TextField("Roll Axes", targetScript.rollAxes);
            targetScript.yawAxes = EditorGUILayout.TextField("Yaw Axes", targetScript.yawAxes);
            targetScript.throttleAxes = EditorGUILayout.TextField("Throttle Axes", targetScript.throttleAxes);
            targetScript.brakeAxes = EditorGUILayout.TextField("Wheel Brake Axes", targetScript.brakeAxes);
            targetScript.cameraAxes = EditorGUILayout.TextField("Camera Toggle Axes", targetScript.cameraAxes);
            targetScript.engineCutoffAxes = EditorGUILayout.TextField("Engine Cutoff Toggle Axes", targetScript.engineCutoffAxes);
            targetScript.lightToggleAxes = EditorGUILayout.TextField("Light Toggle Axes", targetScript.lightToggleAxes);
            targetScript.langingGearToggleAxes = EditorGUILayout.TextField("Landing Gear Assembly Toggle Axes", targetScript.langingGearToggleAxes);

            GUILayout.Space(20);

            Repaint();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(targetScript);
            }

        }

    }
}
