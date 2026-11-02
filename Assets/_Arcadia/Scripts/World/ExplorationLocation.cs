using System;

namespace Arcadia.World
{
    /// <summary>
    /// Represents a two-dimensional world position without depending on a scene transform.
    /// </summary>
    public readonly struct MapPosition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapPosition"/> struct.
        /// </summary>
        /// <param name="x">Horizontal world coordinate.</param>
        /// <param name="y">Vertical world coordinate.</param>
        public MapPosition(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets the horizontal world coordinate.
        /// </summary>
        public float X { get; }

        /// <summary>
        /// Gets the vertical world coordinate.
        /// </summary>
        public float Y { get; }
    }

    /// <summary>
    /// Identifies where the player is in exploration without retaining a reference to a Unity scene or GameObject.
    /// </summary>
    public sealed class ExplorationLocation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExplorationLocation"/> class.
        /// </summary>
        /// <param name="mapId">Stable identifier of the current map.</param>
        /// <param name="spawnPointId">Stable identifier of the entry point used in the current map.</param>
        /// <param name="position">Current player position in world coordinates.</param>
        /// <exception cref="ArgumentException">Thrown when a stable identifier is empty or whitespace.</exception>
        public ExplorationLocation(string mapId, string spawnPointId, MapPosition position)
        {
            MapId = ValidateIdentifier(mapId, nameof(mapId));
            SpawnPointId = ValidateIdentifier(spawnPointId, nameof(spawnPointId));
            Position = position;
        }

        /// <summary>
        /// Gets the stable identifier of the current map.
        /// </summary>
        public string MapId { get; }

        /// <summary>
        /// Gets the stable identifier of the entry point used in the current map.
        /// </summary>
        public string SpawnPointId { get; }

        /// <summary>
        /// Gets the current player position in world coordinates.
        /// </summary>
        public MapPosition Position { get; }

        private static string ValidateIdentifier(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("A stable identifier is required.", parameterName);
            }

            return value.Trim();
        }
    }

    /// <summary>
    /// Defines a named entry point in a map using a stable identifier and a world position.
    /// </summary>
    public sealed class SpawnPointDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpawnPointDefinition"/> class.
        /// </summary>
        /// <param name="spawnPointId">Stable identifier of the entry point.</param>
        /// <param name="position">World position where the player should appear.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="spawnPointId"/> is empty or whitespace.</exception>
        public SpawnPointDefinition(string spawnPointId, MapPosition position)
        {
            if (string.IsNullOrWhiteSpace(spawnPointId))
            {
                throw new ArgumentException("A stable identifier is required.", nameof(spawnPointId));
            }

            SpawnPointId = spawnPointId.Trim();
            Position = position;
        }

        /// <summary>
        /// Gets the stable identifier of the entry point.
        /// </summary>
        public string SpawnPointId { get; }

        /// <summary>
        /// Gets the world position where the player should appear.
        /// </summary>
        public MapPosition Position { get; }
    }
}
