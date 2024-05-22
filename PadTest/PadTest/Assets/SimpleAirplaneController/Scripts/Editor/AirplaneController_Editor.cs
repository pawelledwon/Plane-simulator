using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SimplePlaneController {
    [CustomEditor(typeof(AirplaneController))]
    public class AirplaneController_Editor : Editor {
        private AirplaneController targetScript;

        void OnEnable(){
            targetScript = (AirplaneController)target;
        }

        public override void OnInspectorGUI(){
            EditorGUILayout.LabelField("General:", EditorStyles.boldLabel);
            GUILayout.Space(5);

            targetScript.weight = EditorGUILayout.FloatField("Weight", targetScript.weight);
            targetScript.isPounds = EditorGUILayout.Toggle("Is Pounds", targetScript.isPounds);
            targetScript.centerOfGravity = (Transform)EditorGUILayout.ObjectField("Center Of Gravity", targetScript.centerOfGravity, typeof(Transform), true);
            GUILayout.Space(5);


            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("engines"), true);
            GUILayout.Space(5);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("wheels"), true);
            GUILayout.Space(5);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("controlSurfaces"), true);
            GUILayout.Space(5);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("landingGearAssemblies"), true);
            GUILayout.Space(5);

            EditorGUILayout.HelpBox("Landing Gear Assemblies should not contain wheel colliders, and should only be added to planes with moving landing gear assemblies which need to be retracted after take off", MessageType.None);

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed) {
                EditorUtility.SetDirty(targetScript);
            }

        }
    }
}
