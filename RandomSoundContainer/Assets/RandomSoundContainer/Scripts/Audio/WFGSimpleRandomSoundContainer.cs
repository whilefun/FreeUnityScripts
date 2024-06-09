using UnityEngine;
using Random = UnityEngine.Random;

namespace Whilefun.Audio
{

    //
    // WFGSimpleRandomSoundContainer
    //
    // This class provides a simple means to play a set of sounds with some randomization.
    //
    [CreateAssetMenu(menuName = "While Fun Games/Simple Random Sound Container")]
    [System.Serializable]
    public class WFGSimpleRandomSoundContainer : WFGRandomSoundContainer
    {

        public AudioClip[] clips;

        // TODO: Make these draw as sliders!

        [WFGMinMaxRange(0.0f, 1.0f)]
        public WFGMinMaxRange volume;

        [WFGMinMaxRange(0.1f, 2.0f)]
        public WFGMinMaxRange pitch;

        public override void Play(AudioSource source)
        {

            if (clips.Length > 0)
            {

                source.clip = clips[Random.Range(0, clips.Length)];
                source.volume = Random.Range(volume.minValue, volume.maxValue);
                source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
                source.Play();

            }

        }

    }

}
