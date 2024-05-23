using UnityEngine;

namespace VillageVentures
{
    [System.Serializable]
    public class MoveSprites
    {
        public Sprite body;
        public Sprite outfit;
        public Sprite maleHair;
        public Sprite femaleHair;

        /*private int outfitSelected;
        private int hairSelected;


        public void RandomizeAll()
        {
            outfitSelected = Random.Range(0, outfit.Length);
            hairSelected = Random.Range(0, hair.Length);
        }*/

        public void SetSprites(SpriteRenderer bodyRenderer, SpriteRenderer outfitRenderer, SpriteRenderer hairRenderer, bool male)
        {
            bodyRenderer.sprite = body;
            outfitRenderer.sprite = outfit;
            hairRenderer.sprite = male ? maleHair : femaleHair;

        }
    }

    [CreateAssetMenu(fileName = "CrazyMove", menuName = "Assets/Crazy Move")]
    public class CrazyMove : ScriptableObject
    {
        [SerializeField] MoveSprites[] move;

        private int currentIndex = 0;
        private float timer = 0f;


        public void SetSprite(SpriteRenderer bodyRenderer, SpriteRenderer outfitRenderer, SpriteRenderer hairRenderer, float delta, bool male)
        {
            timer += delta;
            if (timer > 1f)
            {
                IncrementIndex();
                timer = 0f;
            }

            move[currentIndex].SetSprites(bodyRenderer, outfitRenderer, hairRenderer, male);
        }
        private void IncrementIndex()
        {
            currentIndex++;
            if (currentIndex >= move.Length)
                currentIndex = 0;
        }
    }
}