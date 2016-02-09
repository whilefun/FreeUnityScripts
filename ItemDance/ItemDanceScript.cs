using UnityEngine;
using System.Collections;

//
// Item Dance
// A fun little script to make your game items stand out to the player
// Copyright 2016 While Fun Games
// http://whilefun.com
//

public class ItemDanceScript : MonoBehaviour {

	//
	// Note: All of the default values are a good sweet spot for a 1x1x1 scale default cube.
	// Take this into account when playing with the values. For example, on an object
	// that is a 10x10x10 cube or equivalent, you might want to start by making the default
	// bounce height 10 times larger (e.g. 1.5) and tweak from there.
	//

	// --- Rotation --- //
	// Inspector flag to enable or disable rotation for a gameObject
	public bool rotateObject = true;
	// Starting with 1/3rd turn around the Y axis gives a "classic" feel
	public Vector3 rotationAngles = Vector3.up * 120.0f;

	// --- Bounce --- //
	// Inspector flag to enable or disable object bouncing
	public bool bounceObject = true;
	// How far away (up/down) from original position the object will bounce (+/- this amount)
	public float bounceHeight = 0.15f;
	// How long the bounce lasts, top to bottom
	public float bounceDuration = 2.0f;
	// The softness of the bounce at the top and bottom. A value of zero is max hardness, and a value of bounceHeight is max softness (no bounce at all!)
	public float bounceSoftness = 0.05f;

	// --- Blink --- //
	// Inspector flag to enable or disable object blinking
	public bool blinkObject = true;
	// The color the object will start as
	public Color startColor = Color.white;
	// The color the object will end as
	public Color endColor = Color.yellow;
	// How fast the colors will change back and forth (Anything between 0.01 and 0.25 should be good)
	public float colorChangeSpeed = 0.025f;
	// Set this flag to True in the following cases:
	// GameObject is Empty GameObject (no Renderer), but has child objects with renderers (e.g. FBX models or something)
	// GameObject contains other child objects that you also want to blink
	public bool ApplyBlinkToChildRenderers = false;

	// Other internal stuff (I recommend leaving these alone)
	private Vector3 startPosition = Vector3.zero;
	private Vector3 bounceDestinationDown = Vector3.zero;
	private Vector3 bounceDestinationUp = Vector3.zero;
	private Vector3 targetPosition = Vector3.zero;
	private float startTime = 0.0f;
	private float bounceTime = 0.0f;
	private float colorChangeAmount = 0.0f;
	private Color currentColor = Color.white;
	private Color targetColor = Color.white;

	void Start () {

		// Warn user in this case, as they probably are not getting the results they want
		if(blinkObject && !gameObject.GetComponent<Renderer>() && !ApplyBlinkToChildRenderers){
			Debug.LogWarning("Warning: You are trying to make an object blink that has no renderer, but have not enabled 'Apply Blink To Child Renderers'!");
		}

		// Unlikely (Impossible?) case, but if the game object has no transform, we cannot rotate or bounce
		if((bounceObject || rotateObject) && !gameObject.transform){
			Debug.LogError("Warning: You are trying to make an object bounce or rotate, but it has no transform!");
		}

		startPosition = gameObject.transform.position;
		bounceDestinationUp = startPosition;
		bounceDestinationUp.y = startPosition.y - bounceHeight;
		bounceDestinationDown = startPosition;
		bounceDestinationDown.y = startPosition.y + bounceHeight;
		targetPosition = bounceDestinationUp;
		startTime = Time.time;
		currentColor = startColor;
		targetColor = endColor;

	}
	
	void Update () {

		// Rotation //
		if(rotateObject){
			
			gameObject.transform.Rotate(rotationAngles * Time.deltaTime);
			
		}

		// Bounce //
		if(bounceObject){

			bounceTime = (Time.time - startTime) / bounceDuration;
			gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPosition, bounceTime);

			if((gameObject.transform.position - bounceDestinationUp).magnitude <= bounceSoftness){

				targetPosition = bounceDestinationDown;
				startTime = Time.time;

			}else if((gameObject.transform.position - bounceDestinationDown).magnitude <= bounceSoftness){

				targetPosition = bounceDestinationUp;
				startTime = Time.time;

			}

		}

		// Blink //
		if(blinkObject){

			colorChangeAmount += colorChangeSpeed;

			if(ApplyBlinkToChildRenderers){
				
				Renderer[] childObjects = gameObject.GetComponentsInChildren<Renderer>();
				foreach (Renderer go in childObjects){
					go.material.color = Color.Lerp(currentColor, targetColor, colorChangeAmount);
				}
				
			}else{

				if(gameObject.GetComponent<Renderer>()){
					gameObject.GetComponent<Renderer>().material.color = Color.Lerp(currentColor, targetColor, colorChangeAmount);
				}

			}

			if(colorChangeAmount >= 1.0f){

				colorChangeAmount = 0.0f;

				if(targetColor == endColor){
					currentColor = endColor;
					targetColor = startColor;
				}else{
					currentColor = startColor;
					targetColor = endColor;
				}

			}

		}

	}

}
