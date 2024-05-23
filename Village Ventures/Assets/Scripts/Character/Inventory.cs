using System.Collections.Generic;
using UnityEngine;

namespace VillageVentures
{
    [System.Serializable]
    public struct ItemStack
    {
        public ItemStack(OutfitAnimation item)
        {
            this.item = item;
            quality = 100;
        }

        public OutfitAnimation item;
        public int quality;

        public readonly int CurrentPrice
        {
            get
            {
                float usePercent = 1f / (float)quality;
                return Mathf.CeilToInt(usePercent * item.Cost);
            }
        }
    }

    public class Inventory : MonoBehaviour
    {
        [SerializeField] int money = 50;
        [Space]
        [SerializeField] ItemStack[] items;

        public int Money => money;


        public void AddMoney(int money)
        {
            this.money += money;
        }

        public int SellItemGetValue(OutfitAnimation item)
        {
            List<ItemStack> newStack = new List<ItemStack>();
            int currentPrice = 0;
            
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].item == item)
                    currentPrice = items[i].CurrentPrice;
                else
                    newStack.Add(items[i]);
            }

            items = newStack.ToArray();
            return currentPrice;
        }
    }
}