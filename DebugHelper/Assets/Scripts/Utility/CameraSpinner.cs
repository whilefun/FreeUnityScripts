using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpinner : MonoBehaviour {

    private float spinAnglePerStep = 30.0f;

	void Update ()
    {
        gameObject.transform.Rotate(transform.up, spinAnglePerStep * Time.deltaTime);
	}
}
