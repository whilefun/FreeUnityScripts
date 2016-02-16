using UnityEngine;
using System.Collections;

public class BuzzerScript : MonoBehaviour {

	private Vector3 homePosition;
	private Vector3 currentRandomTargetPosition;
	public float randomOffsetAmount = 0.5f;
	public float closeEnough = 0.05f;
	public float lerpFactor = 0.1f;

	// Use this for initialization
	void Start () {
	
		homePosition = Vector3.zero;
		currentRandomTargetPosition = homePosition;

	}
	
	// Update is called once per frame
	void Update () {
	
		if(Vector3.Distance(transform.position, currentRandomTargetPosition) < closeEnough){

			//Debug.Log ("Close enough, retargeting");
			currentRandomTargetPosition = new Vector3(homePosition.x + Random.Range(-randomOffsetAmount,randomOffsetAmount),homePosition.y + Random.Range(-randomOffsetAmount,randomOffsetAmount),0.0f);

		}

		transform.position = Vector3.Lerp(transform.position, currentRandomTargetPosition, lerpFactor);

		if(Input.GetKeyDown (KeyCode.Escape)){
			Application.Quit();
		}

	}


}
