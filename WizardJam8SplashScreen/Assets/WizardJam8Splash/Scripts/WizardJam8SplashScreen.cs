using UnityEngine;
using UnityEngine.SceneManagement;

//
// WizardJam8SplashScreen
// A splash screen animation for Wizard Jam 8
// This script handles some state and effects then transitions to the next scene in the build.
// To use: Place in your build settings as Scene Index 0. When the animation finishes, scene 
// index 1 will be loaded.
//
public class WizardJam8SplashScreen : MonoBehaviour {

    [SerializeField, Tooltip("The AudioSource that will play the intro sound.")]
    private AudioSource introSpeaker = null;
    
    [SerializeField, Tooltip("The text or image hint that will appear once intro sound and animation are finished.")]
    private GameObject inputHint = null;

    private bool haveStartedPlaying = false;

    private void Update()
    {
        
        if(haveStartedPlaying && introSpeaker.isPlaying == false)
        {
            inputHint.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            loadNextScene();
        }

    }

    public void PlayIntro()
    {

        if (introSpeaker)
        {
            introSpeaker.Play();
            haveStartedPlaying = true;
        }

    }

    /// <summary>
    /// By default, just loads the next scene in the build index. Seems reasonable. Change as required.
    /// </summary>
    private void loadNextScene()
    {

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (SceneManager.sceneCountInBuildSettings > (nextSceneIndex))
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("WizardJam8SplashScreen:: Looks like you don't have any scenes after index " + SceneManager.GetActiveScene().buildIndex + ". Reloading splash scene.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

}
