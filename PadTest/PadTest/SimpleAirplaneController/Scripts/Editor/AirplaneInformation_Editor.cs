using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SimplePlaneController {
    [CustomEditor(typeof(AirplaneInformation))]
    public class AirplaneInformation_Editor : Editor{
        private AirplaneInformation targetScript;

        void OnEnable(){
            targetScript = (AirplaneInformation)target;
        }

        public override void OnInspectorGUI(){
            string informationOut = "";
            informationOut += "MPH: " + ((int)targetScript.MPH) + "\n";
            informationOut += "Knots: " + ((int)targetScript.Knots) + "\n";
            informationOut += "KPH: " + ((int)targetScript.KPH) + "\n";
            informationOut += "\n";
            informationOut += "MSL (Sea Level): " + ((int)targetScript.MSL) + " feet\n";
            informationOut += "AGL (Ground Level): " + ((int)targetScript.AGL) + " feet\n";
            informationOut += "Average RPM: " + ((int)targetScript.RPM) + "\n";
            informationOut += "\n";
            informationOut += "Average Fuel: " + ((int)targetScript.Fuel) + " gallons\n";
            informationOut += "Fuel Percentage: " + ((int) (targetScript.FuelNormalized * 100)) + "%\n";
            informationOut += "\n";
            informationOut += "Roll Angle: " + ((int)targetScript.RollAngle) + " °\n";
            informationOut += "Pitch Angle: " + ((int)targetScript.PitchAngle) + " °\n";


            EditorGUILayout.LabelField("Information:", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(informationOut, MessageType.None);
            GUILayout.Space(5);

            targetScript.showHelperGizmos = EditorGUILayout.Toggle("Show Helper Gizmos", targetScript.showHelperGizmos);

            Repaint();

            if (GUI.changed) {
                EditorUtility.SetDirty(targetScript);
            }
        }
    }
}
