using UnityEngine;
using System.Collections;

//
// A simple FPS Player Controller helper script
// Just attach this to any empty game object in the same scene as the FPS Player Controller
//
// Richard Walsh
// May 30, 2016
// http://whilefun.com/
//

public class DebugHelperScript : MonoBehaviour {

	private GameObject thePlayerController;

	public bool showDebugKeysAndStates = true;

	void Awake(){

		thePlayerController = GameObject.FindGameObjectWithTag("Player");

		if(!thePlayerController){
			Debug.LogError("DebugHelperScript:: Cannot find Player!");
		}

	}

	void Start(){
	
	}
	
	void Update(){
	
		if(Input.GetKeyDown(KeyCode.Alpha1)){

			if(thePlayerController.GetComponent<CharacterMotorScript>().canCurrentlyMove()){
				thePlayerController.GetComponent<CharacterMotorScript>().disableMovement();
			}else{
				thePlayerController.GetComponent<CharacterMotorScript>().enableMovement();
			}

		}

		if(Input.GetKeyDown(KeyCode.Alpha2)){
			
			if(thePlayerController.GetComponent<CharacterMotorScript>().canCurrentlyLook()){
				thePlayerController.GetComponent<CharacterMotorScript>().disableLook();
			}else{
				thePlayerController.GetComponent<CharacterMotorScript>().enableLook();
			}
			
		}

	}

	// Debug only
	void OnGUI(){

		if(showDebugKeysAndStates){

			GUI.contentColor = Color.black;
			GUI.Box(new Rect(0,0,400,100), "");
			
			GUI.contentColor = Color.yellow;
			GUI.Label(new Rect(4,0,400,128),"1 to Toggle Movement On/Off (Enabled="+thePlayerController.GetComponent<CharacterMotorScript>().canCurrentlyMove()+")");
			GUI.Label(new Rect(4,16,400,128),"2 to Toggle Mouse Look On/Off (Enabled="+thePlayerController.GetComponent<CharacterMotorScript>().canCurrentlyLook()+")");
			GUI.Label(new Rect(4,32,400,128),"W,A,S,D or left analog stick to move");
			GUI.Label(new Rect(4,48,400,128),"Left Shift or gamepad Right Shoulder to run (Running="+thePlayerController.GetComponent<CharacterMotorScript>().isCurrentlyRunning()+")");
			GUI.Label(new Rect(4,64,400,128),"Space or gamepad 'Y' to jump");
			GUI.Label(new Rect(4,80,400,128),"C or gamepad 'A' to crouch (Crouching="+thePlayerController.GetComponent<CharacterMotorScript>().isCurrentlyCrouching()+")");
		
		}

	}

}
