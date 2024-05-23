using TMPro;
using UnityEngine;

namespace VillageVentures
{
    public enum MessageMode { Normal, Warning, Error }

    [RequireComponent (typeof (TextMeshProUGUI))]
    public class Message : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI textMesh;

        public float time;


        public void SetupMessage(string message, MessageMode mode)
        {
            textMesh.text = message;
            textMesh.color = mode switch
            {
                MessageMode.Normal => Color.white,
                MessageMode.Warning => Color.yellow,
                MessageMode.Error => Color.red,
                _ => Color.white,
            };
        }
    }
}