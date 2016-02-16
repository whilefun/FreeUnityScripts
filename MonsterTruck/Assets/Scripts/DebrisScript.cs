using UnityEngine;
using System.Collections;

public class DebrisScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		transform.rotation = Quaternion.Euler(new Vector3(Random.Range(0,359),Random.Range(0,359),Random.Range(0,359)));

	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
