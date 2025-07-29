using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Match3
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputReader : MonoBehaviour
    {
        PlayerInput playerInput;
        InputAction positionAction;
        InputAction downAction;

        public event Action onDown;
        public event Action onUp;

        public Vector2 Selected => positionAction.ReadValue<Vector2>();
        void OnSelect(InputAction.CallbackContext obj) => onDown?.Invoke();
        void OnRelease(InputAction.CallbackContext obj) => onUp?.Invoke();

        void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            positionAction = playerInput.actions["Position"];
            downAction = playerInput.actions["Down"];

            downAction.started += OnSelect;
            downAction.canceled += OnRelease;
        }

        void OnDestroy()
        {
            downAction.performed -= OnSelect;
        }
    }
}

