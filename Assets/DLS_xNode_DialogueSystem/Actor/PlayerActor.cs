using DLS.Dialogue;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DLS.Core
{
    public class PlayerActor : ActorController
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Vector2 movement;
        private Rigidbody2D rb;
        private PlayerInputActions playerInput;

        private void Awake()
        {
            playerInput = new PlayerInputActions();
            rb = GetComponent<Rigidbody2D>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            playerInput.Player.Move.performed += MoveOnperformed;
            playerInput.Player.Move.canceled += MoveOncanceled;
            playerInput.Player.Interact.performed += InteractOnperformed;
            playerInput.Enable();
        }

        private void InteractOnperformed(InputAction.CallbackContext input)
        {
            Interact();
        }

        private void MoveOncanceled(InputAction.CallbackContext input)
        {
            movement = Vector2.zero;
        }

        private void MoveOnperformed(InputAction.CallbackContext input)
        {
            if (!isMovementDisabled)
            {
                movement = input.ReadValue<Vector2>();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            playerInput.Player.Move.performed -= MoveOnperformed;
            playerInput.Player.Move.canceled -= MoveOncanceled;
            playerInput.Player.Interact.performed -= InteractOnperformed;
            playerInput.Disable();
        }

        private void FixedUpdate() => rb.position += movement * (moveSpeed * Time.fixedDeltaTime);

        protected override void OnOnDialogueInteract(GameObject source, GameObject target)
        {
            if (gameObject == source)
            {
                isInteracting = true;
                isMovementDisabled = true;
            }
        }
    }
}