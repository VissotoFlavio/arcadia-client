using UnityEngine;
using UnityEngine.Tilemaps;

namespace Arcadia.Exploration
{
    /// <summary>
    /// Builds the small, intentionally art-free exploration map used to validate
    /// isometric readability and foot-level collision.
    /// </summary>
    [RequireComponent(typeof(Grid))]
    public sealed class TechnicalIsometricMapBuilder : MonoBehaviour
    {
        private static readonly Vector3Int MapOrigin = new(-12, -7, 0);
        private static readonly Vector2Int MapSize = new(24, 14);

        private const string GroundTilemapName = "GroundTilemap";
        private const string ObstacleTilemapName = "ObstacleTilemap";
        private const string GeneratedObjectsName = "GeneratedTechnicalObstacles";

        [SerializeField] private TechnicalMapVariant mapVariant = TechnicalMapVariant.Exploration;

        private Tilemap groundTilemap;
        private Tilemap obstacleTilemap;
        private Sprite placeholderSprite;
        private Sprite tileSprite;

        private void Awake()
        {
            groundTilemap = FindTilemap(GroundTilemapName);
            obstacleTilemap = FindTilemap(ObstacleTilemapName);
            var spriteRenderers = FindObjectsByType<SpriteRenderer>(FindObjectsInactive.Exclude);
            placeholderSprite = spriteRenderers.Length > 0 ? spriteRenderers[0].sprite : null;
            tileSprite = CreateDiamondSprite();

            if (groundTilemap == null || obstacleTilemap == null || placeholderSprite == null || tileSprite == null)
            {
                Debug.LogError("The technical isometric map needs both Tilemap children and a placeholder sprite.", this);
                enabled = false;
                return;
            }

            BuildGround();
            BuildWalls();
            BuildLandmarks();
        }

        private Tilemap FindTilemap(string tilemapName)
        {
            var child = transform.Find(tilemapName);
            return child != null ? child.GetComponent<Tilemap>() : null;
        }

        private void BuildGround()
        {
            groundTilemap.ClearAllTiles();
            groundTilemap.GetComponent<TilemapRenderer>().sortingOrder = YSortOrder.GroundOrder;
            var lightGround = CreateTile(mapVariant == TechnicalMapVariant.Exploration
                ? new Color(0.38f, 0.59f, 0.47f)
                : new Color(0.56f, 0.48f, 0.35f));
            var darkGround = CreateTile(mapVariant == TechnicalMapVariant.Exploration
                ? new Color(0.32f, 0.52f, 0.41f)
                : new Color(0.48f, 0.40f, 0.29f));

            for (var x = 0; x < MapSize.x; x++)
            {
                for (var y = 0; y < MapSize.y; y++)
                {
                    var cell = MapOrigin + new Vector3Int(x, y, 0);
                    groundTilemap.SetTile(cell, (x + y) % 2 == 0 ? lightGround : darkGround);
                }
            }
        }

        private void BuildWalls()
        {
            obstacleTilemap.ClearAllTiles();
            EnsureObstacleCollider();
            obstacleTilemap.GetComponent<TilemapRenderer>().sortingOrder = YSortOrder.StaticObstacleOrder;
            var wallTile = CreateTile(mapVariant == TechnicalMapVariant.Exploration
                ? new Color(0.21f, 0.26f, 0.31f)
                : new Color(0.31f, 0.24f, 0.20f), Tile.ColliderType.Grid);
            var maxX = MapOrigin.x + MapSize.x - 1;
            var maxY = MapOrigin.y + MapSize.y - 1;

            for (var x = MapOrigin.x; x <= maxX; x++)
            {
                SetObstacleTile(new Vector3Int(x, maxY, 0), wallTile);
            }

            if (mapVariant == TechnicalMapVariant.Exploration)
            {
                for (var y = MapOrigin.y; y <= maxY; y++)
                {
                    SetObstacleTile(new Vector3Int(MapOrigin.x, y, 0), wallTile);
                }

                for (var x = 3; x <= 7; x++)
                {
                    SetObstacleTile(new Vector3Int(x, 3, 0), wallTile);
                }
            }
            else
            {
                for (var y = MapOrigin.y; y <= maxY; y++)
                {
                    SetObstacleTile(new Vector3Int(maxX, y, 0), wallTile);
                }

                for (var x = -7; x <= -3; x++)
                {
                    SetObstacleTile(new Vector3Int(x, -1, 0), wallTile);
                }
            }
        }

