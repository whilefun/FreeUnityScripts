using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace Whilefun.Audio
{

    //
    // WFGMinMaxRangeAttribute
    //
    // Use a WFGMinMaxRange class to replace twin float range values (eg: float minSpeed,
    // maxSpeed; becomes WFGMinMaxRange speed)
    // Apply a [WFGMinMaxRange( minLimit, maxLimit )] attribute to a WFGMinMaxRange 
    // instance to control the limits and to show a slider in the inspector
    //
    public class WFGMinMaxRangeAttribute : PropertyAttribute
    {

        public float minLimit = 0.0f;
        public float maxLimit = 0.0f;

        public WFGMinMaxRangeAttribute(float minLimit, float maxLimit)
        {
            this.minLimit = minLimit;
            this.maxLimit = maxLimit;
        }

    }

    //
    // WFGMinMaxRange
    //
    // Basic container for min and max float values for slider, as well as function 
    // to fetch a random value inside the range defined by min and max values.
    //
    [Serializable]
    public class WFGMinMaxRange
    {

        public float minValue = 0.0f;
        public float maxValue = 1.0f;

        public float GetRandomValue()
        {
            return Random.Range(minValue, maxValue);
        }

    }

}
