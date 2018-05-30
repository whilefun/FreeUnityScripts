using UnityEngine;

public class AnimatedGoldSeven : MonoBehaviour {

    public void shimmerAnimationComplete()
    {
        gameObject.transform.parent.parent.GetComponent<WizardJam7SplashScreen>().endShimmer();
    }
}
