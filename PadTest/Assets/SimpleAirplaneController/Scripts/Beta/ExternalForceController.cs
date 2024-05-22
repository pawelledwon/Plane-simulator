using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePlaneController{

    public enum ExternalForceType {
        Constant,
        Variable
    }

    public class ExternalForceController : MonoBehaviour {
        public AirplaneAerodynamics planeTarget;
        public ExternalForceType forceType = ExternalForceType.Constant;
        public ForceMode forceMode = ForceMode.Force;
        public bool simulateTorque = true;

        public Vector3 directionA = Vector3.zero;
        public Vector3 directionB = Vector3.zero;

        public float forceA = 3000f;
        public float forceB = 5000f;

        public AnimationCurve forceCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        public float curveDurationA = 15f;
        public float curveDurationB = 20f;
        public float frequencyA = 10f;
        public float frequencyB = 20f;

        public float minAltitude = float.NegativeInfinity;
        public float maxAltitude = float.PositiveInfinity;

        public float minMPH = float.NegativeInfinity;
        public float maxMPH = float.PositiveInfinity;

        public bool visualizeForces = false;
        public float torqueA = 1500f;
        public float torqueB = 2000f;

        public AnimationCurve torqueCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        private float lastTime = 0f;
        private bool triggerActive = false;

        private Vector3 dir;
        private float force;
        private float torque;
        private float frequency;
        private float curveDuration;

        private Rigidbody rBody;


        public void Start(){
            if(planeTarget.GetComponent<AirplaneInformation>() == null && (!float.IsInfinity(minAltitude) || !float.IsInfinity(maxAltitude) || !float.IsInfinity(minMPH) || !float.IsInfinity(maxAltitude))){
                Debug.LogWarning("Your plane is missing the information module, this means you may be unable to set up coniditionals like speed and altitude");
            } else {
                if(planeTarget.GetComponent<Rigidbody>() != null) {
                    rBody = planeTarget.GetComponent<Rigidbody>();
                }
            }
            UpdateInputs();
        }

        public void FixedUpdate() {
            ApplyForce();
        }

        public void OnTriggerEnter(Collider other){
            if(other.GetComponent<AirplaneAerodynamics>() == planeTarget){
                triggerActive = true;
            }
        }

        public void OnTriggerExit(Collider other){
            if(other.GetComponent<AirplaneAerodynamics>() == planeTarget){
                triggerActive = false;
            }
        }

        public void OnDrawGizmos(){
            if (visualizeForces && CanApplyForce()){
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(planeTarget.transform.position, planeTarget.transform.position + (dir * 10f));
            }
        }

        private void ApplyForce() {
            if (CanApplyForce()){
                switch (forceType) {
                    case ExternalForceType.Constant:
                        ApplyConstantForce();
                        break;
                    case ExternalForceType.Variable:
                        ApplyVariableForce();
                        break;
                }

                ApplyTorque();
            }
        }

        private void ApplyTorque(){
            if (simulateTorque) {
                rBody.AddTorque(GetCrossProductTorque() * torque);
            }
        }

        private void ApplyConstantForce() {
            force = forceA;
            torque = torqueA;
            rBody.AddForce(dir * force, forceMode);
        }

        private void ApplyVariableForce() {
            force = GetVariableForceOverTime();
            torque = GetVariableTorqueOverTime();
            rBody.AddForce(dir * force, forceMode);
        }

        private void UpdateInputs() {
            dir = (forceType == ExternalForceType.Constant) ? directionA : GetRandomDirection(directionA, directionB);
            frequency = Random.Range(frequencyA, frequencyB);
            curveDuration = Random.Range(curveDurationA, curveDurationB);
        }

        private bool CanApplyForce(){
            if (AllConditionalsMet() && planeTarget != null && rBody != null) { 
                if(forceType == ExternalForceType.Variable){
                    // TODO: Run tests to determine if the variable force should be applied
                    float pointInTime = GetPointInFrequencyTime();
                    if(pointInTime >= frequency){
                        if(pointInTime <= (frequency + curveDuration)){
                            return true;
                        } else{
                            lastTime = Time.time;
                            UpdateInputs();
                        }
                    }

                    return false;

                }
                return true;
            }
            return false;
        }

        private bool AllConditionalsMet(){
            return AltitudeConiditionMet() && SpeedConditionMet() && TriggerConditionMet();
        }

        private bool AltitudeConiditionMet(){
            if (float.IsInfinity(minAltitude) && float.IsInfinity(maxAltitude)){
                //No altitude requirement
                return true;
            } else {
                if (planeTarget.GetComponent<AirplaneInformation>() != null){
                    if (planeTarget.GetComponent<AirplaneInformation>().MSL >= minAltitude && planeTarget.GetComponent<AirplaneInformation>().MSL <= maxAltitude){
                        return true;
                    }

                }
            }
            return false;
        }

        private bool SpeedConditionMet(){
            if (float.IsInfinity(minMPH) && float.IsInfinity(maxMPH)){
                //No MPH requirement
                return true;
            } else {
                if (planeTarget.GetComponent<AirplaneInformation>() != null){
                    if (planeTarget.GetComponent<AirplaneInformation>().MPH >= minMPH && planeTarget.GetComponent<AirplaneInformation>().MPH <= maxMPH){
                        return true;
                    }

                }
            }
            return false;
        }

        private bool TriggerConditionMet(){
            if(GetComponent<Collider>() != null && GetComponent<Collider>().isTrigger){
                return triggerActive;
            }
            return true; //Always met if there is no collider
        }

        private float GetVariableForceOverTime(){
            float pointInCurce = GetPointInFrequencyTime() / curveDuration;
            return Mathf.Lerp(forceA, forceB, forceCurve.Evaluate(pointInCurce));
        }

        private float GetVariableTorqueOverTime(){
            float pointInCurce = GetPointInFrequencyTime() / curveDuration;
            return Mathf.Lerp(torqueA, torqueB, torqueCurve.Evaluate(pointInCurce));
        }

        private Vector3 GetCrossProductTorque(){
            // It's hard for me to say this is correct
            // My knowledge here is letting me down a bit, but I believe this should create enough of a random 'tilt' (roll, pitch, yaw) 
            // What I will do is make this an optional additional calculation for those who want something basic for now
            // Todo: Self-study and learn more about how these forces would be simulated more realistically
            // TL;DR: This is okay for now
            return Vector3.Cross(planeTarget.transform.up, dir).normalized;
        }

        private float GetPointInFrequencyTime() {
            return Time.time - lastTime;
        }

        private Vector3 GetRandomDirection(Vector3 dA, Vector3 dB){
            return new Vector3(Random.Range(dA.x, dB.x), Random.Range(dA.y, dB.y), Random.Range(dA.z, dB.z));
        }



    }
}
