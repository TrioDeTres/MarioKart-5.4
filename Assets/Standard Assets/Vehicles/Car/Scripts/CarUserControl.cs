using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car;

        private void Awake()
        {
            m_Car = GetComponent<CarController>();
        }


        private void FixedUpdate()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            float handbrake = Input.GetAxis("Jump");

            m_Car.Move(h, v, v, handbrake);
        }
    }
}
