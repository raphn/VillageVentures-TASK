using UnityEngine;

namespace VillageVentures
{
    public class DancerNPC : MonoBehaviour
    {
        [SerializeField] SpriteRenderer bodyRenderer;
        [SerializeField] SpriteRenderer outfirRenderer;
        [SerializeField] SpriteRenderer headRenderer;
        [Space]
        [SerializeField] float speed = 8.0f;
        [SerializeField] float speedVariation = 2.0f;
        [Space]
        [SerializeField] Color[] bodyColors;
        [SerializeField] Color[] outfitColors;
        [SerializeField] Color[] hairfitColors;
        [SerializeField] CrazyMove[] moves;

        private bool male;
        private float finalSpeed;
        private CrazyMove selected;


        public void StartMoving()
        {
            selected = moves[Random.Range(0, moves.Length)];
            finalSpeed = speed + Random.Range(-speedVariation, speedVariation);

            bodyRenderer.color = bodyColors[Random.Range(0, bodyColors.Length)];
            outfirRenderer.color = outfitColors[Random.Range(0, outfitColors.Length)];
            headRenderer.color = hairfitColors[Random.Range(0, hairfitColors.Length)];

            male = Random.Range(0.0f, 1.0f) > 0.5f;
        }

        private void Update() => selected.SetSprite(bodyRenderer, outfirRenderer, headRenderer, Time.deltaTime * finalSpeed, male);
    }
}