using TMPro;
using UnityEngine;
using VillageVentures;

[RequireComponent(typeof(CanvasGroup))]
public class GameInterface : MonoBehaviour
{
    public static GameInterface Instance;

    [Header("Transition")]
    [SerializeField] float inOutTransitTime = 0.25f;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI playerMoney;

    private bool transitioning = false;
    private bool transitioningCall = false;
    private bool afterTransitState = false;
    private CanvasGroup group;
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

    private void Start()
    {
        group = GetComponent<CanvasGroup>();
        group.interactable = group.blocksRaycasts = false;
        group.alpha = 0f;
    }
    private void Update() => timer?.Update(Time.deltaTime);


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
}
