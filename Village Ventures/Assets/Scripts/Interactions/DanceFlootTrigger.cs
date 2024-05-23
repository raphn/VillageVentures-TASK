using UnityEngine;

namespace VillageVentures
{
    public class DanceFlootTrigger : MonoBehaviour
    {
        [SerializeField] float maxTime = 3.0f;

        private Movement playerMove;
        private Inventory playerInventory;
        private int worthByDaceSec;

        private bool advisedToLeave;
        private float timer;
        private float earned;


        private void Update()
        {
            if (playerMove && playerMove.IsMoving)
            {
                float delta = Time.deltaTime;
                timer += delta;
                if (timer < maxTime)
                    earned += delta * worthByDaceSec;
                else if (!advisedToLeave)
                {
                    GameInterface.UIMessage("Hey!! You shold leave others dance!", MessageMode.Error);
                    advisedToLeave = true;
                }
            }
        }



        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                timer = 0;
                earned = 0;
                advisedToLeave = false;

                playerMove = collision.GetComponent<Movement>();
                playerInventory = collision.GetComponent<Inventory>();
                worthByDaceSec = playerInventory.GetOutfitGainPerSec();
                GameSingleton.SetDanceMusic(true);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                playerMove = null;
                playerInventory.DecreValues();

                GameSingleton.Instance.AddMoneyToPlayer(Mathf.RoundToInt(earned));
                GameSingleton.SetDanceMusic(false);
            }
        }
    }
}