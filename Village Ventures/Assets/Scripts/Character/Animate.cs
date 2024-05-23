using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Windows.WebCam;

namespace VillageVentures
{
    public enum Direction { Left, Right, Up, Down }

    [RequireComponent(typeof(Movement))]
    [RequireComponent (typeof(SpriteRenderer))]
    public class Animate : MonoBehaviour
    {
        /// <summary>
        /// Hold the individualized references to the outfits renderers and sprite animations
        /// </summary>
        [System.Serializable]
        public class AnimationSetup
        {
            public string label = "Animation";
            [Tooltip("Renderer that will have its sprite changed")]
            public SpriteRenderer renderer;
            [Tooltip("Animation sprite asset to animate")]
            public OutfitAnimation outfit;

            public bool HasRenderer => renderer != null;

            public void UpdateRenderer(bool moving, Direction dir, float delta)
            {
                if (outfit)
                    outfit.SetSprite(renderer, moving, dir, delta);
                else
                    renderer.enabled = false;
            }
        }

        [SerializeField] private int spritePerSec = 5;
        [SerializeField] private OutfitAnimation character;

        [Header("Clothing")]
        [SerializeField] private AnimationSetup[] outfits;

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

            float animSpeed = Time.deltaTime * spritePerSec;
            if (character)
                character.SetSprite(sRenderer, moving, direction, animSpeed);
            else
            {
                Debug.LogError("Main body not asigned!");
                return;
            }
                
            for (int i = 0; i < outfits.Length; i++)
            {
                if (outfits[i].HasRenderer)
                    outfits[i].UpdateRenderer(moving, direction, animSpeed);

            }
        }

        public bool CheckIsEquiped(OutfitAnimation outfit)
        {
            for (int i = 0;i < outfits.Length; i++)
            {
                if (outfits[i].outfit == outfit)
                    return true;
            }
            return false;
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