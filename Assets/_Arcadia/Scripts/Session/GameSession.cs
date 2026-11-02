using System;
using Arcadia.World;

namespace Arcadia.Session
{
    /// <summary>
    /// Keeps exploration state in memory while the application is running.
    /// </summary>
    public sealed class GameSession
    {
        private ExplorationLocation currentExplorationLocation;

        /// <summary>
        /// Gets a value indicating whether exploration state has been registered for the current session.
        /// </summary>
        public bool HasExplorationLocation => currentExplorationLocation != null;

        /// <summary>
        /// Gets the latest exploration state registered for the current session.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when no exploration state has been registered.</exception>
        public ExplorationLocation CurrentExplorationLocation => currentExplorationLocation
            ?? throw new InvalidOperationException("No exploration location has been registered for this session.");

        /// <summary>
        /// Replaces the exploration state kept in memory for this application session.
        /// </summary>
        /// <param name="location">Current map, entry point and player position.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="location"/> is <c>null</c>.</exception>
        public void SetExplorationLocation(ExplorationLocation location)
        {
            currentExplorationLocation = location
                ?? throw new ArgumentNullException(nameof(location));
        }
    }
}
