using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lesson3_12 : MonoBehaviour
{
    public int counter;
    [SerializeField] GameObject Lesson3__12, Task1, Task2;

    [Header("Task 1: BNPL Quiz")]
    [SerializeField] TMP_Text scenarioText;
    [SerializeField] TMP_Text feedbackText;
    [SerializeField] Button goodOptionBtn;
    [SerializeField] Button riskyOptionBtn;
    private int currentScenarioIndex = 0;
    private struct BNPLScenario
    {
        public string situation;
        public bool isBNPLGood; 
        public string feedback;
    }
    private BNPLScenario[] scenarios = new BNPLScenario[]
{
    new BNPLScenario {
        situation = "Sofia needs to replace her broken fridge quickly but doesn’t have the full amount right now.",
        isBNPLGood = true,
        feedback = " This is a good time to use BNPL — it's an emergency need!"
    },
    new BNPLScenario {
        situation = "Sofia wants to buy a new gaming console, but she’s already struggling to pay bills.",
        isBNPLGood = false,
        feedback = " Using BNPL with low income could lead to debt — better to wait!"
    },
    new BNPLScenario {
        situation = "Sofia doesn’t have a credit card, and BNPL is the only way to shop online.",
        isBNPLGood = true,
        feedback = " BNPL can be helpful if used responsibly, especially without credit cards."
    }
};

    [Header("Task 2: Choose the Best BNPL Offer")]
    [SerializeField] GameObject task2TimerPanel;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text offerFeedbackText;

    [SerializeField] Button offerBtn1;
    [SerializeField] Button offerBtn2;
    [SerializeField] Button offerBtn3;

    private float decisionTime = 30f;
    private bool hasSelectedOffer = false;
    private Coroutine offerTimerCoroutine;

    private struct BNPL_OfferOption
    {
        public string summary;
        public bool isFair;
        public string feedback;
    }

    private BNPL_OfferOption[] offerOptions;
    private BNPL_OfferOption[] penaltyOffers;



    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        GameManagerModule3.instance.AddMoney(0);
        GameManagerModule3.instance.AddPoint(0);
        GameManagerModule3.instance.AddBankMoney(0, 0);
        GameManagerModule3.instance.LivesCheck();
    }
    public void Task3_12NextBtn() 
    {
        // Debug.Log(counter);
        GameManagerModule3.instance.CurrentLevelProgress(counter, 7.0f);

        if (counter == 0 && PlayerPrefs.GetInt("sofia") == 12)
        {
            GameManagerModule3.instance.NPCsNames.text = "Sofia";
            GameManagerModule3.instance.NpcImages[2].SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "Sofia: Well the advertisement said I can buy the fridge now and give them the money later, but I don't know how that would work... Can you explain, Alex?";
            GameManagerModule3.instance.DialogPanel.SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            counter++;
        }
        else if (counter == 1)
        {

            Lesson3__12.SetActive(true);
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            Task1.SetActive(true);
            StartBNPLQuiz();
            counter++;
        }

        else if (counter == 2)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "Alright so you established that not all by no preliator offers at the same but I have three so please help me pick.";
            counter++;
        }
        else if (counter == 3)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            Task2.SetActive(true);
            StartTask2();
            counter++;
        }

        else if (counter == 4)
        {
            Lesson3__12.SetActive(false);
            PlayerPrefs.SetInt("sofia", 13);
            PlayerPrefs.SetInt("gameprogress", 21);
            GameManagerModule3.instance.RewardPanell();
            GameManagerModule3.instance.AddMoney(100);
            GameManagerModule3.instance.AddPoint(10);
            counter++;
        }
    }

    void StartBNPLQuiz()
    {
        currentScenarioIndex = 0;
        ShowCurrentScenario();

        goodOptionBtn.onClick.RemoveAllListeners();
        riskyOptionBtn.onClick.RemoveAllListeners();
        ;

        goodOptionBtn.onClick.AddListener(() => EvaluateAnswer(true));
        riskyOptionBtn.onClick.AddListener(() => EvaluateAnswer(false));
    }

    void ShowCurrentScenario()
    {
        var sc = scenarios[currentScenarioIndex];
        scenarioText.text = sc.situation;
        feedbackText.text = ""; 
        goodOptionBtn.interactable = true;
        riskyOptionBtn.interactable = true;
    }

    void EvaluateAnswer(bool playerThinksBNPLIsGood)
    {
        var sc = scenarios[currentScenarioIndex];
        if (playerThinksBNPLIsGood == sc.isBNPLGood)
        {
            GameManagerModule3.instance.PopUpShow(0);
            feedbackText.text = " Correct!\n" + sc.feedback;

            currentScenarioIndex++;
            if (currentScenarioIndex >= scenarios.Length)
            {
                Task1Complete();
                return;
            }

            ShowCurrentScenario();
        }
        else
        {
            GameManagerModule3.instance.PopUpShow(1);
            feedbackText.text = " Not quite!\n" + sc.feedback;
        }

        //goodOptionBtn.interactable = false;
        //riskyOptionBtn.interactable = false;
    }

    void Task1Complete()
    {
        GameManagerModule3.instance.Toast.Show("+10 Points\n+150 Money", 1.5f);
        GameManagerModule3.instance.AddPoint(10);
        GameManagerModule3.instance.AddMoney(150);
        Task1.SetActive(false);

        Debug.Log("Task 1 Complete!");
        Invoke(nameof(Task3_12NextBtn), 1.5f);
    }

    void StartTask2()
    {
        hasSelectedOffer = false;
        task2TimerPanel.SetActive(true);
        offerFeedbackText.text = "";

        
        offerOptions = new BNPL_OfferOption[]
        {
        new BNPL_OfferOption
        {
            summary = "0% interest if paid in 4 installments over 8 weeks. No late fees. Return within 30 days.",
            isFair = true,
            feedback = " This is a good offer! No fees and a fair return window."
        },
        new BNPL_OfferOption
        {
            summary = "10% interest + €30 late fee if missed. No returns.",
            isFair = false,
            feedback = " Ouch! That’s a trap — high fees and no return policy."
        },
        new BNPL_OfferOption
        {
            summary = "Pay in 6 months, but 25% interest added after 2 months. Returns only in first 5 days.",
            isFair = false,
            feedback = " This sounds flexible, but interest kicks in fast and return window is tiny."
        }
        };

        penaltyOffers = new BNPL_OfferOption[]
{
    new BNPL_OfferOption {
        summary = "Now includes €20 late fee. Return within 7 days only.",
        isFair = true,
        feedback = " Missed your chance! Now you’re stuck with less friendly terms."
    },
    new BNPL_OfferOption {
        summary = "Now includes 15% interest and no returns allowed.",
        isFair = false,
        feedback = " Missing the deadline means no return policy and costly interest."
    },
    new BNPL_OfferOption {
        summary = "Now delayed interest starts after just 1 month. No fee cap.",
        isFair = false,
        feedback = " The delay cost you — interest hits sooner, no protection from fees."
    }
};


        //offer text on buttons
        offerBtn1.GetComponentInChildren<TMP_Text>().text = offerOptions[0].summary;
        offerBtn2.GetComponentInChildren<TMP_Text>().text = offerOptions[1].summary;
        offerBtn3.GetComponentInChildren<TMP_Text>().text = offerOptions[2].summary;

       
        offerBtn1.onClick.RemoveAllListeners();
        offerBtn2.onClick.RemoveAllListeners();
        offerBtn3.onClick.RemoveAllListeners();

        offerBtn1.onClick.AddListener(() => OnOfferSelected(0));
        offerBtn2.onClick.AddListener(() => OnOfferSelected(1));
        offerBtn3.onClick.AddListener(() => OnOfferSelected(2));

        offerTimerCoroutine = StartCoroutine(OfferTimerCountdown());
    }

    IEnumerator OfferTimerCountdown()
    {
        float timeLeft = decisionTime;
        while (timeLeft > 0 && !hasSelectedOffer)
        {
            timerText.text = $" Time Left: {Mathf.Ceil(timeLeft)}s";
            yield return new WaitForSeconds(1f);
            timeLeft -= 1f;
        }

        if (!hasSelectedOffer)
        {
            //  penalty offers
            offerOptions = penaltyOffers;

            offerBtn1.GetComponentInChildren<TMP_Text>().text = penaltyOffers[0].summary;
            offerBtn2.GetComponentInChildren<TMP_Text>().text = penaltyOffers[1].summary;
            offerBtn3.GetComponentInChildren<TMP_Text>().text = penaltyOffers[2].summary;

            offerFeedbackText.text = " Time's up! Now the offers are worse... choose carefully!";
            //GameManagerModule3.instance.PopUpShow(1);

            
        }
    }

    void OnOfferSelected(int index)
    {
        if (hasSelectedOffer) return;

        hasSelectedOffer = true;
        StopCoroutine(offerTimerCoroutine);

        var selected = offerOptions[index];
        offerFeedbackText.text = selected.feedback;

        bool isPenaltyPhase = selected.isFair == false && offerOptions[0].feedback.Contains("Missed");

        if (selected.isFair && !isPenaltyPhase)
        {
            offerBtn1.interactable = false;
            offerBtn2.interactable = false;
            offerBtn3.interactable = false;
            GameManagerModule3.instance.PopUpShow(0);

            Invoke(nameof(Task2Complete), 1f);
        }
        else
        {
            hasSelectedOffer = false;
            GameManagerModule3.instance.Toast.Show(" That offer had hidden fees! No reward.", 1.5f);
            GameManagerModule3.instance.PopUpShow(1);
        }
    }
    void Task2Complete()
    {
        Task2.SetActive(false);
        Invoke(nameof(Task3_12NextBtn), 1f);
    }
    public void Retry()
    {
        PlayerPrefs.SetInt("sofia", 12);
        PlayerPrefs.SetInt("gameprogress", 20);
        SceneManager.LoadScene(4);
    }
}
