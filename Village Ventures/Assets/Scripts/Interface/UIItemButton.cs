using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VillageVentures
{
    public class UIItemButton : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI qualityText;
        [SerializeField] TextMeshProUGUI priceText;
        [Space]
        [SerializeField] GameObject equipBtn;
        [SerializeField] GameObject equipedText;

        private ItemStack outfit;


        public void SetupFromItemStack(ItemStack stack)
        {
            outfit = stack;
            icon.sprite = outfit.item.Icon;
            qualityText.text = $"{outfit.quality}%";
            priceText.text = $"${stack.CurrentPrice}";

            equipBtn.SetActive(!stack.equiped);
            equipedText.SetActive(stack.equiped);
        }

        public void SellItem() => GameInterface.OpenDialog($"Are you sure you want sell {outfit.item.name}?", Sell);
        private void Sell() => GameSingleton.Instance.SellItem(outfit);

        public void EquipItem() => outfit.Equip();
    }
}