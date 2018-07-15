using UnityEngine;

//
// Item Dance
// A fun little script to make your game items stand out to the player
// Copyright 2016 While Fun Games
// http://whilefun.com
//

public class ItemDance : MonoBehaviour {

	//
	// Note: All of the default values are a good sweet spot for a 1x1x1 scale default cube.
	//

	// --- Rotation --- //
    [SerializeField, Tooltip("If true, object will rotate")]
	private bool rotateObject = true;
    [SerializeField, Tooltip("Rotation applied per second. 120 degrees will give a 1/3rd turn per second")]
    private Vector3 rotationPerSecond = Vector3.up * 120.0f;

    // --- Bounce --- //
    [SerializeField, Tooltip("If true, object will bounce")]
    private bool bounceObject = true;
    [SerializeField, Tooltip("How quickly the object will bounce up and down. Higher value means faster bounce cycle.")]
    private float bounceSpeed = 5.0f;
    [SerializeField, Tooltip("The total delta from top to bottom. Higher scale means larger bounce distance.")]
    private float bounceScale = 0.5f;

    // --- Blink --- //
    [SerializeField, Tooltip("If true, object will 'blink'")]
    private bool blinkObject = true;
    [SerializeField, Tooltip("Start color (default White)")]
    private Color startColor = Color.white;
    [SerializeField, Tooltip("Second (blink) color. Default is Yellow.")]
    private Color endColor = Color.yellow;
    // How fast the colors will change back and forth (Anything between 0.01 and 0.25 should be good)
    [SerializeField, Tooltip("Speed factor at which colors will change. Higher factor value means more rapid color change.")]
    private float colorChangeSpeed = 1.5f;
    [SerializeField, Tooltip("If true, child renderers will also get blink applied. Covers cases where you have an empty gameobject with child meshes.")]
    private bool ApplyBlinkToChildRenderers = false;

    private Vector3 startPosition = Vector3.zero;
    private Vector3 tempPosition = Vector3.zero;
    private float colorChangeAmount = 0.0f;
	private Color currentColor = Color.white;
	private Color targetColor = Color.white;


    void Start () {

		// Warn user in this case, as they probably are not getting the results they want
		if(blinkObject && !gameObject.GetComponent<Renderer>() && !ApplyBlinkToChildRenderers){
			Debug.LogWarning("Warning: You are trying to make an object blink that has no renderer, but have not enabled 'Apply Blink To Child Renderers'!");
		}

		startPosition = gameObject.transform.position;
		currentColor = startColor;
		targetColor = endColor;

	}
	
	void Update () {

		// Rotation //
		if(rotateObject){
			
			gameObject.transform.Rotate(rotationPerSecond * Time.deltaTime);
			
		}

		// Bounce //
		if(bounceObject){

            tempPosition = startPosition;
            tempPosition.y = startPosition.y + (Mathf.Sin(Time.time * bounceSpeed) * bounceScale);
            gameObject.transform.position = tempPosition;

        }

		// Blink //
		if(blinkObject){

			colorChangeAmount += colorChangeSpeed * Time.deltaTime;

			if(ApplyBlinkToChildRenderers)
            {
				
				Renderer[] childObjects = gameObject.GetComponentsInChildren<Renderer>();

				foreach (Renderer go in childObjects)
                {
					go.material.color = Color.Lerp(currentColor, targetColor, colorChangeAmount);
				}
				
			}
            else
            {

				if(gameObject.GetComponent<Renderer>())
                {
					gameObject.GetComponent<Renderer>().material.color = Color.Lerp(currentColor, targetColor, colorChangeAmount);
				}

			}

            if (colorChangeAmount >= 1.0f)
            {

                colorChangeAmount = 0.0f;

                if (targetColor == endColor)
                {
                    currentColor = endColor;
                    targetColor = startColor;
                }
                else
                {
                    currentColor = startColor;
                    targetColor = endColor;
                }

            }

        }

	}

}
