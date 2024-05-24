using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VillageVentures;

public class GameInterface : MonoBehaviour
{
    public static GameInterface Instance;

    [Header("Transition")]
    [SerializeField] float inOutTransitTime = 0.25f;
    [SerializeField] CanvasGroup group;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] GameObject mainMenuScreen;

    [Header("Info Displays")]
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI playerMoney;
    [Space]
    [SerializeField] MarketInterfaceUpdater marketInterface;
    
    [Header("Inventory")]
    [SerializeField] UIItemButton itemDisplayPrefab;
    [SerializeField] RectTransform inventoryDisplayRoot;
    [SerializeField] CanvasGroup inventoryCanvasGroup;
    [SerializeField] TextMeshProUGUI cashDisplay;

    [Header("Dialogs")]
    [SerializeField] GameObject dialogGO;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] UIButton dialogAcceptButton;
    [SerializeField] UIButton dialogCancelButton;

    [Header("UI Message")]
    [SerializeField] float msnTime = 2.0f;
    [SerializeField] Message msnPref;
    [SerializeField] Transform msnParent;
    [Space]
    [SerializeField] AudioSource errorAudio;

    private bool transitioning = false;
    private bool transitioningCall = false;
    private bool afterTransitState = false;
    private CountDownTimer timer;

    private bool inventTransiting = false;
    private bool inventTransitioningCall = false;
    private bool afterInventTransitState = false;
    private CountDownTimer inventoryTimer;

    private GameObject dispose;

    private List<Message> messages = new List<Message>();

    public bool VendorInterfaceOn => group.interactable;
    public string PlayerName { set { playerName.text = value; } }
    public int PlayerMoney
    {
        set
        {
            string moneyStr = $"${value}";
            playerMoney.text = moneyStr;
            cashDisplay.text = moneyStr;
        }
    }


    // SETUP & CALLBACKS
    private void Awake() => Instance = this;
    public void SetupAvailableItemsToSell(OutfitList list) => marketInterface.SetupFromItemsList(list);
    private void Start()
    {
        group.interactable = group.blocksRaycasts = false;
        group.alpha = 0f;

        inventoryCanvasGroup.interactable = inventoryCanvasGroup.blocksRaycasts = false;
        inventoryCanvasGroup.alpha = 0f;

        dispose = new GameObject("disposed_ui");
        dispose.transform.parent = transform;
        dispose.SetActive(false);
        StartCoroutine(CleanupDispose());
    }
    private void Update()
    {
        timer?.Update(Time.deltaTime);
        inventoryTimer?.Update(Time.deltaTime);

        float delta = Time.deltaTime;
        for (int i = 0; i < messages.Count; i++)
        {
            var item = messages[i];
            item.time += delta;
            if (item.time > msnTime)
            {
                messages.Remove(item);
                DestroyImmediate(item.gameObject);
            }
        }
    }

    private IEnumerator CleanupDispose()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        // Clean current buttons
        int currentBtnsTotal = dispose.transform.childCount;
        for (int i = 0; i < currentBtnsTotal; i++)
            Destroy(dispose.transform.GetChild(0).gameObject);

        StartCoroutine(CleanupDispose());
    }

    #region Call main interface ON/OFF
    public void CallInterface()
    {
        if (transitioning)
        {
            transitioningCall = true;
            afterTransitState = true;
            return;
        }
        timer = new CountDownTimer(inOutTransitTime, () => InterfaceTransitionDone(true), ChangingInterfaceState);
        transitioning = true;
        ShowInventoryRefresh();
    }
    public void CloseInterface()
    {
        if (transitioning)
        {
            transitioningCall = true;
            afterTransitState = false;
            return;
        }
        timer = new CountDownTimer(inOutTransitTime, () => InterfaceTransitionDone(false), ChangingInterfaceState);
        transitioning = true;
        CloseInventory();
    }
    
    private void ChangingInterfaceState(float progress) => group.alpha = progress;
    private void InterfaceTransitionDone(bool isOn)
    {
        // In case there were any call for a transition during a transition
        if (transitioningCall && afterTransitState != isOn)
        {
            if (isOn)
                CallInterface();
            else
                CloseInterface();
        }
        else
        {
            group.interactable = group.blocksRaycasts = isOn;
            group.alpha = isOn ? 1f : 0f;
        }
        transitioning = false;
        transitioningCall = false;
        timer = null; // Discart timer
    }
    #endregion


    // INTERACTIONS & MESSAGES
    public static void UIMessage(string message, MessageMode mode = MessageMode.Normal)
    {
        if (Instance)
        {
            Message m = Instantiate(Instance.msnPref, Instance.msnParent).GetComponent<Message>();
            m.SetupMessage(message, mode);
            Instance.messages.Add(m);
            if (mode == MessageMode.Error || mode == MessageMode.Warning)
                Instance.errorAudio.Play();
        }
    }
    
    public static void OpenDialog(string message, UnityAction onAccept, string acceptText = "Accept", string cancel = "Cancel")
    {
        if (!Instance)
            return;

        Instance.dialogGO.SetActive(true);
        Instance.dialogText.text = message;
        Instance.dialogAcceptButton.Text = acceptText;
        Instance.dialogAcceptButton.AddOnClickListener(onAccept);
        Instance.dialogCancelButton.Text = cancel;
    }
    public static void CloseDialog()
    {
        if (!Instance)
            return;

        Instance.dialogGO.SetActive(false);
        Instance.dialogAcceptButton.RemoveAllListeners();
    }
    public void CloseDialogPlaySoud()
    {
        dialogGO.SetActive(false);
        dialogAcceptButton.RemoveAllListeners();
        errorAudio.Play();
    }


    // OPEN CLOSE WINDOWS
    public static void OpenMainMenuScreen()
    {
        if (Instance)
            Instance.mainMenuScreen.SetActive(true);
    }
    public static void CloseMainMenuScreen()
    {
        if (Instance)
            Instance.mainMenuScreen.SetActive(false);
    }

    public static void ShowLoadingScreen()
    {
        if (Instance)
            Instance.loadingScreen.SetActive(true);
    }
    public static void CloseLoadingScreen()
    {
        if (Instance)
            Instance.loadingScreen.SetActive(false);
    }


    // DISPLAY INVENTORY
    public static void ShowInventoryRefresh()
    {
        RefreshPlayerInventoryDisplay();

        if (Instance.inventTransiting)
        {
            Instance.inventTransitioningCall = true;
            Instance.afterInventTransitState = true;
            return;
        }
        Instance.inventoryTimer = new CountDownTimer(Instance.inOutTransitTime, () => Instance.InventoryDisplayTransitionDone(true), Instance.ChangingInventoryDisplayState);
        Instance.inventTransiting = true;
    }
    public static void RefreshPlayerInventoryDisplay()
    {
        // Get Inventory Info
        ItemStack[] items = GameSingleton.Instance.PlayerInventory.CurrentItems;

        // Clean current buttons
        int currentBtnsTotal = Instance.inventoryDisplayRoot.childCount;
        for (int i = 0; i < currentBtnsTotal; i++)
            Instance.inventoryDisplayRoot.GetChild(0).parent = Instance.dispose.transform;

        // Instantiate new buttons
        for (int i = 0;i < items.Length; i++)
        {
            UIItemButton nBtn = Instantiate(Instance.itemDisplayPrefab, Instance.inventoryDisplayRoot).GetComponent<UIItemButton>();
            nBtn.SetupFromItemStack(items[i]);
        }
    }
    public static void CloseInventory()
    {
        if (Instance.inventTransiting)
        {
            Instance.inventTransitioningCall = true;
            Instance.afterInventTransitState = false;
            return;
        }
        Instance.inventoryTimer = new CountDownTimer(Instance.inOutTransitTime, () => Instance.InventoryDisplayTransitionDone(false), Instance.ChangingInventoryDisplayState);
        Instance.inventTransiting = true;
    }

    private void ChangingInventoryDisplayState(float progress) => inventoryCanvasGroup.alpha = progress;
    private void InventoryDisplayTransitionDone(bool isOn)
    {
        // In case there were any call for a transition during a transition
        if (inventTransitioningCall && afterInventTransitState != isOn)
        {
            if (isOn)
                CloseInventory();
            else
                ShowInventoryRefresh();
        }
        else
        {
            inventoryCanvasGroup.interactable = inventoryCanvasGroup.blocksRaycasts = isOn;
            inventoryCanvasGroup.alpha = isOn ? 1f : 0f;
        }
        inventTransiting = false;
        inventTransitioningCall = false;
        inventoryTimer = null; // Discart timer
    }
}
