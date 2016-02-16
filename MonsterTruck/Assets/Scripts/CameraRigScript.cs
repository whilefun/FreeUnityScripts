using UnityEngine;
using System.Collections;

public class CameraRigScript : MonoBehaviour {

	public GameObject chaseTarget;

	public float lerpFactor;

	public float minXRot = 340.0f;
	public float maxXRot = 5.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
	
		transform.position = Vector3.Lerp(transform.position, chaseTarget.transform.position, lerpFactor);

		transform.rotation = Quaternion.Lerp(transform.rotation, chaseTarget.transform.rotation, lerpFactor);

		// Clamp rotation to not face into the sky or into the ground

		Vector3 clampedRotation = transform.rotation.eulerAngles;

		//Debug.Log("clampX=" + clampedRotation.x);

		// Lock x and z?
		clampedRotation.x = 0.0f;
		clampedRotation.z = 0.0f;
		transform.rotation = Quaternion.Euler(clampedRotation);


	}

}
