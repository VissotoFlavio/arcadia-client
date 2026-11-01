using UnityEngine;
using UnityEngine.Rendering;

namespace Arcadia.Exploration
{
    /// <summary>
    /// Assigns one global visual order from the world Y coordinate of an object's feet.
    /// </summary>
    [RequireComponent(typeof(SortingGroup))]
    public sealed class YSortOrder : MonoBehaviour
    {
        public const int GroundOrder = -10000;
        public const int StaticObstacleOrder = -9000;
        public const int OrderPerWorldUnit = 100;

        [SerializeField] private Transform footAnchor;

        private SortingGroup sortingGroup;

        private void Awake()
        {
            sortingGroup = GetComponent<SortingGroup>();
        }

        private void LateUpdate()
        {
            ApplyOrder();
        }

        /// <summary>Associates this visual group with the transform located at its physical foot point.</summary>
        public void SetFootAnchor(Transform anchor)
        {
            footAnchor = anchor;
            ApplyOrder();
        }

        public static int FromWorldY(float worldY)
        {
            return -Mathf.RoundToInt(worldY * OrderPerWorldUnit);
        }

        private void ApplyOrder()
        {
            if (sortingGroup == null)
            {
                sortingGroup = GetComponent<SortingGroup>();
            }

            var feet = footAnchor != null
                ? footAnchor
                : transform.parent != null ? transform.parent : transform;
            sortingGroup.sortingOrder = FromWorldY(feet.position.y);
        }
    }
}
