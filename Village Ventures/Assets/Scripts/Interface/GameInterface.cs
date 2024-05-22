using TMPro;
using UnityEngine;
using VillageVentures;

public class GameInterface : MonoBehaviour
{
    public static GameInterface Instance;

    [Header("Transition")]
    [SerializeField] float inOutTransitTime = 0.25f;
    [SerializeField] CanvasGroup group;

    [Header("Info Displays")]
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI playerMoney;
    [Space]
    [SerializeField] MarketInterfaceUpdater marketInterface;

    [Header("UI Message")]
    [SerializeField] float msnTime = 2.0f;
    [SerializeField] Message msnPref;
    [SerializeField] Transform msnParent;

    private bool transitioning = false;
    private bool transitioningCall = false;
    private bool afterTransitState = false;
    private CountDownTimer timer;


    public string PlayerName { set { playerName.text = value; } }
    public int PlayerMoney
    {
        set
        {
            string moneyStr = value.ToString();
            playerMoney.text = "$ " + moneyStr;
        }
    }


    private void Awake() => Instance = this;

    public void SetupAvailableItemsToSell(OutfitList list) => marketInterface.SetupFromItemsList(list);

    private void Start()
    {
        group.interactable = group.blocksRaycasts = false;
        group.alpha = 0f;
    }
    
    private void Update() => timer?.Update(Time.deltaTime);


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

    public static void UIMessage(string message, MessageMode mode = MessageMode.Normal)
    {
        if (Instance)
        {
            Message m = Instantiate(Instance.msnPref, Instance.msnParent).GetComponent<Message>();
            m.SetupMessage(message, mode);
            Destroy(m, Instance.msnTime);
        }
    }
}
