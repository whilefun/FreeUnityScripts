using UnityEngine;
using UnityEngine.SceneManagement;

public class WizardJamXSplash : MonoBehaviour
{

    [SerializeField, Tooltip("The string name of the scene you want to come after the splash screen.")]
    private string nextSceneName = "MainMenu";

    [SerializeField]
    private AudioClip wizarrrd = null;
    [SerializeField]
    private AudioClip logoBonk = null;
    [SerializeField]
    private AudioClip logoRawwr = null;
    [SerializeField]
    private AudioClip logoPresents = null;


    [SerializeField]
    private Transform logoPartWoosh = null;
    [SerializeField]
    private Transform logoPartJam = null;
    [SerializeField]
    private Transform logoPartX = null;
    [SerializeField]
    private Transform logoPartPresents = null;


    [SerializeField]
    private AudioSource mySpeaker = null;


    private Vector3 wooshStartPosition = Vector3.zero;
    private Vector3 wooshEndPosition = Vector3.zero;
    private Vector3 wooshStepPerSecond = Vector3.zero;
    private float wooshTime = 0.0f;

    private float splashCompleteWaitTime = 0.5f;

    private enum eSplashState
    {
        IDLE = 0,
        WOOSH,
        JAM,
        X,
        PRESENTS,
        WAIT_FOR_KEYPRESS_OR_TIMEOUT
    }
    private eSplashState currentSplashState = eSplashState.IDLE;

    private void Awake()
    {

        wooshEndPosition = logoPartWoosh.transform.position;
        // end.x=-0.492, start.x = -14.69
        wooshEndPosition.x = -0.492f;
        wooshStartPosition = wooshEndPosition;
        wooshStartPosition.x = -14.69f;

        wooshTime = mySpeaker.clip.length;

        wooshStepPerSecond = new Vector3((wooshEndPosition.x - wooshStartPosition.x) / wooshTime, 0.0f, 0.0f);

        logoPartJam.gameObject.SetActive(false);
        logoPartX.gameObject.SetActive(false);
        logoPartPresents.gameObject.SetActive(false);

    }

    void Start()
    {

        doWoosh();

    }


    void Update()
    {

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            doWoosh();
        }
#endif

        if (currentSplashState == eSplashState.WOOSH)
        {

            logoPartWoosh.transform.Translate(wooshStepPerSecond * Time.deltaTime);

            if (Mathf.Abs(logoPartWoosh.transform.position.x - wooshEndPosition.x) < (wooshStepPerSecond.x * Time.deltaTime) || Input.GetMouseButtonDown(0))
            {

                logoPartWoosh.transform.position = wooshEndPosition;
                currentSplashState = eSplashState.JAM;
                logoPartJam.gameObject.SetActive(true);
                mySpeaker.clip = logoRawwr;
                mySpeaker.Play();

            }

        }
        else if (currentSplashState == eSplashState.JAM)
        {

            if(mySpeaker.isPlaying == false || Input.GetMouseButtonDown(0))
            {

                currentSplashState = eSplashState.X;
                logoPartX.gameObject.SetActive(true);
                mySpeaker.clip = logoBonk;
                mySpeaker.Play();

            }

        }
        else if (currentSplashState == eSplashState.X)
        {

            if (mySpeaker.isPlaying == false || Input.GetMouseButtonDown(0))
            {

                currentSplashState = eSplashState.PRESENTS;
                logoPartPresents.gameObject.SetActive(true);
                mySpeaker.clip = logoPresents;
                mySpeaker.Play();

            }

        }
        else if (currentSplashState == eSplashState.PRESENTS)
        {

            if (mySpeaker.isPlaying == false || Input.GetMouseButtonDown(0))
            {

                currentSplashState = eSplashState.WAIT_FOR_KEYPRESS_OR_TIMEOUT;

            }

        }
        else if (currentSplashState == eSplashState.WAIT_FOR_KEYPRESS_OR_TIMEOUT)
        {

            splashCompleteWaitTime -= Time.deltaTime;

            if (Input.anyKeyDown || splashCompleteWaitTime < 0.0f || Input.GetMouseButtonDown(0))
            {
                loadNextScene();
            }

        }

    }

    private void doWoosh()
    {

        currentSplashState = eSplashState.WOOSH;
        logoPartWoosh.transform.position = wooshStartPosition;

        mySpeaker.clip = wizarrrd;
        mySpeaker.Play();

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
            Debug.Log("WizardJamXSplash:: Looks like you don't have any scenes after index " + SceneManager.GetActiveScene().buildIndex + ". Reloading splash scene.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

}
