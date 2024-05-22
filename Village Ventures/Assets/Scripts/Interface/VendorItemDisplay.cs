using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VillageVentures
{
    public class VendorItemDisplay : MonoBehaviour
    {
        [SerializeField] Image iconImage;
        [SerializeField] TextMeshProUGUI costText;

        private OutfitAnimation item;


        public void SetupFromItem(OutfitAnimation item)
        {
            this.item = item;
            iconImage.sprite = item.Icon;
            costText.text = $"${item.Cost}";

            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(Clicked);
            btn.interactable = true;
        }

        public void SetupNoItems() => costText.text = $"$0";

        public void Clicked()
        {
            GameSingleton.Instance.TryToBuy(item);
        }
    }
}