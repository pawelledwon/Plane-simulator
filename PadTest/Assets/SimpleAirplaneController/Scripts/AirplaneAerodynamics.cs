using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePlaneController {
    public class AirplaneAerodynamics : MonoBehaviour {

        public float speed;

        public float maxSpeed = 120f;
        public bool maxSpeedIsMPH = true;

        /* Will be replaced by the calculated max speed */
        public float maxMPH = 120f;

        public float maxLiftPower = 500f;
        public AnimationCurve liftCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public float flapLiftPower = 100f;

        public float dragFactor = 0.01f;
        public float flapDragFactor = 0.005f;

        public float pitchSpeed = 1000f;
        public float rollSpeed = 1000f;
        public float yawSpeed = 1000f;
        public AnimationCurve controlSurfaceEfficiency = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);


        public float rBodyLerpSpeed = 0.03f;

        public bool groundEffectEnabled = true;
        public float groundEffectDistance = 3f;
        public float groundEffectLiftForce = 100f;
        public float groundEffectMaxSpeed = 15f;

        public bool arcadeRoll = false;

        private Rigidbody rBody;
        private AirplaneInput input;
        private float mph;
        private float startDrag;
        private float startAngularDrag;
        private float maxMetersPerSecond;
        private float normalizedMPH;
        private float angleOfAttack;
        private float pitchAngle;
        private float rollAngle;

        private float csEfficiencyValue;

        const float metersToMiles = 2.23694f;
        const float milesToKnots = 0.868976f;
        const float kmToMiles = 0.62137119f;
        const float milesToKm = 1.609344f;

        public float MPH{
            get {
                return mph;
            }
        }

        public float KPH{
            get {
                return mph * milesToKm;
            }
        }

        public float Knots{
            get {
                return mph * milesToKnots;
            }
        }

        public float RollAngle{
            get{
                return rollAngle;
            }
        }

        public float PitchAngle{
            get{
                return pitchAngle;
            }
        }

        public void InitAero(Rigidbody rb, AirplaneInput currentInput){
            if (maxSpeedIsMPH){
                maxMPH = maxSpeed;
            } else {
                maxMPH = maxSpeed * kmToMiles;
            }

            rBody = rb;
            input = currentInput;
            startDrag = rBody.drag;
            startAngularDrag = rBody.angularDrag;

            maxMetersPerSecond = maxMPH / metersToMiles;
        }

        public void UpdateAero(){
            if(rBody){
                CalculateSpeed();
                CalculateLift();
                CalculateDrag();

                CalculateControlEfficiency();

                ApplyPitch();
                ApplyRoll();
                ApplyYaw();

                if (!arcadeRoll){
                    /* All forces apply, arcade mode disabled */
                    ApplyBanking();
                }

                HandleRigibodyTransform();

                if(groundEffectEnabled){
                    ApplyGroundEffect();
                }
            }
        }

        void CalculateSpeed(){
            Vector3 localVelocity = transform.InverseTransformDirection(rBody.velocity);
            speed = Mathf.Max(0f, localVelocity.z);
            speed = Mathf.Clamp(speed, 0f, maxMetersPerSecond);

            mph = speed * metersToMiles;
            mph = Mathf.Clamp(mph, 0f, maxMPH);
            normalizedMPH = Mathf.InverseLerp(0f, maxMPH, mph);
        }

        void CalculateLift(){
            angleOfAttack = Vector3.Dot(rBody.velocity.normalized, transform.forward);
            angleOfAttack *= angleOfAttack;

            Vector3 liftDir = transform.up;
            float liftPower = liftCurve.Evaluate(normalizedMPH) * maxLiftPower;

            float flapFinalLiftPower = flapLiftPower * input.FlapsNormalized;

            Vector3 finalLift = liftDir * (liftPower + flapFinalLiftPower) * angleOfAttack;
            rBody.AddForce(finalLift);

        }

        void CalculateDrag(){
            float dragSpeed = speed * dragFactor;
            float flapDrag = input.Flaps * flapDragFactor;
            float finalDrag = startDrag + dragSpeed + flapDrag;

            rBody.drag = finalDrag;
            rBody.angularDrag = startAngularDrag * speed;
        }

        void CalculateControlEfficiency(){
            csEfficiencyValue = controlSurfaceEfficiency.Evaluate(normalizedMPH);
        }

        void ApplyPitch(){
            Vector3 flatForward = transform.forward;
            flatForward.y = 0;
            flatForward = flatForward.normalized;
            pitchAngle = Vector3.Angle(transform.forward, flatForward);

            Vector3 pitchTorque = input.Pitch * pitchSpeed * transform.right * csEfficiencyValue;
            rBody.AddTorque(pitchTorque);
        }

        void ApplyRoll(){
            Vector3 flatRight = transform.right;
            flatRight.y = 0;
            flatRight = flatRight.normalized;
            rollAngle = Vector3.SignedAngle(transform.right, flatRight, transform.forward);
            
            Vector3 rollTorque = input.Roll * rollSpeed * transform.forward * csEfficiencyValue;
            rBody.AddTorque(rollTorque);
        }

        void ApplyYaw(){
            Vector3 yawTorque = -input.Yaw * yawSpeed * transform.up * csEfficiencyValue;
            rBody.AddTorque(yawTorque);
        }

        void ApplyBanking(){
            float bankSide = Mathf.InverseLerp(-90f, 90f, rollAngle);
            float bankAmount = Mathf.Lerp(-1f, 1f, bankSide);

            Vector3 bankTorque = bankAmount * rollSpeed * transform.up;
            rBody.AddTorque(bankTorque);
        }

        void HandleRigibodyTransform(){
            if(rBody.velocity.magnitude > 1f){
                Vector3 velocityCorrection = Vector3.Lerp(rBody.velocity, transform.forward * speed, speed * angleOfAttack * Time.deltaTime * rBodyLerpSpeed);
                rBody.velocity = velocityCorrection;

                Quaternion rotationCorrection = Quaternion.Slerp(rBody.rotation, Quaternion.LookRotation(rBody.velocity.normalized, transform.up), Time.deltaTime * rBodyLerpSpeed);
                rBody.MoveRotation(rotationCorrection);
            }
        }

        void ApplyGroundEffect(){
            RaycastHit hit;
            if(Physics.Raycast(transform.position, Vector3.down, out hit)){
                if(hit.distance < groundEffectDistance){
                    float velocity = rBody.velocity.magnitude;
                    float normalizedSpeed = velocity / groundEffectMaxSpeed;
                    normalizedSpeed = Mathf.Clamp01(normalizedSpeed);

                    float distance = groundEffectDistance - hit.distance;
                    float finalForce = groundEffectLiftForce * distance * normalizedSpeed;
                    rBody.AddForce(Vector3.up * finalForce);
                }
            }
        }
    }
}
