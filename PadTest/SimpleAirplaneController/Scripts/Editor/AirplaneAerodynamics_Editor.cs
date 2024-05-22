using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SimplePlaneController {
    [CustomEditor(typeof(AirplaneAerodynamics))]
    public class AirplaneAerodynamics_Editor : Editor {
        private AirplaneAerodynamics targetScript;

        void OnEnable(){
            targetScript = (AirplaneAerodynamics)target;
        }

        public override void OnInspectorGUI(){
            EditorGUILayout.LabelField("Lift Settings:", EditorStyles.boldLabel);
            targetScript.maxSpeed = EditorGUILayout.FloatField("Max Speed (" + (targetScript.maxSpeedIsMPH ? "MPH" : "KPH") + ")", targetScript.maxSpeed);
            targetScript.maxSpeedIsMPH = EditorGUILayout.Toggle("Is MPH", targetScript.maxSpeedIsMPH);

            GUILayout.Space(5);


            targetScript.maxLiftPower = EditorGUILayout.FloatField("Max Lift Power", targetScript.maxLiftPower);
            targetScript.rBodyLerpSpeed = EditorGUILayout.FloatField("Lerp Speed Multiplier", targetScript.rBodyLerpSpeed);
            targetScript.liftCurve = EditorGUILayout.CurveField("Lift Curve", targetScript.liftCurve);
            targetScript.flapLiftPower = EditorGUILayout.FloatField("Flap Lift Power", targetScript.flapLiftPower);



            EditorGUILayout.LabelField("Drag Settings:", EditorStyles.boldLabel);
            targetScript.dragFactor = EditorGUILayout.FloatField("Drag Factor", targetScript.dragFactor);
            targetScript.flapDragFactor = EditorGUILayout.FloatField("Flap Drag Factor", targetScript.flapDragFactor);

            EditorGUILayout.LabelField("Control Settings:", EditorStyles.boldLabel);
            targetScript.pitchSpeed = EditorGUILayout.FloatField("Pitch Speed", targetScript.pitchSpeed);
            targetScript.rollSpeed = EditorGUILayout.FloatField("Roll Speed", targetScript.rollSpeed);
            targetScript.yawSpeed = EditorGUILayout.FloatField("Yaw Speed", targetScript.yawSpeed);
            targetScript.controlSurfaceEfficiency = EditorGUILayout.CurveField("Control Surface Efficiency", targetScript.controlSurfaceEfficiency);

            targetScript.arcadeRoll = EditorGUILayout.Toggle("Arcade Roll", targetScript.arcadeRoll);

            if (targetScript.arcadeRoll){
                EditorGUILayout.HelpBox("Arcade roll mode will disable banking effects, but result in a smoother roll", MessageType.Info);
            }

            GUILayout.Space(5);

            EditorGUILayout.LabelField("Ground Effect Settings:", EditorStyles.boldLabel);
            targetScript.groundEffectEnabled = EditorGUILayout.Toggle("Enabled", targetScript.groundEffectEnabled);
            targetScript.groundEffectDistance = EditorGUILayout.FloatField("Distance (Wing Length)", targetScript.groundEffectDistance);
            targetScript.groundEffectLiftForce = EditorGUILayout.FloatField("Lift Force", targetScript.groundEffectLiftForce);
            targetScript.groundEffectMaxSpeed = EditorGUILayout.FloatField("Max Speed", targetScript.groundEffectMaxSpeed);

            if (GUI.changed) {
                EditorUtility.SetDirty(targetScript);
            }

            GUILayout.Space(5);

            EditorGUILayout.HelpBox("Forces are applied in N (Newtons)\nTorque is applied as Nm (Newton-metres)", MessageType.None);

        }

    }
}
