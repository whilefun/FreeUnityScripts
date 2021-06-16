using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    
    [SerializeField]
    private Flicker myLightFlicker = null;

    [SerializeField]
    private Text lightPatternText = null;

    
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            lightPatternText.text = myLightFlicker.SwitchToToNextLightStyle();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            lightPatternText.text = myLightFlicker.SwitchToToPreviousLightStyle();
        }

    }


#if UNITY_EDITOR

	void OnDrawGizmos()
    {
        
    }

#endif
		
}
