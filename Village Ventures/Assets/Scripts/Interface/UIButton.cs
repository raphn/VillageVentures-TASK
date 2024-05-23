using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace VillageVentures
{
    public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [SerializeField] UnityEvent onClicked;
        [SerializeField] AudioClip hoverAudio;
        [SerializeField] AudioClip pressedAudio;
        [SerializeField] TextMeshProUGUI textMesh;

        private AudioSource audioSource;

        public string Text
        {
            set
            {
                if (textMesh != null)
                    textMesh.text = value;
            }
        }


        private void Awake() => audioSource = gameObject.AddComponent<AudioSource>();

        public void AddOnClickListener(UnityAction action) => onClicked.AddListener(action);
        public void RemoveAllListeners() => onClicked.RemoveAllListeners();


        public void OnPointerEnter(PointerEventData eventData)
        {
            audioSource.clip = hoverAudio;
            audioSource.Play();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            audioSource.clip = pressedAudio;
            audioSource.Play();
            onClicked.Invoke();
        }
    }
}