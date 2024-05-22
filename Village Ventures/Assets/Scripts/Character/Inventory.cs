using UnityEngine;

namespace VillageVentures
{
    [System.Serializable]
    public struct ItemStack
    {
        public ItemStack(OutfitAnimation item)
        {
            this.item = item;
            quantity = 1;
        }

        public OutfitAnimation item;
        public int quantity;
    }

    public class Inventory : MonoBehaviour
    {
        [SerializeField] int money = 50;
        [SerializeField] ItemStack[] items;

        public int Money => money;


        public void AddMoney(int money)
        {
            this.money += money;
        }
    }
}