using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace Arcadia.Input
{
    /// <summary>Creates the temporary joystick and writes to the virtual Gamepad stick.</summary>
    public sealed class VirtualJoystickView : MonoBehaviour
    {
        private const float BaseSize = 192f;
        private const float StickSize = 96f;
        private const float MovementRange = 72f;

        private void Awake()
        {
            if (!Application.isPlaying || FindAnyObjectByType<OnScreenStick>() != null)
            {
                return;
            }

            EnsureEventSystem();
            CreateJoystick();
        }

        private static void EnsureEventSystem()
        {
            if (FindAnyObjectByType<EventSystem>() == null)
            {
                new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            }
        }

        private static void CreateJoystick()
        {
            var canvasObject = new GameObject("ArcadiaInputCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;

            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            var background = CreateImage("VirtualJoystick", canvasObject.transform, BaseSize, new Color(0.08f, 0.15f, 0.23f, 0.42f));
            var backgroundTransform = background.rectTransform;
            backgroundTransform.anchorMin = Vector2.zero;
            backgroundTransform.anchorMax = Vector2.zero;
            backgroundTransform.pivot = new Vector2(0.5f, 0.5f);
            backgroundTransform.anchoredPosition = new Vector2(144f, 144f);
            background.raycastTarget = false;

            var stick = CreateImage("Stick", background.transform, StickSize, new Color(0.25f, 0.7f, 0.95f, 0.8f));
            stick.rectTransform.anchoredPosition = Vector2.zero;

            var onScreenStick = stick.gameObject.AddComponent<OnScreenStick>();
            onScreenStick.controlPath = "<Gamepad>/leftStick";
            onScreenStick.movementRange = MovementRange;
            onScreenStick.behaviour = OnScreenStick.Behaviour.RelativePositionWithStaticOrigin;
        }

        private static Image CreateImage(string name, Transform parent, float size, Color color)
        {
            var gameObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            gameObject.transform.SetParent(parent, false);

            var image = gameObject.GetComponent<Image>();
            image.color = color;
            image.rectTransform.sizeDelta = new Vector2(size, size);
            return image;
        }
    }
}
