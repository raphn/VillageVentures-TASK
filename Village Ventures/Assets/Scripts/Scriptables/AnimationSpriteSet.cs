using UnityEngine;

namespace VillageVentures
{
    //Creating new scriptable allows me to manipulate the animation faster in code rather than Animation Controller and the project keeps scalability.
    [CreateAssetMenu(fileName = "NewOutfit", menuName = "Assets/Outfit")]
    public class OutfitAnimation : ScriptableObject
    {
        [SerializeField] private int cost = 50;

        [Header("Idle Animation Sprites")]
        public Sprite[] idle_down;
        public Sprite[] idle_up;
        public Sprite[] idle_right;
        public Sprite[] idle_left;

        [Header("Walk Animation Sprites")]
        public Sprite[] walk_right;
        public Sprite[] walk_left;
        public Sprite[] walk_up;
        public Sprite[] walk_down;

        private bool enabled = true;
        private bool isMovingTrack = false;
        private Direction currentDirection;
        private int currentIndex = 0;
        private float timer = 0f;

        public int Cost => cost;
        public Sprite Icon => idle_down[0];
        //public string Label => label;


        public void SetSprite(SpriteRenderer renderer, bool moving, Direction dir, float delta)
        {
            bool restartAnimation = false;

            // Returns if this item is not been showed
            if (enabled != renderer.enabled)
            {
                renderer.enabled = enabled;
                if (!enabled)
                    return;
                else
                    restartAnimation = true;
            }

            // Restarts the animation if the direction changed or started/stropped moving
            if (!restartAnimation)
                restartAnimation = moving != isMovingTrack || dir != currentDirection;

            if (restartAnimation)
            {
                currentIndex = 0;
                timer = 0f;

                isMovingTrack = moving;
                currentDirection = dir;

                renderer.sprite = GetSprite();
            }
            else
            {
                timer += delta;
                if (timer > 1f)
                {
                    IncrementIndex();
                    timer = 0f;
                }
                renderer.sprite = GetSprite();
            }
        }

        private void IncrementIndex()
        {
            currentIndex++;
            if (currentIndex >= GetAnimLength())
                currentIndex = 0;

        }
        private int GetAnimLength()
        {
            if (isMovingTrack)
            {
                return currentDirection switch
                {
                    Direction.Left => walk_left.Length,
                    Direction.Right => walk_right.Length,
                    Direction.Up => walk_up.Length,
                    Direction.Down => walk_down.Length,
                    _ => walk_down.Length,
                };
            }
            else
            {
                return currentDirection switch
                {
                    Direction.Left => idle_left.Length,
                    Direction.Right => idle_right.Length,
                    Direction.Up => idle_up.Length,
                    Direction.Down => idle_down.Length,
                    _ => idle_down.Length,
                };
            }
        }
        private Sprite GetSprite()
        {
            if (isMovingTrack)
            {
                return currentDirection switch
                {
                    Direction.Left => walk_left[currentIndex],
                    Direction.Right => walk_right[currentIndex],
                    Direction.Up => walk_up[currentIndex],
                    Direction.Down => walk_down[currentIndex],
                    _ => walk_down[currentIndex],
                };
            }
            else
            {
                return currentDirection switch
                {
                    Direction.Left => idle_left[currentIndex],
                    Direction.Right => idle_right[currentIndex],
                    Direction.Up => idle_up[currentIndex],
                    Direction.Down => idle_down[currentIndex],
                    _ => idle_down[currentIndex],
                };
            }
        }
    }
}