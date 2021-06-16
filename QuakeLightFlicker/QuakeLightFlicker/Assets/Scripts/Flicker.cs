using System.Linq;
using UnityEngine;

//
// Flicker
// A modification of the C# port of old quake code that makes lights flicker based on strings.
// See https://github.com/id-Software/Quake/blob/master/QW/progs/world.qc#L303
// And also https://twitter.com/LotteMakesStuff/status/1405024836536127490
// 
// Original code written by Lotte, and modifed by Richard
//
[RequireComponent(typeof(Light))]
public class Flicker : MonoBehaviour
{

    [SerializeField]
    private LightDefinitions.eLightStyles currentLightStyle = LightDefinitions.eLightStyles.NORMAL;
    [SerializeField]
    private string currentLightStylePattern = "";

    private Light myLight = null;

    [HideInInspector]
    [SerializeField]
    private int currentIndex = 0;

    private float lightTimer = 0.0f;
    private float stepTime = 0.0f;
    private bool lightOn = true;


    void Awake()
    {

        myLight = gameObject.GetComponent<Light>();
        ChangeLightStyle(currentLightStyle);

    }


    void Update()
    {

        if (lightOn)
        {

            lightTimer += Time.deltaTime;

            if (stepTime < lightTimer)
            {

                lightTimer -= stepTime;
                currentIndex++;

                if (currentIndex >= currentLightStylePattern.Length)
                {
                    currentIndex = 0;
                }

            }

            char c = currentLightStylePattern[currentIndex];
            int val = c - 'a';
            float intensity = (val / 25.0f) * 2.0f;
            myLight.intensity = intensity;

        }

    }


    public void ChangeLightStyle(LightDefinitions.eLightStyles style, float loopDuration = LightDefinitions.DEFAULT_LOOP_DURATION_IN_SECONDS)
    {

        lightTimer = 0.0f;
        currentLightStyle = style;
        currentLightStylePattern = LightDefinitions.LightStylePatterns[(int)currentLightStyle];
        stepTime = (loopDuration / currentLightStylePattern.Length);
        currentIndex = 0;

    }

    public void TurnLightOn()
    {

        if (lightOn == false)
        {
            lightOn = true;
        }

    }

    public void TurnLightOff()
    {

        if (lightOn == true)
        {
            lightOn = false;
        }

    }

    // For testing only, really
    public string SwitchToToNextLightStyle()
    {

        int lightPatternCount = System.Enum.GetNames(typeof(LightDefinitions.eLightStyles)).Length;

        currentLightStyle++;

        if ((int)currentLightStyle > (lightPatternCount-1))
        {
            currentLightStyle = 0;
        }

        ChangeLightStyle(currentLightStyle);

        return "" + currentLightStyle;

    }

    public string SwitchToToPreviousLightStyle()
    {

        int lightPatternCount = System.Enum.GetNames(typeof(LightDefinitions.eLightStyles)).Length;

        currentLightStyle--;

        if (currentLightStyle < 0 )
        {
            currentLightStyle = System.Enum.GetValues(typeof(LightDefinitions.eLightStyles)).Cast<LightDefinitions.eLightStyles>().Max();
        }

        ChangeLightStyle(currentLightStyle);

        return "" + currentLightStyle;

    }

}