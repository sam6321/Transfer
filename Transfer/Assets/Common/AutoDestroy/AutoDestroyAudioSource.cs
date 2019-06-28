using UnityEngine;

namespace Common
{
    /// <summary>
    /// Automatically destroy an audio source when it finishes playing
    /// </summary>
    class AutoDestroyAudioSource : AutoDestroy<AudioSource>
    {
        protected override bool IsAlive(AudioSource behaviour)
        {
            return behaviour.isPlaying;
        }
    }
}
