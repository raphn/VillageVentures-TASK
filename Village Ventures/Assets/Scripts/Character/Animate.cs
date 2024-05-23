using Unity.VisualScripting;
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
            public string label = "Body";
            [Tooltip("Renderer that will have its sprite changed")]
            public SpriteRenderer renderer;

            [HideInInspector] public ItemStack item;
            public OutfitAnimation animation;

            public bool HasRenderer => renderer != null;

            public void UpdateRenderer(bool moving, Direction dir, float delta)
            {
                if (animation)
                    animation.SetSprite(renderer, moving, dir, delta);
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


        
        public bool Equip(ItemStack stack)
        {
            for (int i = 0;i < outfits.Length;i++)
            {
                if (outfits[i].label == stack.item.EquipTo)
                {
                    if (outfits[i].item != null)
                        outfits[i].item.equiped = false;

                    outfits[i].animation = stack.item;
                    outfits[i].item = stack;
                    stack.equiped = true;

                    GameInterface.UIMessage($"{stack.item.name} equiped!");
                    return true;
                }
            }
            GameInterface.UIMessage($"Could not equip {stack.item.name}, to equipt at: {stack.item.EquipTo}!", MessageMode.Error);
            return false;
        }

        public bool Unequip(ItemStack stack, ItemStack substitute = null)
        {
            for (int i = 0; i < outfits.Length; i++)
            {
                if (outfits[i].item == stack)
                {
                    outfits[i].item.equiped = false;


                    outfits[i].animation = substitute == null ? null : substitute.item;
                    outfits[i].item = substitute;
                    if (substitute != null)
                        substitute.equiped = true;
                    return true;
                }
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