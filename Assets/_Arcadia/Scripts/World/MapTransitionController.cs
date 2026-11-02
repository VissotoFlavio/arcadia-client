using System.Collections;
using Arcadia.Exploration;
using Arcadia.Session;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Arcadia.World
{
    /// <summary>
    /// Coordinates a portal-driven map transition without exposing a loading screen.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class MapTransitionController : MonoBehaviour
    {
        private const int TransitionCanvasSortingOrder = 1000;

        [SerializeField, Min(0f)] private float fadeDuration = 0.2f;

        private readonly GameSession gameSession = new();
        private static MapTransitionController instance;

        private CanvasGroup fadeGroup;
        private bool transitionInProgress;

        /// <summary>
        /// Gets the duration of each fade half-cycle in seconds.
        /// </summary>
        public float FadeDuration => fadeDuration;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
            CreateFadeOverlay();
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void Start()
        {
            BindPortalsInLoadedScene();
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            if (instance == this)
            {
                instance = null;
            }
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            BindPortalsInLoadedScene();
        }

        private void BindPortalsInLoadedScene()
        {
            foreach (var portal in FindObjectsByType<Portal>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
            {
                portal.TransitionRequested -= HandlePortalTransitionRequested;
                portal.TransitionRequested += HandlePortalTransitionRequested;
            }
        }

        private void HandlePortalTransitionRequested(PortalTransitionRequest request)
        {
            if (transitionInProgress)
            {
                return;
            }

            StartCoroutine(TransitionToMap(request));
        }

        private IEnumerator TransitionToMap(PortalTransitionRequest request)
        {
            transitionInProgress = true;

            var player = FindFirstSceneObject<PlayerMotor>();
            if (player != null)
            {
                player.enabled = false;
            }

            yield return FadeTo(1f);

            AsyncOperation loadOperation = null;
            System.Exception loadException = null;
            try
            {
                loadOperation = SceneManager.LoadSceneAsync(
                    request.DestinationMap.SceneReference,
                    LoadSceneMode.Single);
            }
            catch (System.Exception exception)
            {
                loadException = exception;
            }

            if (loadException != null)
            {
                Debug.LogException(loadException, this);
                yield return RecoverFromFailedTransition(player);
                yield break;
            }

            while (loadOperation is { progress: < 0.9f })
            {
                yield return null;
            }

            if (loadOperation == null)
            {
                Debug.LogError("The destination scene could not be loaded.", this);
                yield return RecoverFromFailedTransition(player);
                yield break;
            }

            loadOperation.allowSceneActivation = true;
            while (!loadOperation.isDone)
            {
                yield return null;
            }

            if (!TryPlacePlayerAtSpawnPoint(request.DestinationSpawnPointId, out var destinationPosition))
            {
                Debug.LogError(
                    $"The destination spawn point '{request.DestinationSpawnPointId}' was not found.",
                    this);
            }

            gameSession.SetExplorationLocation(new ExplorationLocation(
                request.DestinationMap.MapId,
                request.DestinationSpawnPointId,
                new MapPosition(destinationPosition.x, destinationPosition.y)));

            if (player != null)
            {
                player = FindFirstSceneObject<PlayerMotor>();
                if (player != null)
                {
                    player.enabled = true;
                }
            }

            yield return FadeTo(0f);
            transitionInProgress = false;
        }

        private IEnumerator RecoverFromFailedTransition(PlayerMotor player)
        {
            if (player != null)
            {
                player.enabled = true;
            }

            yield return FadeTo(0f);
            transitionInProgress = false;
        }

        private bool TryPlacePlayerAtSpawnPoint(string spawnPointId, out Vector3 destinationPosition)
        {
            foreach (var spawnPoint in FindObjectsByType<SpawnPoint>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
            {
                if (string.Equals(spawnPoint.SpawnPointId, spawnPointId, System.StringComparison.Ordinal))
                {
                    destinationPosition = spawnPoint.transform.position;
                    var player = FindFirstSceneObject<PlayerMotor>();
                    if (player != null)
                    {
                        player.transform.position = destinationPosition;
                        var body = player.GetComponent<Rigidbody2D>();
                        if (body != null)
                        {
                            body.position = new Vector2(destinationPosition.x, destinationPosition.y);
                            body.linearVelocity = Vector2.zero;
                        }
                    }

                    return true;
                }
            }

            destinationPosition = Vector3.zero;
            return false;
        }

        private static T FindFirstSceneObject<T>() where T : Object
        {
            var objects = FindObjectsByType<T>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            return objects.Length > 0 ? objects[0] : null;
        }

        private void CreateFadeOverlay()
        {
            var fadeCanvasObject = new GameObject("MapTransitionFade");
            fadeCanvasObject.transform.SetParent(transform, false);

            var canvas = fadeCanvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = TransitionCanvasSortingOrder;

            var scaler = fadeCanvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            var group = fadeCanvasObject.AddComponent<CanvasGroup>();
            group.alpha = 0f;
            group.blocksRaycasts = false;
            fadeGroup = group;

            var image = new GameObject("FadeImage").AddComponent<Image>();
            image.transform.SetParent(fadeCanvasObject.transform, false);
            image.color = Color.black;
            image.raycastTarget = false;

            var rect = image.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        private IEnumerator FadeTo(float targetAlpha)
        {
            if (fadeGroup == null)
            {
                yield break;
            }

            var startAlpha = fadeGroup.alpha;
            if (fadeDuration <= 0f)
            {
                fadeGroup.alpha = targetAlpha;
                yield break;
            }

            var elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                fadeGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
                yield return null;
            }

            fadeGroup.alpha = targetAlpha;
        }
    }
}
