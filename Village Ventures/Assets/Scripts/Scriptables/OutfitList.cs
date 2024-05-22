using UnityEngine;

namespace VillageVentures
{
    [CreateAssetMenu(fileName = "OutfitList", menuName = "Outfit List")]
    public class OutfitList : ScriptableObject
    {
        [SerializeField] private OutfitAnimation[] outfits;


        public void GetOutfit(string name = "")
        {
            for (int i = 0; i < outfits.Length; i++)
            {
                Debug.Log(outfits[i].name);
            }
        }
        public OutfitAnimation[] GetAllItems() => outfits;
    }
}