        private void BuildLandmarks()
        {
            if (mapVariant == TechnicalMapVariant.Exploration)
            {
                BuildTallTree(new Vector3Int(3, 1, 0));
                BuildRock(new Vector3Int(-4, -2, 0));
                return;
            }

            BuildTallTree(new Vector3Int(-6, 2, 0));
            BuildRock(new Vector3Int(4, -3, 0));
        }

        private void EnsureObstacleCollider()
        {
            if (obstacleTilemap.GetComponent<TilemapCollider2D>() == null)
            {
                obstacleTilemap.gameObject.AddComponent<TilemapCollider2D>();
            }
        }

        private void BuildTallTree(Vector3Int cell)
        {
            var root = GetOrCreateGeneratedRoot();
            var tree = new GameObject("TechnicalTreePlaceholder");
            tree.transform.SetParent(root, false);
            tree.transform.position = obstacleTilemap.GetCellCenterWorld(cell) + Vector3.up * 0.58f;

            var visual = tree.AddComponent<SpriteRenderer>();
            visual.sprite = placeholderSprite;
            visual.color = new Color(0.12f, 0.38f, 0.19f);
            tree.transform.localScale = new Vector3(0.85f, 1.6f, 1f);

            var foot = AddFootCollider(root, "TechnicalTreeFoot", cell, 0.22f);
            tree.AddComponent<YSortOrder>().SetFootAnchor(foot);
        }

        private void BuildRock(Vector3Int cell)
        {
            var root = GetOrCreateGeneratedRoot();
            var rock = new GameObject("TechnicalRockPlaceholder");
            rock.transform.SetParent(root, false);
            rock.transform.position = obstacleTilemap.GetCellCenterWorld(cell) + Vector3.up * 0.1f;

            var visual = rock.AddComponent<SpriteRenderer>();
            visual.sprite = placeholderSprite;
            visual.color = new Color(0.42f, 0.43f, 0.43f);
            rock.transform.localScale = new Vector3(0.65f, 0.45f, 1f);

            var foot = AddFootCollider(root, "TechnicalRockFoot", cell, 0.24f);
            rock.AddComponent<YSortOrder>().SetFootAnchor(foot);
        }

        private void SetObstacleTile(Vector3Int cell, Tile wallTile)
        {
            obstacleTilemap.SetTile(cell, wallTile);
        }

        private Transform AddFootCollider(Transform root, string colliderName, Vector3Int cell, float radius)
        {
            var footprint = new GameObject(colliderName);
            footprint.transform.SetParent(root, false);
            footprint.transform.position = obstacleTilemap.GetCellCenterWorld(cell);
            footprint.AddComponent<CircleCollider2D>().radius = radius;
            return footprint.transform;
        }

        private Transform GetOrCreateGeneratedRoot()
        {
            var existingRoot = transform.Find(GeneratedObjectsName);
            if (existingRoot != null)
            {
                return existingRoot;
            }

            var generatedRoot = new GameObject(GeneratedObjectsName);
            generatedRoot.transform.SetParent(transform, false);
            return generatedRoot.transform;
        }

        private Tile CreateTile(Color color, Tile.ColliderType colliderType = Tile.ColliderType.None)
        {
            var tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = tileSprite;
            tile.color = color;
            tile.colliderType = colliderType;
            return tile;
        }

        private Sprite CreateDiamondSprite()
        {
            const int width = 64;
            const int height = 32;

            var texture = new Texture2D(width, height)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp,
                hideFlags = HideFlags.DontSave
            };

            for (var y = 0; y < height; y++)
            {
                var halfWidthAtY = Mathf.Min(y, height - 1 - y) * 4 + 2;
                var startX = (width - halfWidthAtY) / 2;

                for (var x = startX; x < startX + halfWidthAtY; x++)
                {
                    texture.SetPixel(x, y, Color.white);
                }
            }

            texture.Apply();
            return Sprite.Create(texture, new Rect(0f, 0f, width, height), new Vector2(0.5f, 0.5f), width);
        }

        private enum TechnicalMapVariant
        {
            Exploration,
            Outpost
        }
    }
}
