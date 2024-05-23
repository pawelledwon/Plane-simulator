using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using SimplePlaneController;

public class DashboardInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text kphText;
    [SerializeField] private TMP_Text mslText;
    [SerializeField] private TMP_Text aglText;
    [SerializeField] private TMP_Text rpmText;
    [SerializeField] private TMP_Text fuelText;
    [SerializeField] private TMP_Text rollText;
    [SerializeField] private TMP_Text pitchText;

    [SerializeField] private AirplaneInformation airplaneInformation;

    void Start()
    {
        if (airplaneInformation == null)
        {
            // Try to find AirplaneInformation component on the same GameObject
            airplaneInformation = GetComponent<AirplaneInformation>();

            // If not found, you may want to log a warning or error
            if (airplaneInformation == null)
            {
                Debug.LogError("AirplaneInformation component not found on the same GameObject.");
            }
        }
    }

    void Update()
    {
        if (airplaneInformation != null)
        {
            kphText.SetText("KPH: " + airplaneInformation.KPH.ToString("F1"));
            mslText.SetText("MSL (Sea Lvl): " + airplaneInformation.MSL.ToString("F1") + " feet");
            aglText.SetText("AGL (Ground Lvl): " + airplaneInformation.AGL.ToString("F1") + " feet");
            rpmText.SetText("RPM: " + airplaneInformation.RPM.ToString("F1"));
            fuelText.SetText("Fuel: " + airplaneInformation.FuelNormalized.ToString("P1")); // Display as percentage
            rollText.SetText("Roll angle: " + airplaneInformation.RollAngle.ToString("F1") + "°");
            pitchText.SetText("Pitch angle: " + airplaneInformation.PitchAngle.ToString("F1") + "°");
        }
        else {
            kphText.SetText("KPH: N/A");
            mslText.SetText("MSL (Sea Lvl): N/A");
            aglText.SetText("AGL (Ground Lvl): N/A");
            rpmText.SetText("RPM: N/A");
            fuelText.SetText("Fuel %: N/A");
            rollText.SetText("Roll angle: N/A");
            pitchText.SetText("Pitch angle: N/A");
        }
    }
}
