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


    private void Awake() => Instance = this;

    void Start()
    {
        player = Instantiate(player_prefab, Vector3.zero, Quaternion.identity);
        playerInventory = player.GetComponent<Inventory>();
    }


    public void AddMoneyToPlayer(int money) => playerInventory.AddMoney(money);
}
