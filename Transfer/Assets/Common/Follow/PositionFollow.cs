using UnityEngine;

namespace Common
{
    /// <summary>
    /// Follows a transform at a certain distance
    /// </summary>
    public class PositionFollow : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Smoothing factor for the follow")]
        private float smoothing = 5.0f;

        [SerializeField]
        [Tooltip("Distance, in world units, to lag behind the target")]
        private float distance = 0.5f;

        [SerializeField]
        [Tooltip("The target that should be followed")]
        private Transform target;
        public Transform Target { get => target; set => target = value; }

        private Vector2 lastTarget = Vector2.zero;
        private Vector2 offset = Vector2.zero;

        /// <summary>
        /// Add to a position follow chain
        /// </summary>
        /// <param name="next"></param>
        public void AddToChain(ChainFollowParent chain)
        {
            if (!chain.Next)
            {
                Target = chain.transform;
            }
            else
            {
                Target = chain.Next.transform;
            }

            chain.Next = this;
        }

        private void Update()
        {
            if (target)
            {
                if (Vector2.Distance(lastTarget, target.position) > 0)
                {
                    offset = (lastTarget - (Vector2)target.position).normalized * distance;
                }

                transform.position = Vector2.Lerp(transform.position, (Vector2)target.position + offset, smoothing * Time.deltaTime);
                lastTarget = transform.position;
            }
        }
    }
}

