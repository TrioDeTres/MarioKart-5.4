using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car;

        public bool isLocal;
        public bool hasControl;

        private void Awake()
        {
            m_Car = GetComponent<CarController>();
        }

        private void FixedUpdate()
        {
            if (isLocal && hasControl)
            {
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");

                float handbrake = Input.GetAxis("Jump");

                m_Car.Move(h, v, v, handbrake);
            }
            else if (isLocal && !hasControl)
                m_Car.Move(0f, 0f, 0f, 1f);
        }
    }
}
