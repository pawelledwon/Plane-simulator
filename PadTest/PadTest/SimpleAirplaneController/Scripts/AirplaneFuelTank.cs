using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePlaneController {
    public class AirplaneFuelTank : MonoBehaviour{
        /* Public Vars */
        [Header("Fuel Settings")]
        [Tooltip("Gallons in fuel tank")]
        public float fuelCapacity = 26f;
        [Tooltip("Gallons per hour")]
        public float fuelBurnRate = 6.1f;

        /* Private Vars */
        private float currentFuel;
        private float normalizedFuel;

        /* Properties */
        public float CurrentFuel{
            get {
                return currentFuel;
            }
        }

        public float NormalizedFuel{
            get {
                return normalizedFuel;
            }
        }

        public void InitFuel(){
            currentFuel = fuelCapacity;
        }

        public void AddFuel(float fuelAmount){
            currentFuel += fuelAmount;
            currentFuel = Mathf.Clamp(currentFuel, 0f, fuelCapacity);
        }

        public void UpdateFuel(float throttle){
            float currentBurn = ((fuelBurnRate * throttle) / 3600f) * Time.deltaTime;
            currentFuel -= currentBurn;
            currentFuel = Mathf.Clamp(currentFuel, 0f, fuelCapacity);
            normalizedFuel = currentFuel / fuelCapacity;
        }
    }
}
