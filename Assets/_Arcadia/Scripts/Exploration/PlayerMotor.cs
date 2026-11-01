using Arcadia.Input;
using UnityEngine;

namespace Arcadia.Exploration
{
    /// <summary>Moves the physical player root from exploration input without knowing about its visual.</summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PlayerMotor : MonoBehaviour
    {
        private const float DefaultSpeed = 4f;

        [SerializeField] private ExplorationInputReader inputReader;
        [SerializeField, Min(0f)] private float speed = DefaultSpeed;

        private Rigidbody2D body;

        /// <summary>Last non-zero cardinal direction, preserved for future idle visuals.</summary>
        public FacingDirection LastFacingDirection { get; private set; } = FacingDirection.Down;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var movement = inputReader != null ? inputReader.Move : Vector2.zero;

            if (movement.sqrMagnitude > 1f)
            {
                movement.Normalize();
            }

            if (movement.sqrMagnitude > 0f)
            {
                LastFacingDirection = ToFacingDirection(movement);
            }

            body.MovePosition(body.position + movement * speed * Time.fixedDeltaTime);
        }

        private static FacingDirection ToFacingDirection(Vector2 movement)
        {
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                return movement.x > 0f ? FacingDirection.Right : FacingDirection.Left;
            }

            return movement.y > 0f ? FacingDirection.Up : FacingDirection.Down;
        }
    }

    public enum FacingDirection
    {
        Down,
        Left,
        Up,
        Right
    }
}
