using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcadia.World
{
    /// <summary>
    /// Content metadata that identifies an exploration map and its portal destinations.
    /// </summary>
    [CreateAssetMenu(
        fileName = "MapDefinition",
        menuName = "Arcadia/World/Map Definition",
        order = 0)]
    public sealed class MapDefinition : ScriptableObject
    {
        [SerializeField] private string mapId = string.Empty;
        [SerializeField] private string sceneReference = string.Empty;
        [SerializeField] private bool isSafeZone = true;
        [SerializeField] private PortalDefinition[] portals = Array.Empty<PortalDefinition>();

        /// <summary>
        /// Gets the stable identifier used by game systems to identify this map.
        /// </summary>
        public string MapId => mapId;

        /// <summary>
        /// Gets the Unity scene name that will be loaded for this map.
        /// </summary>
        public string SceneReference => sceneReference;

        /// <summary>
        /// Gets a value indicating whether random encounters are disabled in this map.
        /// </summary>
        public bool IsSafeZone => isSafeZone;

        /// <summary>
        /// Gets the portal destinations authored for this map.
        /// </summary>
        public IReadOnlyList<PortalDefinition> Portals => portals;

        /// <summary>
        /// Attempts to retrieve the destination configured for a portal identifier.
        /// </summary>
        /// <param name="portalId">Stable identifier of the portal in this map.</param>
        /// <param name="portal">The configured destination when the portal exists; otherwise <c>null</c>.</param>
        /// <returns><c>true</c> when a configured portal matches <paramref name="portalId"/>; otherwise <c>false</c>.</returns>
        public bool TryGetPortal(string portalId, out PortalDefinition portal)
        {
            if (string.IsNullOrWhiteSpace(portalId))
            {
                portal = null;
                return false;
            }

            foreach (var configuredPortal in portals)
            {
                if (configuredPortal != null && string.Equals(
                        configuredPortal.PortalId,
                        portalId,
                        StringComparison.Ordinal))
                {
                    portal = configuredPortal;
                    return true;
                }
            }

            portal = null;
            return false;
        }
    }

    /// <summary>
    /// Defines the stable destination selected when the player crosses one portal.
    /// </summary>
    [Serializable]
    public sealed class PortalDefinition
    {
        [SerializeField] private string portalId = string.Empty;
        [SerializeField] private string destinationMapId = string.Empty;
        [SerializeField] private string destinationSpawnPointId = string.Empty;

        /// <summary>
        /// Gets the stable identifier of this portal inside its source map.
        /// </summary>
        public string PortalId => portalId;

        /// <summary>
        /// Gets the stable identifier of the map reached through this portal.
        /// </summary>
        public string DestinationMapId => destinationMapId;

        /// <summary>
        /// Gets the stable identifier of the destination spawn point.
        /// </summary>
        public string DestinationSpawnPointId => destinationSpawnPointId;
    }
}
