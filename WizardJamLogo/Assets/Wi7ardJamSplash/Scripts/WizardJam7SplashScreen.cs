using UnityEngine;
using UnityEngine.SceneManagement;

//
// WizardJam7SplashScreen
// A splash screen animation for Wizard Jam 7 (or Wi7zard Jam)
// This script handles some state and effects then transitions to the next scene in the build.
// To use: Place in your build settings as Scene Index 0. When the animation finishes, scene 
// index 1 will be loaded.
//
public class WizardJam7SplashScreen : MonoBehaviour {

    private enum eSplashState
    {
        IDLE = 0, // Heh.
        STARTED = 1,
        FINISHED = 2
    }
    private eSplashState currentSplashState = eSplashState.IDLE;

    private Transform spinner = null;
    private GameObject grayZed = null;
    private GameObject graySeven = null;
    private GameObject animatedGoldSeven = null;
    private GameObject goldSeven = null;

    // Audio
    private AudioSource mySpeaker = null;
    private float clipDuration = 0.0f;
    private float elapsedClipTime = 0.0f;

    // Camera
    private Camera myCamera = null;
    private float finalOrthoSize = 0.0f;
    private float orthoZoomFactor = 0.5f;

    // Transitions
    [SerializeField, Tooltip("How long (in seconds) between end of 'bing' sound and load next scene. Default is 0.25.")]
    private float shimmerToSceneLoadDelay = 0.25f;
    private float sceneLoadCounter = 0.0f;

    [SerializeField, Tooltip("How long (in seconds) to wait after scene Start before starting splash animation. Default is 0.1.")]
    private float splashStartDelay = 0.1f;

    [SerializeField, Tooltip("If true, camera will do a soft zoom at end of animation")]
    private bool cameraZoom = true;

    void Awake () {

        spinner = gameObject.transform.Find("Spinner");
        grayZed = gameObject.transform.Find("Spinner/GrayZed").gameObject;
        graySeven = gameObject.transform.Find("Spinner/GraySeven").gameObject;
        animatedGoldSeven = gameObject.transform.Find("Spinner/AnimatedGoldSeven").gameObject;
        goldSeven = gameObject.transform.Find("Spinner/GoldSeven").gameObject;

        if(!spinner || !grayZed || !graySeven || !animatedGoldSeven || !goldSeven)
        {
            Debug.LogError("WizardJam7SplashScreen:: One or more of the Sprites or Transforms in the splash screen are missing! Logo splash will be lackluster.");
        }

        grayZed.SetActive(true);
        graySeven.SetActive(false);
        animatedGoldSeven.SetActive(false);
        goldSeven.SetActive(false);

        mySpeaker = gameObject.GetComponent<AudioSource>();
        if (!mySpeaker)
        {
            Debug.LogError("WizardJam7SplashScreen:: You deleted the speaker, silly. Sounds won't play.");
        }

        clipDuration = mySpeaker.clip.length;

        sceneLoadCounter = clipDuration + shimmerToSceneLoadDelay;

        // Going to assume there's only one camera and the scene didn't get broken.
        myCamera = Camera.main;
        finalOrthoSize = myCamera.orthographicSize * 0.9f;

    }
	
	void Update () {

        // Some fancy Dolby stuff or something
        if (mySpeaker.isPlaying)
        {

            elapsedClipTime += Time.deltaTime;
            // Note the clip included in the package had a long fade, so we start X% less 
            // than "Full Left" stereo so it sounds a little better. Tweak as required.
            float panValue = Mathf.Min(-0.7f + (2.0f * (elapsedClipTime / clipDuration)), 1.0f);
            mySpeaker.panStereo = panValue;

            if (cameraZoom)
            {
                myCamera.orthographicSize = Mathf.Lerp(myCamera.orthographicSize, finalOrthoSize, orthoZoomFactor * Time.deltaTime);
            }

        }

        if(currentSplashState == eSplashState.IDLE)
        {

            splashStartDelay -= Time.deltaTime;

            if (splashStartDelay <= 0.0f)
            {
                startSplashAnimation();
            }

        }
        if (currentSplashState == eSplashState.FINISHED)
        {

            sceneLoadCounter -= Time.deltaTime;

            if (sceneLoadCounter <= 0.0f)
            {
                loadNextScene();
            }

        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD

        #region DEBUG_KEYS

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("WizardJam7SplashScreen-Debug:: Reload Scene");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("WizardJam7SplashScreen-Debug:: Start Animation");
            startSplashAnimation();
        }

        #endregion

#endif

    }

    /// <summary>
    /// Starts the animation sequence
    /// </summary>
    public void startSplashAnimation()
    {

        grayZed.SetActive(true);
        graySeven.SetActive(false);
        animatedGoldSeven.SetActive(false);
        goldSeven.SetActive(false);
        spinner.GetComponent<Animator>().SetTrigger("RotateLetter");
        currentSplashState = eSplashState.STARTED;

    }

    /// <summary>
    /// Swaps out the Z for the 7.
    /// </summary>
    public void swapLetters()
    {

        graySeven.SetActive(true);
        grayZed.SetActive(false);

    }

    /// <summary>
    /// Enables the shiny gold 7 and plays its animation and a nice sound
    /// </summary>
    public void startShimmer()
    {

        spinner.GetComponent<Animator>().ResetTrigger("RotateLetter");
        animatedGoldSeven.SetActive(true);
        graySeven.SetActive(false);
        goldSeven.SetActive(false);
        mySpeaker.Play();

    }

    /// <summary>
    /// Swaps shiny gold 7 for normal gold 7
    /// </summary>
    public void endShimmer()
    {

        goldSeven.SetActive(true);
        animatedGoldSeven.SetActive(false);
        currentSplashState = eSplashState.FINISHED;

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
            Debug.Log("WizardJam7SplashScreen:: Looks like you don't have any scenes after index "+ SceneManager.GetActiveScene().buildIndex + ". Reloading splash scene.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

}
