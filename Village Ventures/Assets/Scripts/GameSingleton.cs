using UnityEngine;
using VillageVentures;
using static UnityEditor.Progress;


public class GameSingleton : MonoBehaviour
{
    public static GameSingleton Instance;

    [Header("Prefabs")]
    [SerializeField] GameObject player_prefab;
    [SerializeField] GameObject village_prefab;
    [SerializeField] GameObject shop_prefab;
    [SerializeField] GameObject saloon_prefab;

    [Header("Game")]
    [SerializeField] OutfitList outfits;

    [Header("Audios")]
    [SerializeField] AudioClip mainMenuMusic;
    [SerializeField] AudioClip villageMusic;
    [Space]
    [SerializeField] AudioSource mainAudio;
    [SerializeField] AudioSource doorOpen;

    // SCENES STUFF
    private GameObject villageInstance;
    private GameObject shopInstance;
    private GameObject saloonInstance;

    // PLAYER STUFF
    private GameObject player;
    private Location playerLocation;
    private Inventory playerInventory;

    private readonly string[] buyingLines = { "It's yours!", "Nice look!", "Ok... Thats IT!" };


    private void Awake() => Instance = this;

    void Start()
    {
        GameInterface.Instance.SetupAvailableItemsToSell(outfits);
        GameInterface.OpenMainMenuScreen();
    }



    public void StartGame()
    {
        GameInterface.ShowLoadingScreen();
        GameInterface.CloseMainMenuScreen();

        LoadVillage();
        LoadPlayer();

        mainAudio.Stop();
        mainAudio.clip = villageMusic;
        mainAudio.Play();

        GameInterface.CloseLoadingScreen();
    }
    void LoadVillage() => villageInstance = Instantiate(village_prefab);
    void LoadPlayer()
    {
        player = Instantiate(player_prefab, Vector3.zero, Quaternion.identity);
        playerInventory = player.GetComponent<Inventory>();
    }


    #region Scene changing
    public static void GoTo(Location newLocation)
    {
        switch (newLocation)
        {
            case Location.Shop:
                Instance.GoToShop();
                break;
            case Location.Saloon:
                Instance.GoToSaloon();
                break;
            case Location.Village:
                Instance.GoToVillage();
                break;
        }
        GameInterface.CloseDialog();
    }

    void GoToShop()
    {
        villageInstance.SetActive(false);

        if (shopInstance == null)
            shopInstance = Instantiate(shop_prefab);

        shopInstance.SetActive(true);
        MovePlayerToMatchTransformPosition(shopInstance.transform, "SpawnPosition");
        playerLocation = Location.Shop;
    }
    void GoToVillage()
    {
        villageInstance.SetActive(true);
        switch (playerLocation)
        {
            case Location.Shop:
                shopInstance.SetActive(false);
                MovePlayerToMatchTransformPosition(villageInstance.transform, "OutOfShop");
                break;
            case Location.Saloon:
                saloonInstance.SetActive(false);
                MovePlayerToMatchTransformPosition(villageInstance.transform, "OutOfSaloon");
                break;
        }
        playerLocation = Location.Village;
    }
    void GoToSaloon()
    {
        villageInstance.SetActive(false);

        if (saloonInstance == null)
            saloonInstance = Instantiate(saloon_prefab);

        saloonInstance.SetActive(true);
        MovePlayerToMatchTransformPosition(saloonInstance.transform, "SpawnPosition");
        playerLocation = Location.Saloon;
    }

    void MovePlayerToMatchTransformPosition(Transform root, string childName)
    {
        for (int i = 0; i < root.childCount; i++)
        {
            Transform child = root.GetChild(i);
            if (child.name == childName)
            {
                player.transform.position = child.position;
                break;
            }
        }
    }
    #endregion


    public void TryToBuy(OutfitAnimation item)
    {
        if (playerInventory.Money > item.Cost)
        {
            // TODO Add to inventory
            GameInterface.UIMessage(buyingLines[Random.Range(0, buyingLines.Length)]);
        }
    }
    public void SellItem(OutfitAnimation item)
    {
        if (item.CanSell)
        {
            // Remove item from inventory
            int price = playerInventory.SellItemGetValue(item);

            // Add money to player
            playerInventory.AddMoney(price);
            GameInterface.UIMessage($"You just made ${price}!");
        }
        else
        {
            GameInterface.UIMessage("You cannot sell this item!", MessageMode.Warning);
        }
    }

    public void AddMoneyToPlayer(int money) => playerInventory.AddMoney(money);
}
