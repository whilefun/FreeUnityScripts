using UnityEngine;

public class SpinningLetter : MonoBehaviour {

    public void swapLetters()
    {
        gameObject.transform.parent.GetComponent<WizardJam7SplashScreen>().swapLetters();
    }

    public void startShimmer()
    {
        gameObject.transform.parent.GetComponent<WizardJam7SplashScreen>().startShimmer();
    }

}
