using UnityEngine;

namespace VillageVentures
{
    public class Interactions : MonoBehaviour
    {
        [SerializeField] int money = 50;

        public int Money => money;


        public void AddMoney(int money)
        {
            this.money += money;
        }
    }
}