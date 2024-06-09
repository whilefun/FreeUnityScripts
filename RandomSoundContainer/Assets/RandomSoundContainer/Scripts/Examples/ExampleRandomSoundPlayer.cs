using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Whilefun.Audio;


public class ExampleRandomSoundPlayer : MonoBehaviour
{

    [SerializeField]
    private AudioSource exampleAudioSource = null;

    [SerializeField]
    private WFGSimpleRandomSoundContainer exampleSoundContainer = null;

    private void Start()
    {

        Debug.Log("Press <space> to play a random sound.");

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {

            if(exampleSoundContainer && exampleAudioSource)
            {
                exampleSoundContainer.Play(exampleAudioSource);
            }
            else
            {
                Debug.LogError("ExampleRandomSoundPlayer:: Can't play sound. Either exampleAudioSource or exampleSoundContainer are missing from Inspector assignment!");
            }

        }

    }

}
