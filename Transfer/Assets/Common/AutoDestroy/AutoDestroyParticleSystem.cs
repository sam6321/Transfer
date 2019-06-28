using UnityEngine;

namespace Common
{
    /// <summary>
    /// Automatically destroy a particle system when it finishes playing.
    /// </summary>
    class AutoDestroyParticleSystem : AutoDestroy<ParticleSystem>
    {
        protected override bool IsAlive(ParticleSystem behaviour)
        {
            return behaviour.IsAlive(true);
        }
    }
}
