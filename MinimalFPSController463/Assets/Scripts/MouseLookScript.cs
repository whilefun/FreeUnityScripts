using UnityEngine;
using System.Collections;

//
// Ported to C# and modified by Richard Walsh
// May 30, 2016
// http://whilefun.com/
//

public class MouseLookScript : MonoBehaviour {

	[Tooltip("Set this to False to use gamepad analog stick for mouselook instead of the mouse")]
	public bool useMouseForLook = true;
	[Tooltip("Set this to True to flip gamepad Y Axis")]
	public bool flipGamepadYAxis = true;

	[SerializeField]
	private float sensitivityX = 10.0f;
	[SerializeField]
	private float sensitivityY = 10.0f;

	//private float minimumX = -360.0f;
	//private float maximumX = 360.0f;

	// Min/max pitch (90 would be straight up and down)
	private float minimumY = -60.0f;
	private float maximumY = 60.0f;
	
	private float rotationY = 0.0f;

	// If set to false, mouse look will be disabled
	[SerializeField]
	private bool canLook = true;

	void Start(){
		
		// Make the rigid body not change rotation
		if(GetComponent<Rigidbody>()){
			GetComponent<Rigidbody>().freezeRotation = true;
		}
		
	}

	void Update(){

		if(canLook){

			if(useMouseForLook){

				// Rotate player transform horizontally
				transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
				
				// And pitch the camera within the bounds specified
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				Camera.main.transform.localEulerAngles = new Vector3(-rotationY, Camera.main.transform.localEulerAngles.y, 0);

			}else{

				// Use gamepad instead
				transform.Rotate(0, Input.GetAxis("Gamepad Look X") * sensitivityX, 0);

				if(flipGamepadYAxis){
					rotationY -= Input.GetAxis("Gamepad Look Y") * sensitivityY;
				}else{
					rotationY += Input.GetAxis("Gamepad Look Y") * sensitivityY;
				}

				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				Camera.main.transform.localEulerAngles = new Vector3(-rotationY, Camera.main.transform.localEulerAngles.y, 0);

			}

		}
	
	}

	public void enableLook(){
		canLook = true;
	}

	public void disableLook(){
		canLook = false;
	}

	public bool canCurrentlyLook(){
		return canLook;
	}

	public void setMouseSensitivity(float xSensitivity, float ySensitivity){
		sensitivityX = xSensitivity;
		sensitivityY = ySensitivity;
	}

	// Returns Vector2 with x and y sensitivity as the X/Y values of the Vector
	public Vector2 getMouseSensitivity(){
		return new Vector2(sensitivityX, sensitivityY);
	}
	


}
