using UnityEngine;
using System.Collections;

public class DebugHelperScript : MonoBehaviour {

	public GameObject thePlayer;

	void Start(){
	
	}
	
	void Update(){

		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}

		if(Input.GetKeyDown(KeyCode.M)){
			thePlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.MouseLook>().enableMouseLook = !thePlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.MouseLook>().enableMouseLook;
		}

		if(Input.GetKeyDown(KeyCode.V)){
			thePlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enableMovement = !thePlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enableMovement;
		}

		if(Input.GetKeyDown(KeyCode.R)){
			thePlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enableRunning = !thePlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enableRunning;
		}


	}

	void OnGUI(){

		GUI.contentColor = Color.yellow;
		GUI.Box(new Rect(0,0,256,64),"");
		GUI.Label(new Rect(0,0,256,64),"M: Toggle MouseLook\nV: Toggle Player Movement\nR: Toggle Player Running Allowed");

	}

}
