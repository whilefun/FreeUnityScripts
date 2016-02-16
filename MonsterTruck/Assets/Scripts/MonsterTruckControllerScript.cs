using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo {
	
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	// is this wheel attached to motor?
	public bool hasMotor;
	// does this wheel apply steer angle?
	public bool hasSteering;
	
}

public class MonsterTruckControllerScript : MonoBehaviour {

	// the information about each individual axle
	public List<AxleInfo> axleInfos;
	// maximum torque the motor can apply to wheel
	public float maxMotorTorque;
	// maximum steer angle the wheel can have
	public float maxSteeringAngle;

	public float downForce;

	private float motorTorque = 0.0f;
	private float steeringAngle = 0.0f;

	private Vector3 homePosition;
	private Quaternion homeRotation;

	public void Start(){
		homePosition = transform.position;
		homeRotation = transform.rotation;
	}

	public void Update(){

		if(Input.GetKeyUp(KeyCode.R)){

			transform.position = homePosition;
			transform.rotation = homeRotation;
			motorTorque = 0.0f;
			steeringAngle = 0.0f;
			gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

		}

	}

	public void FixedUpdate(){

		motorTorque = maxMotorTorque * Input.GetAxis("Vertical");
		steeringAngle = maxSteeringAngle * Input.GetAxis("Horizontal");
		
		foreach(AxleInfo axleInfo in axleInfos){

			if(axleInfo.hasSteering){

				axleInfo.leftWheel.steerAngle = steeringAngle;
				axleInfo.rightWheel.steerAngle = steeringAngle;

			}
			
			if(axleInfo.hasMotor) {

				axleInfo.leftWheel.motorTorque = motorTorque;
				axleInfo.rightWheel.motorTorque = motorTorque;

			}

			// Only apply visuals to wheels with steering
			ApplyLocalPositionToVisuals(axleInfo.leftWheel);
			ApplyLocalPositionToVisuals(axleInfo.rightWheel);

		}

		AddDownForce();

	}

	public void ApplyLocalPositionToVisuals(WheelCollider collider){

		if(collider.transform.childCount == 0){
			return;
		}
		
		Transform visualWheel = collider.transform.GetChild(0);
		
		Vector3 position;// = collider.transform.position;
		Quaternion rotation;// = collider.transform.rotation;
		collider.GetWorldPose(out position, out rotation);

		visualWheel.transform.position = position;
		visualWheel.transform.rotation = rotation;
		// Note: since we're hacking this together with z=90 rotated cylinders, rotate by 90 for now
		visualWheel.transform.Rotate(new Vector3(0.0f,0.0f,90.0f));

	}

	private void AddDownForce(){

		//TODO: make downforce configurable based on spoilers or whatever

		//m_WheelColliders[0].attachedRigidbody.AddForce(-transform.up*m_Downforce*m_WheelColliders[0].attachedRigidbody.velocity.magnitude);
		foreach(AxleInfo axleInfo in axleInfos){

			axleInfo.leftWheel.attachedRigidbody.AddForce(-transform.up*downForce*axleInfo.leftWheel.attachedRigidbody.velocity.magnitude);
			axleInfo.rightWheel.attachedRigidbody.AddForce(-transform.up*downForce*axleInfo.rightWheel.attachedRigidbody.velocity.magnitude);

		}

	}

	void OnGUI(){

		GUI.contentColor = Color.black;
		GUI.Label (new Rect (2, 2, 256, 64), "motor=" + motorTorque + ",steerAngle=" + steeringAngle);

	}

}


