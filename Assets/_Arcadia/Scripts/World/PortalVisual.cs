using UnityEngine;

namespace Arcadia.World
{
    /// <summary>
    /// Builds the temporary in-world marker that makes a portal readable before final art exists.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Portal))]
    public sealed class PortalVisual : MonoBehaviour
    {
        private const int TextureSize = 64;
        private const int PortalSortingOrder = 30;

        [SerializeField] private PortalExitDirection exitDirection = PortalExitDirection.Right;
        [SerializeField] private Color ringColor = new(0.17f, 0.88f, 0.9f, 1f);

        private Sprite ringSprite;
        private Sprite arrowSprite;
        private Texture2D ringTexture;
        private Texture2D arrowTexture;

        private void Awake()
        {
            CreateVisuals();
        }

        private void OnDestroy()
        {
            DestroyGeneratedAsset(ringSprite);
            DestroyGeneratedAsset(arrowSprite);
            DestroyGeneratedAsset(ringTexture);
            DestroyGeneratedAsset(arrowTexture);
        }

        private void CreateVisuals()
        {
            var visualRoot = new GameObject("TechnicalPortalVisual");
            visualRoot.transform.SetParent(transform, false);
            visualRoot.transform.localPosition = Vector3.up * 0.55f;

            ringTexture = CreateRingTexture();
            ringSprite = Sprite.Create(
                ringTexture,
                new Rect(0f, 0f, TextureSize, TextureSize),
                new Vector2(0.5f, 0.5f),
                TextureSize);

            var ring = visualRoot.AddComponent<SpriteRenderer>();
            ring.name = "PortalRing";
            ring.sprite = ringSprite;
            ring.color = ringColor;
            ring.sortingOrder = PortalSortingOrder;

            arrowTexture = CreateArrowTexture();
            arrowSprite = Sprite.Create(
                arrowTexture,
                new Rect(0f, 0f, TextureSize, TextureSize),
                new Vector2(0.5f, 0.5f),
                TextureSize);

            var arrowObject = new GameObject("PortalExitArrow");
            arrowObject.transform.SetParent(visualRoot.transform, false);
            arrowObject.transform.localPosition = Vector3.up * 0.72f;
            arrowObject.transform.localRotation = Quaternion.Euler(0f, 0f, ToAngle(exitDirection));

            var arrow = arrowObject.AddComponent<SpriteRenderer>();
            arrow.sprite = arrowSprite;
            arrow.color = Color.white;
            arrow.sortingOrder = PortalSortingOrder + 1;
        }

        private static Texture2D CreateRingTexture()
        {
            var texture = CreateTexture();
            var center = (TextureSize - 1) * 0.5f;

            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var distance = Vector2.Distance(new Vector2(x, y), new Vector2(center, center)) / center;
                    var alpha = distance is > 0.6f and < 0.88f ? 1f : 0f;
                    texture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateArrowTexture()
        {
            var texture = CreateTexture();
            var center = (TextureSize - 1) * 0.5f;

            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var normalizedX = (x - center) / center;
                    var normalizedY = (y - center) / center;
                    var isShaft = Mathf.Abs(normalizedX) < 0.16f && normalizedY is > -0.62f and < 0.25f;
                    var isHead = normalizedY is >= 0.1f && normalizedY <= 0.78f &&
                        Mathf.Abs(normalizedX) <= 0.78f - normalizedY;
                    texture.SetPixel(x, y, isShaft || isHead ? Color.white : Color.clear);
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateTexture()
        {
            return new Texture2D(TextureSize, TextureSize)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp,
                hideFlags = HideFlags.DontSave
            };
        }

        private static float ToAngle(PortalExitDirection direction)
        {
            return direction switch
            {
                PortalExitDirection.Up => 0f,
                PortalExitDirection.Left => 90f,
                PortalExitDirection.Down => 180f,
                PortalExitDirection.Right => -90f,
                _ => 0f
            };
        }

        private static void DestroyGeneratedAsset(Object generatedAsset)
        {
            if (generatedAsset != null)
            {
                Destroy(generatedAsset);
            }
        }
    }

    /// <summary>
    /// Identifies the direction communicated by a technical portal's exit arrow.
    /// </summary>
    public enum PortalExitDirection
    {
        /// <summary>Points upward in the exploration map.</summary>
        Up,

        /// <summary>Points left in the exploration map.</summary>
        Left,

        /// <summary>Points downward in the exploration map.</summary>
        Down,

        /// <summary>Points right in the exploration map.</summary>
        Right
    }
}
