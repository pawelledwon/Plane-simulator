using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePlaneController {

    [RequireComponent(typeof(AirplaneAerodynamics))]
    [System.Serializable]
    public class AirplaneController : AirplaneRigidbodyController {

        /* Private Vars */
        private AirplaneInput input;
        private AirplaneAerodynamics aerodynamics;

        /* Public Vars */
        public float weight = 700f;
        public bool isPounds = false;
        public Transform centerOfGravity;

        public List<AirplaneEngine> engines = new List<AirplaneEngine>();
        public List<AirplaneWheel> wheels = new List<AirplaneWheel>();
        public List<AirplaneLandingGear> landingGearAssemblies = new List<AirplaneLandingGear>();
        public List<AirplaneControlSurface> controlSurfaces = new List<AirplaneControlSurface>();

        const float poundsToKg = 0.453592f;

        public AirplaneAerodynamics Aero{
            get {
                return aerodynamics;
            }
        }

        public override void Start(){
            base.Start();

            if(GetComponent<AirplaneInput>() == null){
                /* Missing an input controller module, add a default */
                Debug.LogWarning("SimpleAirplaneController: Missing input module, adding default (Keyboard) module. Consider adding a specific module instead");
                gameObject.AddComponent<AirplaneInput>();
            }

            input = GetComponent<AirplaneInput>();
            aerodynamics = GetComponent<AirplaneAerodynamics>();

            float finalMass = weight;
            if(isPounds){
                finalMass = weight * poundsToKg;
            }

            if(rBody){
                rBody.mass = finalMass;
                if(centerOfGravity){
                    rBody.centerOfMass = centerOfGravity.localPosition;
                }
            }

            if(wheels != null){
                if(wheels.Count > 0){
                    foreach(AirplaneWheel wheel in wheels){
                        wheel.InitWheel();
                    }
                }
            }

            if(aerodynamics){
                aerodynamics.InitAero(rBody, input);
            }
        }

        protected override void ApplyPhysics(){
            if(input){
                ApplyEngines();
                ApplyAerodynamics();
                UpdateControlSurfaces();
                UpdateLandingGearAssemblies();
                UpdateWheels();
            } else {
                /* Input mmodule is missing, user may be hotswapping */
                RebindInputModule();
            }
        }

        void ApplyEngines(){
            if(engines != null){
                if(engines.Count > 0){
                    foreach(AirplaneEngine engine in engines){
                        rBody.AddForce(engine.CalculateForce(input.StickyThrottle));
                        engine.CheckCutoffSwitch(input.EngineCutoff);
                    }
                }
            }
        }

        void ApplyAerodynamics(){
            if(aerodynamics){
                aerodynamics.UpdateAero();
            }
        }

        void UpdateControlSurfaces(){
            if(controlSurfaces.Count > 0){
                foreach(AirplaneControlSurface controlSurface in controlSurfaces){
                    controlSurface.UpdateControlSurface(input);
                }
            }
        }

        void UpdateLandingGearAssemblies(){
            if(landingGearAssemblies.Count > 0){
                foreach(AirplaneLandingGear landingGear in landingGearAssemblies)
                {
                    landingGear.UpdateLandingAngle(input);
                }
            }
        }

        void UpdateWheels(){
            if(wheels != null){
                if(wheels.Count > 0){
                    foreach(AirplaneWheel wheel in wheels){
                        wheel.UpdateWheel(input);
                    }
                }
            }
        }

        void RebindInputModule() {
            if (GetComponent<AirplaneInput>() != null){
                input = GetComponent<AirplaneInput>();
                if (aerodynamics) {
                    aerodynamics.InitAero(rBody, input);
                }
            }
        }
    }
}
