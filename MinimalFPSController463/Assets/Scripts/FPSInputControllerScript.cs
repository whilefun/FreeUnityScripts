using UnityEngine;
using System.Collections;

//
// Ported to C# and modified by Richard Walsh
// May 30, 2016
// http://whilefun.com/
//

[RequireComponent(typeof(CharacterMotorScript))]

public class FPSInputControllerScript : MonoBehaviour {

	private CharacterMotorScript myCharacteraMotor = null;
	private Vector3 directionVector = Vector3.zero;

	void Awake(){

		myCharacteraMotor = gameObject.GetComponent<CharacterMotorScript>();

	}

	void Start(){
	
	}
	
	void Update(){

		directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		if(directionVector != Vector3.zero){

			// Get the length of the directon vector and then normalize it
			// Dividing by the length is cheaper than normalizing when we already have the length anyway
			float directionLength = directionVector.magnitude;
			directionVector = directionVector/directionLength;
			
			// Make sure the length is no bigger than 1
			directionLength = Mathf.Min(1.0f, directionLength);
			
			// Make the input vector more sensitive towards the extremes and less sensitive in the middle
			// This makes it easier to control slow speeds when using analog sticks
			directionLength = directionLength * directionLength;
			
			// Multiply the normalized direction vector by the modified length
			directionVector = directionVector * directionLength;

		}

		// Apply the direction to the CharacterMotor
		myCharacteraMotor.setInputMoveDirection(transform.rotation * directionVector);
		myCharacteraMotor.setJumpInput(Input.GetButton("Jump"));

		//if(Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown("Crouch")){
		if(Input.GetButtonDown("Crouch")){
			myCharacteraMotor.toggleCrouch();
		}

		//myCharacteraMotor.setRunning(Input.GetKey(KeyCode.LeftShift));
		myCharacteraMotor.setRunning(Input.GetButton("Run"));

	}

}
