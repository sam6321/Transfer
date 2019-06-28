using System.Collections;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// Base class for auto destroy behaviours.
    /// This class performs the check loop in a coroutine.
    /// Derived classes specify the type of behaviour to check and provide 
    /// the <see cref="IsAlive"/> property to check whether the behaviour is alive.
    /// The <see cref="behaviour"/> inspector field can be used to specify the behaviour to check. 
    /// If this is not set, GetComponent will be used to get any matching components on this game object.
    /// </summary>
    /// <typeparam name="T">The type of the behaviour to check</typeparam>
    abstract class AutoDestroy<T> : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("If true, only deactivate this game object instead of destroying it")]
        private bool onlyDeactivate;

        [SerializeField]
        [Tooltip("Delay between alive checks, in seconds")]
        private float checkDelay = 0.5f;

        [SerializeField]
        [Tooltip("The behaviour to check. If not set, GetComponent will be used to get any matching components on this game object.")]
        private T behaviour;

        void Start()
        {
            if(behaviour == null)
            {
                behaviour = GetComponent<T>();
            }

            if(behaviour == null)
            {
                Debug.LogError("AutoDestroy has no behaviour set and will do nothing");
            }
            else
            {
                StartCoroutine(CheckIfAlive());
            }
        }

        /// <summary>
        /// The coroutine to check if the behaviour is alive.
        /// </summary>
        /// <returns>Coroutine Enumerator</returns>
        private IEnumerator CheckIfAlive()
        {
            while (true)
            {
                yield return new WaitForSeconds(checkDelay);
                if (!IsAlive(behaviour))
                {
                    if (onlyDeactivate)
                    {
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Abstract function that is used to check if the behaviour is running.
        /// </summary>
        /// <param name="behaviour">The behaviour being checked</param>
        /// <returns>True if the behaviour is still alive, false if not and it should be destroyed.</returns>
        protected abstract bool IsAlive(T behaviour);
    }
}
