using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace Arcadia.Input
{
    /// <summary>Instantiates the editable joystick prefab that writes to the virtual Gamepad stick.</summary>
    public sealed class VirtualJoystickView : MonoBehaviour
    {
        [SerializeField] private GameObject joystickPrefab;

        private void Awake()
        {
            if (!Application.isPlaying || FindAnyObjectByType<OnScreenStick>() != null)
            {
                return;
            }

            EnsureEventSystem();
            Instantiate(joystickPrefab, CreateCanvas().transform);
        }

        private static void EnsureEventSystem()
        {
            if (FindAnyObjectByType<EventSystem>() == null)
            {
                new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            }
        }

        private static Canvas CreateCanvas()
        {
            var canvasObject = new GameObject("ArcadiaInputCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;

            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;
            return canvas;
        }
    }
}
