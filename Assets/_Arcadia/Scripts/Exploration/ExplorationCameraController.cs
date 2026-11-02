using UnityEngine;

namespace Arcadia.Exploration
{
    /// <summary>Follows the exploration player while keeping the camera viewport inside map bounds.</summary>
    [RequireComponent(typeof(Camera))]
    public sealed class ExplorationCameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Rect worldBounds = new(-12f, -7f, 24f, 14f);
        [SerializeField] private Vector2 lookAheadOffset = new(0f, 1.25f);
        [SerializeField, Min(0f)] private float smoothTime = 0.15f;

        private Camera explorationCamera;
        private Vector3 followVelocity;

        private void Awake()
        {
            explorationCamera = GetComponent<Camera>();
            explorationCamera.orthographic = true;
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            var desiredPosition = ClampToBounds(target.position + (Vector3)lookAheadOffset);
            transform.position = smoothTime > 0f
                ? Vector3.SmoothDamp(transform.position, desiredPosition, ref followVelocity, smoothTime)
                : desiredPosition;
        }

        private Vector3 ClampToBounds(Vector3 targetPosition)
        {
            var halfHeight = explorationCamera.orthographicSize;
            var halfWidth = halfHeight * explorationCamera.aspect;

            var x = ClampAxis(targetPosition.x, worldBounds.xMin, worldBounds.xMax, halfWidth);
            var y = ClampAxis(targetPosition.y, worldBounds.yMin, worldBounds.yMax, halfHeight);
            return new Vector3(x, y, transform.position.z);
        }

        private static float ClampAxis(float position, float min, float max, float halfViewportSize)
        {
            var viewportMin = min + halfViewportSize;
            var viewportMax = max - halfViewportSize;

            return viewportMin > viewportMax
                ? (min + max) * 0.5f
                : Mathf.Clamp(position, viewportMin, viewportMax);
        }
    }
}
