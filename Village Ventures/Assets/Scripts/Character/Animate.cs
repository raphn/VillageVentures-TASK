using UnityEngine;

namespace VillageVentures
{
    public enum Direction { Left, Right, Up, Down }

    [RequireComponent(typeof(Movement))]
    [RequireComponent (typeof(SpriteRenderer))]
    public class Animate : MonoBehaviour
    {
        [SerializeField]
        private int spritePerSec = 5;

        [SerializeField]
        private AnimationSpriteSet character;

        [Header("Clothing")]
        [SerializeField]
        private AnimationSpriteSet outfit;
        [SerializeField]
        private AnimationSpriteSet head;

        private Direction direction;
        private Movement movement;
        private SpriteRenderer sRenderer;
        

        void Start()
        {
            movement = GetComponent<Movement>();
            sRenderer = GetComponent<SpriteRenderer>();
        }

        void LateUpdate()
        {
            bool moving = movement.IsMoving;
            if (moving)
                direction = GetMovementDirection();

            if (character)
                character.SetSprite(sRenderer, moving, direction, Time.deltaTime * spritePerSec);
            else
                Debug.LogError("Character with no body Sprites!");
        }

        Direction GetMovementDirection()
        {
            Vector2 move = movement.MoveInput;
            if (move.y > 0 )
                return Direction.Up;
            else if (move.y < 0)
                return Direction.Down;
            else if (move.x < 0)
                return Direction.Left;
            else
                 return Direction.Right;
        }
    }
}