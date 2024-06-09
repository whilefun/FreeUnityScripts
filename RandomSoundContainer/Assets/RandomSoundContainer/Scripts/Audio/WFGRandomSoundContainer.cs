using UnityEngine;

namespace Whilefun.Audio
{

    //
    // WFGRandomSoundContainer
    //
    // This simple class allows for you to create a sound bank
    //
    [System.Serializable]
    public abstract class WFGRandomSoundContainer : ScriptableObject
    {
        public abstract void Play(AudioSource source);
    }

}
