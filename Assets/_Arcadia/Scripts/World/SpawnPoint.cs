using System;
using UnityEngine;

namespace Arcadia.World
{
    /// <summary>
    /// Adapts a scene position to a stable exploration spawn-point identifier.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private string spawnPointId = string.Empty;

        /// <summary>
        /// Gets the stable identifier used by portals and map transitions to find this entry point.
        /// </summary>
        public string SpawnPointId => spawnPointId;

        /// <summary>
        /// Gets a value indicating whether this component has a valid stable identifier.
        /// </summary>
        public bool IsConfigured => !string.IsNullOrWhiteSpace(spawnPointId);

        /// <summary>
        /// Creates a scene-independent definition from this spawn point's current world position.
        /// </summary>
        /// <returns>The stable identifier and world position represented by this component.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no stable identifier has been configured.</exception>
        public SpawnPointDefinition CreateDefinition()
        {
            if (!IsConfigured)
            {
                throw new InvalidOperationException("A spawn point requires a stable identifier.");
            }

            var position = transform.position;
            return new SpawnPointDefinition(
                spawnPointId,
                new MapPosition(position.x, position.y));
        }
    }
}
