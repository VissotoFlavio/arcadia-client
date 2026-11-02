using System;
using Arcadia.Exploration;
using UnityEngine;

namespace Arcadia.World
{
    /// <summary>
    /// Detects a player entering a visible portal and requests a map transition.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CircleCollider2D))]
    public sealed class Portal : MonoBehaviour
    {
        [SerializeField] private string portalId = string.Empty;
        [SerializeField] private MapDefinition destinationMap;
        [SerializeField] private string destinationSpawnPointId = string.Empty;

        private CircleCollider2D activationArea;
        private bool transitionRequestedDuringOccupancy;

        /// <summary>
        /// Gets the stable identifier of this portal inside its source map.
        /// </summary>
        public string PortalId => portalId;

        /// <summary>
        /// Gets the map selected when this portal is crossed.
        /// </summary>
        public MapDefinition DestinationMap => destinationMap;

        /// <summary>
        /// Gets the stable destination spawn-point identifier.
        /// </summary>
        public string DestinationSpawnPointId => destinationSpawnPointId;

        /// <summary>
        /// Gets a value indicating whether this portal has a usable transition destination.
        /// </summary>
        public bool IsConfigured =>
            !string.IsNullOrWhiteSpace(portalId) &&
            destinationMap != null &&
            !string.IsNullOrWhiteSpace(destinationSpawnPointId);

        /// <summary>
        /// Occurs once while the player occupies this portal's activation area.
        /// </summary>
        public event Action<PortalTransitionRequest> TransitionRequested;

        private void Awake()
        {
            activationArea = GetComponent<CircleCollider2D>();
            activationArea.isTrigger = true;
        }

        private void OnValidate()
        {
            var area = GetComponent<CircleCollider2D>();
            if (area != null)
            {
                area.isTrigger = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (transitionRequestedDuringOccupancy || !IsConfigured ||
                other.GetComponentInParent<PlayerMotor>() == null)
            {
                return;
            }

            transitionRequestedDuringOccupancy = true;
            TransitionRequested?.Invoke(new PortalTransitionRequest(
                portalId,
                destinationMap,
                destinationSpawnPointId));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponentInParent<PlayerMotor>() != null)
            {
                transitionRequestedDuringOccupancy = false;
            }
        }
    }

    /// <summary>
    /// Describes a transition requested by a player crossing a portal.
    /// </summary>
    public readonly struct PortalTransitionRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PortalTransitionRequest"/> structure.
        /// </summary>
        /// <param name="portalId">Stable identifier of the portal that was crossed.</param>
        /// <param name="destinationMap">Configured destination map.</param>
        /// <param name="destinationSpawnPointId">Stable identifier of the destination entry point.</param>
        public PortalTransitionRequest(
            string portalId,
            MapDefinition destinationMap,
            string destinationSpawnPointId)
        {
            PortalId = portalId;
            DestinationMap = destinationMap;
            DestinationSpawnPointId = destinationSpawnPointId;
        }

        /// <summary>
        /// Gets the stable identifier of the portal that was crossed.
        /// </summary>
        public string PortalId { get; }

        /// <summary>
        /// Gets the configured map to be loaded by a later transition system.
        /// </summary>
        public MapDefinition DestinationMap { get; }

        /// <summary>
        /// Gets the stable identifier of the entry point to use in the destination map.
        /// </summary>
        public string DestinationSpawnPointId { get; }
    }
}
