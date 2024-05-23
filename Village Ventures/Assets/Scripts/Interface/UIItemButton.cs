using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VillageVentures
{
    public class UIItemButton : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI qualityText;

        private OutfitAnimation outfit;


        public void SellItem()
        {
            GameInterface.OpenDialog($"Are you sure you want sell {outfit.name}?", "Sell", () => GameSingleton.Instance.SellItem(outfit));
        }
    }
}