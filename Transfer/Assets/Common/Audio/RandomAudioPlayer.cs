using UnityEngine;

namespace Common
{
    /// <summary>
    /// Plays random sounds from an audio group whenever <see cref="Play"/> is called.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class RandomAudioPlayer : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Audio group to play sounds from.")]
        private AudioGroup audioGroup;

        private AudioSource audioSource = null;

        /// <summary>
        /// Play a random sound from the provided audio group.
        /// </summary>
        public void Play()
        {
            if(audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }

            audioGroup.PlayRandomOneShot(audioSource);
        }
    }
}

