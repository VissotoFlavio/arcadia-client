using UnityEngine;
using UnityEngine.InputSystem;

namespace Arcadia.Input
{
    /// <summary>Owns exploration input without knowing about the player.</summary>
    public sealed class ExplorationInputReader : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;

        private InputAction moveAction;

        public Vector2 Move => moveAction?.ReadValue<Vector2>() ?? Vector2.zero;

        private void OnEnable()
        {
            moveAction = inputActions?.FindAction("Exploration/Move", throwIfNotFound: true);
            moveAction?.Enable();
        }

        private void OnDisable()
        {
            moveAction?.Disable();
        }
    }
}
