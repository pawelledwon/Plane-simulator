using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SimplePlaneController {
    public class AirplaneInformation : MonoBehaviour {

        public bool showHelperGizmos = true;

        private AirplaneController controller;

        private float meanSeaLevel;
        private float aboveGroundLevel;
        private float averageRMP;
        private float averageFuel;
        private float averageFuelNormalized;
        private float milesPerHour;
        private float knotsPerHour;
        private float kmPerHour;

        private float rollAngle;
        private float pitchAngle;

        /* Constants */
        const float metersToFeet = 3.28084f;

        /* Properties */
        public float MSL{
            get {
                return meanSeaLevel;
            }
        }

        public float AGL{
            get {
                return aboveGroundLevel;
            }
        }

        public float RPM{
            get {
                return averageRMP;
            }
        }

        public float Fuel{
            get {
                return averageFuel;
            }
        }

        public float FuelNormalized{
            get {
                return averageFuelNormalized;
            }
        }

        public float MPH{
            get {
                return milesPerHour;
            }
        }

        public float Knots{
            get {
                return knotsPerHour;
            }
        }

        public float KPH { 
            get {
                return kmPerHour;
            }
        }

        public float RollAngle { 
            get {
                return rollAngle;
            }
        }

        public float PitchAngle { 
            get {
                return pitchAngle;
            }
        }

        /* Methods */
        void Start(){
            if(transform.GetComponent<AirplaneController>() != null){
                controller = transform.GetComponent<AirplaneController>();
            }
        }

        void FixedUpdate(){
            if(controller){
                UpdateInformation();
            }
        }

        void OnDrawGizmos(){
            if (showHelperGizmos){
                AirplaneController gizmoController = transform.GetComponent<AirplaneController>();
                if (gizmoController != null) {
                    if(gizmoController.centerOfGravity != null){
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawWireSphere(gizmoController.centerOfGravity.position, 0.1f);
                        Gizmos.DrawLine(gizmoController.centerOfGravity.position + gizmoController.centerOfGravity.right * 0.2f, gizmoController.centerOfGravity.position + -gizmoController.centerOfGravity.right * 0.2f);
                        Gizmos.DrawLine(gizmoController.centerOfGravity.position + gizmoController.centerOfGravity.up * 0.2f, gizmoController.centerOfGravity.position + -gizmoController.centerOfGravity.up * 0.2f);
                        Gizmos.DrawLine(gizmoController.centerOfGravity.position + gizmoController.centerOfGravity.forward * 0.2f, gizmoController.centerOfGravity.position + -gizmoController.centerOfGravity.forward * 0.2f);
                    }

                    if (gizmoController.engines != null){
                        if(gizmoController.engines.Count > 0){
                            foreach(AirplaneEngine engine in gizmoController.engines) {
                                Gizmos.color = Color.cyan;
                                Gizmos.DrawLine(engine.transform.position, engine.transform.position + engine.transform.forward * 5f);
                            }
                        }
                    }
                }
            }
        }

        void UpdateInformation(){
            meanSeaLevel = transform.position.y * metersToFeet;

            RaycastHit hit;
            if(Physics.Raycast(transform.position, Vector3.down, out hit)){
                aboveGroundLevel = (transform.position.y - hit.point.y) * metersToFeet;
            }

            averageRMP = CalculateAvgRPM();
            averageFuel = CalculateAvgFuel(false);
            averageFuelNormalized = CalculateAvgFuel(true);

            if(controller.Aero){
                milesPerHour = controller.Aero.MPH;
                knotsPerHour = controller.Aero.Knots;
                kmPerHour = controller.Aero.KPH;

                rollAngle = controller.Aero.RollAngle;
                pitchAngle = controller.Aero.PitchAngle;
            }
        }

        float CalculateAvgRPM(){
            float avgRPM = 0;
            if(controller.engines.Count > 0){
                foreach(AirplaneEngine engine in controller.engines){
                    avgRPM += engine.RPM;
                }

                avgRPM = avgRPM / controller.engines.Count;
            }
            return avgRPM;
        }

        float CalculateAvgFuel(bool normalized){
            float avgFuel = 0;
            int fuelTanks = 0;
            if(controller.engines.Count > 0){
                foreach(AirplaneEngine engine in controller.engines){
                    if(engine.fuelTank){
                        if(normalized){
                            avgFuel += engine.fuelTank.NormalizedFuel;
                        } else{
                            avgFuel += engine.fuelTank.CurrentFuel;
                        }
                        fuelTanks++;
                    }
                }

                if(fuelTanks > 0){
                    avgFuel = avgFuel / fuelTanks;
                }
            }
            return avgFuel;
        }
    }
}
