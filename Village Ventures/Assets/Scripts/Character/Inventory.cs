using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace VillageVentures
{
    [System.Serializable]
    public class ItemStack
    {
        public ItemStack(OutfitAnimation item, Inventory owner)
        {
            this.item = item;
            quality = 100;
            this.owner = owner;
        }

        [SerializeField] Inventory owner;
        public OutfitAnimation item;
        public int quality;
        public bool equiped;

        public int CurrentPrice
        {
            get
            {
                float usePercent = (float)quality/100.0f;
                return Mathf.CeilToInt(usePercent * item.Cost);
            }
        }
        public int CurrentEarnPerSec
        {
            get
            {
                float usePercent = 1f / (float)quality;
                return Mathf.CeilToInt(usePercent * item.EarnPerSec);
            }
        }
        public Inventory Owner => owner;


        public void Equip()
        {
            if (owner != null)
                owner.Equip(this);
        }
        public void DecreaseValue() => quality = Mathf.Max(0, quality - 20);
    }

    [RequireComponent(typeof(Animate))]
    public class Inventory : MonoBehaviour
    {
        [SerializeField] int money = 50;
        [SerializeField] Animate animator;
        [Space]
        [SerializeField] ItemStack[] items;

        public int Money => money;
        public ItemStack[] CurrentItems => items;


        private void Start()
        {
            animator = GetComponent<Animate>();

            // Equipping all the default items
            for (int i = 0; i < items.Length; i++)
                animator.Equip(items[i]);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                GameSingleton.Instance.ToggleInventory();
        }


        public void AddMoney(int money)
        {
            this.money += money;
        }

        public void BuyItem(OutfitAnimation item)
        {
            money -= item.Cost;
            AddItem(item);
        }
        void AddItem(OutfitAnimation item)
        {
            ItemStack newStack = new (item, this);
            List<ItemStack> newList = new(items)
            {
                newStack
            };
            items = newList.ToArray();
        }


        public void DecreValues()
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].equiped)
                    items[i].DecreaseValue();
            }
        }
        public int GetOutfitGainPerSec()
        {
            int gain = 0;
            for (int i = 0;i < items.Length; i++)
            {
                if (items[i].equiped)
                    gain += items[i].CurrentEarnPerSec;
            }
            return gain;
        }


        public void Equip(ItemStack item) => animator.Equip(item);
        public int SellItemGetValue(ItemStack item)
        {
            List<ItemStack> newStack = new List<ItemStack>();
            int currentPrice = 0;
            
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == item)
                {
                    if (items[i].equiped)
                        animator.Unequip(items[i]);
                    currentPrice = items[i].CurrentPrice;
                }
                else
                    newStack.Add(items[i]);
            }

            items = newStack.ToArray();
            return currentPrice;
        }
    }
}