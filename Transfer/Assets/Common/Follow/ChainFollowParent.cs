using UnityEngine;

namespace Common
{
    /// <summary>
    /// The parent of a position follow chain. Objects with a <see cref="PositionFollow"/>
    /// behaviour can add themselves to the chain using <see cref="PositionFollow.AddToChain(ChainFollowParent)"/>
    /// </summary>
    public class ChainFollowParent : MonoBehaviour
    {
        /// <summary>
        /// The next item in the follow chain
        /// </summary>
        public PositionFollow Next { get; set; }
    }
}

