using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SimplePlaneController {
    [CustomEditor(typeof(ExternalForceController))]
    public class ExternalForceController_Editor : Editor {
        private ExternalForceController targetScript;

        void OnEnable() {
            targetScript = (ExternalForceController)target;
        }

        public override void OnInspectorGUI() {
            EditorGUILayout.LabelField("Operation Settings", EditorStyles.boldLabel);
            targetScript.planeTarget = (AirplaneAerodynamics) EditorGUILayout.ObjectField("Plane Target", targetScript.planeTarget, typeof(AirplaneAerodynamics), true);
            targetScript.forceType = (ExternalForceType)EditorGUILayout.EnumPopup("Force Type", targetScript.forceType);
            targetScript.forceMode = (ForceMode)EditorGUILayout.EnumPopup("Force Mode", targetScript.forceMode);

            GUILayout.Space(5);

            EditorGUILayout.LabelField("Force Settings", EditorStyles.boldLabel);
            if(targetScript.forceType == ExternalForceType.Constant){
                targetScript.directionA = EditorGUILayout.Vector3Field("Direction", targetScript.directionA);
                targetScript.forceA = EditorGUILayout.FloatField("Force", targetScript.forceA);

            } else { 
                targetScript.directionA = EditorGUILayout.Vector3Field("Min Direction Range", targetScript.directionA);
                targetScript.directionB = EditorGUILayout.Vector3Field("Max Direction Range", targetScript.directionB);

                GUILayout.Space(5);

                targetScript.forceA = EditorGUILayout.FloatField("Min Force", targetScript.forceA);
                targetScript.forceB = EditorGUILayout.FloatField("Max Force", targetScript.forceB);

                GUILayout.Space(5);

                targetScript.forceCurve = EditorGUILayout.CurveField("Force Curve", targetScript.forceCurve);

                targetScript.curveDurationA = EditorGUILayout.FloatField("Min Curve Duration (sec)", targetScript.curveDurationA);
                targetScript.curveDurationB = EditorGUILayout.FloatField("Max Curve Duration (sec)", targetScript.curveDurationB);
                targetScript.frequencyA = EditorGUILayout.FloatField("Min Frequency (sec)", targetScript.frequencyA);
                targetScript.frequencyB = EditorGUILayout.FloatField("Max Frequency (sec)", targetScript.frequencyB);
            }

            GUILayout.Space(5);

            EditorGUILayout.LabelField("Torque Forces", EditorStyles.boldLabel);
            targetScript.simulateTorque = GUILayout.Toggle(targetScript.simulateTorque, "Simulate Torque Forces");

            if (targetScript.simulateTorque){
                EditorGUILayout.HelpBox("This option is early beta and needs improvement. It uses an approximate torque angle based, which is the cross product of the plane up direction and the input force. Most likely not ideal, use with caution.", MessageType.Info);

                if (targetScript.forceType == ExternalForceType.Constant){
                    targetScript.torqueA = EditorGUILayout.FloatField("Torque", targetScript.torqueA);
                } else {
                    targetScript.torqueA = EditorGUILayout.FloatField("Min Torque", targetScript.torqueA);
                    targetScript.torqueB = EditorGUILayout.FloatField("Max Torque", targetScript.torqueB);

                    GUILayout.Space(5);

                    targetScript.torqueCurve = EditorGUILayout.CurveField("Torque Curve", targetScript.torqueCurve);
                }
            
            }


            GUILayout.Space(5);


            EditorGUILayout.LabelField("Conditions", EditorStyles.boldLabel);

            if(targetScript.GetComponent<Collider>() != null) {
                if (targetScript.GetComponent<Collider>().isTrigger){
                    EditorGUILayout.HelpBox("Using collider attached to this script to enable/disable this external force", MessageType.None);
                } else {
                    EditorGUILayout.HelpBox("Your collider is not set to be a trigger, this will have no affect on this force", MessageType.Warning);
                }
            } else {
                EditorGUILayout.HelpBox("Operating in all open space. Add a collider (trigger) to mark an area where this force applies", MessageType.None);
            }

            GUILayout.Space(5);

            targetScript.minAltitude = EditorGUILayout.FloatField("Min Altitude", targetScript.minAltitude);
            targetScript.maxAltitude = EditorGUILayout.FloatField("Max Altitude", targetScript.maxAltitude);

            GUILayout.Space(5);

            targetScript.minMPH = EditorGUILayout.FloatField("Min MPH", targetScript.minMPH);
            targetScript.maxMPH = EditorGUILayout.FloatField("Max MPH", targetScript.maxMPH);


            GUILayout.Space(5);

            EditorGUILayout.LabelField("Information", EditorStyles.boldLabel);

            targetScript.visualizeForces = GUILayout.Toggle(targetScript.visualizeForces, "Visualize Forces");

            if (GUI.changed) {
                 EditorUtility.SetDirty(targetScript);
            }
        }
    }
}
