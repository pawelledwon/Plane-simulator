using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePlaneController
{
    public enum SimpleAIMode
    {
        FOLLOW,
        ROUTE
    }

    public class AirplaneInputSimpleAI : AirplaneInput
    {
        public SimpleAIMode mode = SimpleAIMode.FOLLOW;

        public Transform followTarget;
        public List<Transform> routeWaypoints = new List<Transform>();

        public float waypointHitRadius = 100f;
        public float groundRayDistance = 5f;

        public float maxRollAngle = 45f;

        public bool drawGizmos = true;

        public int flapsOverride = 0;
        public float brakeOverride = 0f;
        public bool cameraSwitchOverride = false;
        public bool engineCutoffOverride = false;
        public bool lightToggleOverride = false;
        public bool landingGearToggleOverride = false;

        private int waypointIndex = 0;
        private AirplaneAerodynamics aero;
        private Transform currentTarget;

        private Vector3 flatForward;
        private Vector3 dirToTarget;

        private RaycastHit groundRayHit;
        private bool isNearGround = false;

        public override void GetInput()
        {
            UpdateReferences();
            UpdateTarget();
            UpdateGroundRay();
            UpdateDirectionToTarget();

            pitch = CalculatePitch();
            roll = CalculateRoll();
            yaw = CalculateYaw();
            throttle = CalculateThrottle();
            
            ApplyStickyThrottle();

            /* These methods could be inherited from the delegate class, but for good measure, we will keep them seperate */
            /* In other words, not dry, but allows for AI scale separately from delegate */
            brake = brakeOverride;
            flaps = Mathf.Clamp(flapsOverride, 0, maxFlaps);

            cameraSwitch = cameraSwitchOverride;
            engineCutoff = engineCutoffOverride;
            lightToggle = lightToggleOverride;
            landingGearToggle = landingGearToggleOverride;

            DisableToggles();

            ApplyAutoBrake();
        }

        protected virtual void OnDrawGizmos()
        {
            if(currentTarget != null && drawGizmos){
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, currentTarget.position);

                Gizmos.color = isNearGround ? Color.red : Color.green;
                Gizmos.DrawLine(transform.position, transform.position + (-transform.up) * groundRayDistance);

                if(mode == SimpleAIMode.ROUTE) {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(currentTarget.position, waypointHitRadius);
                }
            }


        }

        protected virtual float CalculatePitch() {
            float sPitch = 0f;
            if(aero != null){
                float pitchAngleToTarget = Vector3.SignedAngle(dirToTarget, flatForward, transform.right);
                float currentAngle = Vector3.SignedAngle(transform.forward, flatForward, transform.right);

                if (currentAngle <= Mathf.Floor(pitchAngleToTarget)){
                    sPitch = -1f;
                } else if(currentAngle >= Mathf.Ceil(pitchAngleToTarget)){
                    sPitch = 1f;
                }

                if (isNearGround){
                    sPitch = 0f;
                }
            }
            return sPitch;
        }

        protected virtual float CalculateRoll(){
            float sRoll = 0f;
            if(aero != null) {
                float currentAngle = aero.RollAngle;

                float yawAngleToTarget = Vector3.SignedAngle(dirToTarget, transform.TransformDirection(Vector3.forward), transform.up);
                if(yawAngleToTarget < -20f && Mathf.Abs(currentAngle) < maxRollAngle) {
                    sRoll = -1f;
                } else if(yawAngleToTarget > 20f && Mathf.Abs(currentAngle) < maxRollAngle){
                    sRoll = 1f;
                } else {
                    /* Relevel the plane, and allow the yaw input to sort things */
                    if(currentAngle < -1f){
                        sRoll = -1f;
                    } else if (currentAngle > 1f) {
                        sRoll = 1f;
                    }
                }

                if (isNearGround){
                    sRoll = 0f;
                }
            }
            return sRoll;
        }

        protected virtual float CalculateYaw() { 
            float sYaw = 0f;
            if(aero != null) {
                float yawAngleToTarget = Vector3.SignedAngle(dirToTarget, transform.TransformDirection(Vector3.forward), transform.up);
                if(yawAngleToTarget >= -20f && yawAngleToTarget < -1f){
                    sYaw = -1f;
                } else if(yawAngleToTarget <= 20f && yawAngleToTarget > 1f){
                    sYaw = 1f;
                }

                if (isNearGround){
                    sYaw = 0f;
                }
            }
            return sYaw;
        }

        protected virtual float CalculateThrottle(){
            if(Vector3.Distance(transform.position, currentTarget.position) > 0.5f) {
                return 1f;
            }
            return 0f;
        }

        protected virtual void UpdateReferences() { 
            if(aero == null) { 
                if(GetComponent<AirplaneAerodynamics>() != null){
                    aero = GetComponent<AirplaneAerodynamics>();
                }
            }
        }

        protected virtual void UpdateGroundRay() {
            isNearGround = false;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out groundRayHit, groundRayDistance))            {
                isNearGround = true;
            }
        }

        protected virtual void UpdateDirectionToTarget() {
            if(currentTarget != null){
                flatForward = transform.forward;
                flatForward.y = 0;
                flatForward = flatForward.normalized;

                dirToTarget = (transform.position - currentTarget.position);
                dirToTarget = -dirToTarget.normalized;
            }
        }

        protected virtual void UpdateTarget()
        {
            if(mode == SimpleAIMode.FOLLOW){
                if (currentTarget == null){
                    currentTarget = followTarget;
                }
            } else {
                if (currentTarget == null) {
                    if(routeWaypoints.Count > 0){
                        currentTarget = routeWaypoints[0];
                        waypointIndex = 0;
                    }
                } else{
                    /* Check if the current waypoint is close enough, if so, swap to the next in the chain */
                    if(Vector3.Distance(transform.position, currentTarget.position) <= waypointHitRadius){
                        if(routeWaypoints.Count > 0){
                            if(routeWaypoints.Count > (waypointIndex + 1)) {
                                currentTarget = routeWaypoints[waypointIndex + 1];
                                waypointIndex ++;
                            } else {
                                currentTarget = routeWaypoints[0];
                                waypointIndex = 0;
                            }
                        }
                    }
                }
            }
        }

        public virtual void SetBrake(float sBrake)
        {
            sBrake = Mathf.Clamp(sBrake, 0f, 1f);
            brakeOverride = sBrake;
        }

        public virtual void SetFlaps(int sFlaps)
        {
            flapsOverride = sFlaps;
        }

        public virtual void ToggleCamera()
        {
            cameraSwitchOverride = !cameraSwitchOverride;
        }

        public virtual void ToggleEngine()
        {
            engineCutoffOverride = !engineCutoffOverride;
        }

        public virtual void ToggleLights()
        {
            lightToggleOverride = !lightToggleOverride;
        }

        public virtual void ToggleLandingGearAssembly()
        {
            landingGearToggleOverride = !landingGearToggleOverride;
        }

        public virtual void DisableToggles()
        {
            if (cameraSwitchOverride) {
                cameraSwitchOverride = false;
            }

            if (engineCutoffOverride) {
                engineCutoffOverride = false;
            }

            if (lightToggleOverride) {
                lightToggleOverride = false;
            }

            if (landingGearToggleOverride) {
                landingGearToggleOverride = false;
            }
        }

    }
}
