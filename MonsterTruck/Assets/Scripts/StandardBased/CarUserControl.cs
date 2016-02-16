using System;
using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use


        private void Awake(){
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        private void FixedUpdate(){

			if(Input.GetKeyDown(KeyCode.R)){
				m_Car.resetToHome();
			}

            // pass the input to the car!
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            float handbrake = Input.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);



        }

    }

}
