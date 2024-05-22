using UnityEngine;

namespace VillageVentures
{
    [System.Serializable]
    public class MarketInterfaceUpdater
    {
        [SerializeField] Transform items_root;


        public void SetupFromItemsList(OutfitList list)
        {
            int totalDisplays = items_root.childCount;
            VendorItemDisplay[] displays = items_root.GetComponentsInChildren<VendorItemDisplay>();
            OutfitAnimation[] items = list.GetAllItems();

            for (int i = 0; i < totalDisplays; i++)
            {
                if (i >= items.Length - 1)
                    displays[i].SetupNoItems();
                else
                    displays[i].SetupFromItem(items[i]);
            }
        }
    }
}