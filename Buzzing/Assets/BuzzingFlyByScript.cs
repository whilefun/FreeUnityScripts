using UnityEngine;
using System.Collections;

public class BuzzingFlyByScript : MonoBehaviour {

	public AudioClip buzzA;
	public AudioClip buzzB;

	public float minRandomInterval = 3.0f;
	public float maxRandomInterval = 7.0f;

	private float playInterval;

	// Use this for initialization
	void Start () {
	
		playInterval = Random.Range(minRandomInterval, maxRandomInterval);

	}
	
	// Update is called once per frame
	void Update () {
	
		playInterval -= Time.deltaTime;

		if(playInterval <= 0.0f){

			playInterval = Random.Range(minRandomInterval, maxRandomInterval);

			if(Random.Range(0,10) < 5){
				gameObject.GetComponent<AudioSource>().clip = buzzA;
			}else{
				gameObject.GetComponent<AudioSource>().clip = buzzB;
			}

			gameObject.GetComponent<AudioSource>().Play();

		}

	}

}
