using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SimplePlaneController {

    [CustomEditor(typeof(AirplaneInputSimpleAI))]
    public class AirplaneInputSimpleAI_Editor : Editor
    {
        private AirplaneInputSimpleAI targetScript;
        void OnEnable()
        {
            targetScript = (AirplaneInputSimpleAI)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Simple AI Input", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("A very basic AI input moduel which can follow a target, or move along a route of points.\n\nNote, this is not a fully aware or smart AI system, but rather, is a showcase of what could be achieved", MessageType.Info);
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

            EditorGUILayout.LabelField("AI Configuration:", EditorStyles.boldLabel);
            GUILayout.Space(5);

            targetScript.mode = (SimpleAIMode)EditorGUILayout.EnumPopup("Mode", targetScript.mode);

            serializedObject.Update();
            if (targetScript.mode == SimpleAIMode.FOLLOW){
                EditorGUILayout.PropertyField(serializedObject.FindProperty("followTarget"), true);
            } else {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("routeWaypoints"), true);

                targetScript.waypointHitRadius = EditorGUILayout.FloatField("Waypoint Goal Range", targetScript.waypointHitRadius);
            }

            serializedObject.ApplyModifiedProperties();

            GUILayout.Space(5);

            targetScript.maxRollAngle = EditorGUILayout.Slider("Max Roll Angle", targetScript.maxRollAngle, 15f, 90f);
            EditorGUILayout.HelpBox("Please ensure you are not using arcade roll with AI as this will result in less efficient direction management", MessageType.None);


            targetScript.groundRayDistance = EditorGUILayout.FloatField("Ground Hit Distance", targetScript.groundRayDistance);
            EditorGUILayout.HelpBox("AI will only apply control surface inputs once this distance is exceeded, this allows for smooth takeoff", MessageType.None);

            GUILayout.Space(5);

            targetScript.drawGizmos = EditorGUILayout.Toggle("Show AI Routing Gizmos", targetScript.drawGizmos);

            GUILayout.Space(5);

            EditorGUILayout.LabelField("Input Delegates:", EditorStyles.boldLabel);
            GUILayout.Space(5);

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
