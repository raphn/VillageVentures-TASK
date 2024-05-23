using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VillageVentures
{
    public class VendorItemDisplay : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] Image iconImage;
        [SerializeField] TextMeshProUGUI costText;
        [SerializeField] AudioClip hoverAudio;

        private OutfitAnimation item;
        private AudioSource audioSource;


        public void SetupFromItem(OutfitAnimation item)
        {
            this.item = item;
            iconImage.sprite = item.Icon;
            costText.text = $"${item.Cost}";

            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(Clicked);
            btn.interactable = true;

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = hoverAudio;
        }

        public void SetupNoItems() => costText.text = $"$0";

        public void Clicked() => GameSingleton.Instance.TryToBuy(item);


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (audioSource)
                audioSource.Play();
        }
    }
}