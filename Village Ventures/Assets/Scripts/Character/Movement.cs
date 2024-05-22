using UnityEngine;

namespace VillageVentures
{
    [RequireComponent (typeof (Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5f; // Speed of the player movement

        private Rigidbody2D rb;
        private Vector2 movement;

        public bool blockMovement;

        public bool IsMoving => movement.magnitude > 0.1f;
        public Vector2 MoveInput => movement;


        void Start() => rb = GetComponent<Rigidbody2D>();

        void Update()
        {
            if (blockMovement)
            {
                movement = Vector2.zero;
                return;
            }

            // Get input
            movement.x = Input.GetAxis("Horizontal"); // Gets input from A/D keys or Left/Right arrow keys
            movement.y = Input.GetAxis("Vertical");   // Gets input from W/S keys or Up/Down arrow keys
        }

        void FixedUpdate() => rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * movement);
    }
}