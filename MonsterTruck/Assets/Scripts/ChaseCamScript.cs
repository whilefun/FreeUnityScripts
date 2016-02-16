using UnityEngine;
using System.Collections;

public class ChaseCamScript : MonoBehaviour {

	//private Vector3 startDistanceFromChaseTarget = Vector3.zero;

	//public GameObject chaseTarget;

	//public float maxDistanceFromTarget = 2.0f;

	// Use this for initialization
	void Start () {
	
		// remember how far away we are from chase target
		//startDistanceFromChaseTarget.x = transform.position.x - chaseTarget.transform.position.x;
		//startDistanceFromChaseTarget.y = transform.position.y - chaseTarget.transform.position.y;
		//startDistanceFromChaseTarget.z = transform.position.z - chaseTarget.transform.position.z;

	}
	
	// Update is called once per frame
	void Update () {

		/*
		if(Vector3.Distance(transform.position, chaseTarget.transform.position) >= maxDistanceFromTarget){
			transform.position = Vector3.Lerp(transform.position, chaseTarget.transform.position, 0.5f);
		}
		*/

	}

}
