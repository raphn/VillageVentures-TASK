using UnityEngine;
using VillageVentures;


public class GameSingleton : MonoBehaviour
{
    public static GameSingleton Instance;

    [Header("Player")]
    [SerializeField] GameObject player_prefab;

    [Header("Game")]
    [SerializeField] OutfitList outfits;

    private GameObject player;
    private Inventory playerInventory;

    private string[] JUST_BOUGHT = { "It's yours!", "Nice look!", "Ok... Thats IT!" };


    private void Awake() => Instance = this;

    void Start()
    {
        player = Instantiate(player_prefab, Vector3.zero, Quaternion.identity);
        playerInventory = player.GetComponent<Inventory>();

        GameInterface.Instance.SetupAvailableItemsToSell(outfits);
    }


    public void TryToBuy(OutfitAnimation item)
    {
        if (playerInventory.Money > item.Cost)
        {
            // TODO Add to inventory
            GameInterface.UIMessage(JUST_BOUGHT[Random.Range(0, JUST_BOUGHT.Length)]);
        }
    }

    public void AddMoneyToPlayer(int money) => playerInventory.AddMoney(money);
}
