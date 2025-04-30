using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lesson3_13 : MonoBehaviour
{
    int counter;
    [SerializeField] GameObject Lesson3__13, Task1, Task2;

    [Header("Task 1: P2P Lending")]
    [SerializeField] Button borrowerBtn1;
    [SerializeField] Button borrowerBtn2;
    [SerializeField] Button borrowerBtn3;


    [SerializeField] GameObject borrowerResultPanel;
    [SerializeField] TMP_Text borrowerResultText;

    [SerializeField] Slider interestSlider;
    [SerializeField] TMP_Text interestText;
    [SerializeField] Button lendButton;

    private bool task1Completed = false;
    private BorrowerProfile[] borrowerOptions;
    private int selectedBorrowerIndex = -1;

    [Header("Task 2: Risk Assessment Quiz")]
    [SerializeField] TMP_Text riskProfileText;
    [SerializeField] TMP_Text riskFeedbackText;
    [SerializeField] Button lowRiskBtn;
    [SerializeField] Button medRiskBtn;
    [SerializeField] Button highRiskBtn;

    private int currentRiskIndex = 0;

    private RiskProfile[] riskProfiles;

    void Start()
    {
        counter = 0;
        GameManagerModule3.instance.AddMoney(0);
        GameManagerModule3.instance.AddPoint(0);
        GameManagerModule3.instance.AddBankMoney(0, 0);
        GameManagerModule3.instance.LivesCheck();
    }

    public void Task3_13NextBtn()
    {
        GameManagerModule3.instance.CurrentLevelProgress(counter, 7.0f);
        if (counter == 0 && PlayerPrefs.GetInt("sofia") == 13)
        {
            GameManagerModule3.instance.NPCsNames.text = "Sofia";
            GameManagerModule3.instance.NpcImages[2].SetActive(true);
            GameManagerModule3.instance.DialogPanel.SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "Alright, Alex, you explain to me how borrowing money works but how does lending work?";
            counter++;
        }
        else if (counter == 1)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);

            Lesson3__13.SetActive(true);
            Task1.SetActive(true);
            StartTask1();
            counter++;
        }
        else if (counter == 2)
        {
            Invoke(nameof(StartTask2), 1.5f);
            //StartTask2();
            counter++;
        }
        else if (counter == 3)
        {
            Lesson3__13.SetActive(false);
            PlayerPrefs.SetInt("sofia", 14);
            PlayerPrefs.SetInt("gameprogress", 22);
            GameManagerModule3.instance.RewardPanell();
            GameManagerModule3.instance.OtherRewards.text = "P2P Lending";
            GameManagerModule3.instance.AddBankMoney(100, 1);
            GameManagerModule3.instance.AddPoint(15);
            counter++;
        }
    }

    void StartTask1()
    {
        Task1.SetActive(true);
        interestSlider.gameObject.SetActive(false);
        lendButton.interactable = false;
        selectedBorrowerIndex = -1;

        borrowerOptions = new BorrowerProfile[]
        {
            new BorrowerProfile
            {
                name = "Emma",
                creditScore = "Good (750)",
                loanAmount = "$1,000",
                repaymentHistory = "No missed payments",
                purpose = "Medical expenses",
                isIdealChoice = true,
                sixMonthOutcome = " Emma repaid the loan with full interest in 6 months. Her strong credit and responsible history made her a reliable borrower."
            },
            new BorrowerProfile
            {
                name = "Leo",
                creditScore = "Fair (620)",
                loanAmount = "$2,500",
                repaymentHistory = "1 missed payment last year",
                purpose = "Vacation",
                isIdealChoice = false,
                sixMonthOutcome = " Leo was late with repayments. Lending for non-essential spending carries risk."
            },
            new BorrowerProfile
            {
                name = "Zara",
                creditScore = "Poor (580)",
                loanAmount = "$3,000",
                repaymentHistory = "Frequent delays",
                purpose = "New gaming PC",
                isIdealChoice = false,
                sixMonthOutcome = " Zara defaulted. Low credit and unnecessary purchase made her high-risk."
            }
        };

        DisplayBorrowers();

        interestSlider.onValueChanged.RemoveAllListeners();
        interestSlider.onValueChanged.AddListener(UpdateInterestText);

        lendButton.onClick.RemoveAllListeners();
        lendButton.onClick.AddListener(SimulateP2PLending);
    }

    void DisplayBorrowers()
    {
        borrowerBtn1.GetComponentInChildren<TMP_Text>().text =
            FormatBorrowerText(borrowerOptions[0]);
        borrowerBtn2.GetComponentInChildren<TMP_Text>().text =
            FormatBorrowerText(borrowerOptions[1]);
        borrowerBtn3.GetComponentInChildren<TMP_Text>().text =
            FormatBorrowerText(borrowerOptions[2]);

        borrowerBtn1.onClick.RemoveAllListeners();
        borrowerBtn2.onClick.RemoveAllListeners();
        borrowerBtn3.onClick.RemoveAllListeners();

        borrowerBtn1.onClick.AddListener(() => SelectBorrower(0));
        borrowerBtn2.onClick.AddListener(() => SelectBorrower(1));
        borrowerBtn3.onClick.AddListener(() => SelectBorrower(2));
    }

    string FormatBorrowerText(BorrowerProfile b)
    {
        return $"Name: {b.name}\nCredit: {b.creditScore}\nLoan: {b.loanAmount}\nHistory: {b.repaymentHistory}\nPurpose: {b.purpose}";
    }

    void SelectBorrower(int index)
    {
        selectedBorrowerIndex = index;
        var borrower = borrowerOptions[index];

        borrowerResultText.text = $"You selected {borrower.name}. Now set an interest rate and press 'Lend'.";
        interestSlider.gameObject.SetActive(true);
        interestSlider.value = 10;
        UpdateInterestText(10);
        lendButton.interactable = true;

        borrowerBtn1.interactable = false;
        borrowerBtn2.interactable = false;
        borrowerBtn3.interactable = false;
    }

    void UpdateInterestText(float value)
    {
        interestText.text = $"Interest Rate: {value:0}%";
    }

    void SimulateP2PLending()
    {
        if (selectedBorrowerIndex == -1) return;

        var borrower = borrowerOptions[selectedBorrowerIndex];
        float rate = interestSlider.value;

        string outcomeMessage = $"{borrower.sixMonthOutcome}\n\nYou set an interest rate of {rate:0}%.";

        if (borrower.isIdealChoice)
        {
            outcomeMessage += "\n Smart lending choice! You earned a return and helped someone in need.";
            //GameManagerModule3.instance.Toast.Show("+15 Points\n“Risk Analyst” badge unlocked!", 3);
            GameManagerModule3.instance.AddPoint(15);
        }
        else
        {
            outcomeMessage += "\n They weren’t the best match. Maybe review credit history next time!";
            //GameManagerModule3.instance.Toast.Show("No reward. Lending can be risky!", 2.5f);
        }

        borrowerResultText.text = outcomeMessage;
        lendButton.interactable = false;
        interestSlider.gameObject.SetActive(false);

        Invoke(nameof(ShowBorrowerResult), 0.5f);
    }

    void ShowBorrowerResult()
    {
        borrowerResultPanel.SetActive(true);

        Invoke(nameof(FinishTask1), 5f);
    }

    void FinishTask1()
    {
        task1Completed = true;
        Task1.SetActive(false);
        Task3_13NextBtn(); // move forward
    }

    // Task 2: Risk Assessment Quiz
    void StartTask2()
    {
        Task2.SetActive(true);
        currentRiskIndex = 0;

        riskProfiles = new RiskProfile[]
{
    new RiskProfile
    {
        profileText =
        " Borrower A\n" +
        " Credit Score: 780 (Excellent)\n" +
        " Loan Amount: $800\n" +
        " Repayment History: No missed payments\n" +
        " Purpose: Dental work (Medical)",
        correctRiskLevel = "Low",
        rationale = "Excellent credit and essential medical use show responsible borrowing."
    },
    new RiskProfile
    {
        profileText =
        " Borrower B\n" +
        " Credit Score: 640 (Fair)\n" +
        " Loan Amount: $2,000\n" +
        " Repayment History: 2 late payments last year\n" +
        " Purpose: Vacation trip",
        correctRiskLevel = "High",
        rationale = "Leisure spending with past late payments is a risky combo."
    },
    new RiskProfile
    {
        profileText =
        " Borrower C\n" +
        " Credit Score: 690 (Average)\n" +
        " Loan Amount: $1,200\n" +
        " Repayment History: 1 delayed payment\n" +
        " Purpose: Car repair",
        correctRiskLevel = "Medium",
        rationale = "Mostly reliable, but some risk due to repayment hiccup."
    },
    new RiskProfile
    {
        profileText =
        " Borrower D\n" +
        " Credit Score: 580 (Poor)\n" +
        " Loan Amount: $1,500\n" +
        " Repayment History: History of defaults\n" +
        " Purpose: Gaming PC",
        correctRiskLevel = "High",
        rationale = "Low credit + unnecessary expense = high risk."
    },
    new RiskProfile
    {
        profileText =
        " Borrower E\n" +
        " Credit Score: 710 (Good)\n" +
        " Loan Amount: $1,000\n" +
        " Repayment History: No late payments\n" +
        " Purpose: School tuition",
        correctRiskLevel = "Low",
        rationale = "Good credit and an education-related loan make this low risk."
    }
};


        ShowCurrentRiskProfile();

        lowRiskBtn.onClick.RemoveAllListeners();
        medRiskBtn.onClick.RemoveAllListeners();
        highRiskBtn.onClick.RemoveAllListeners();

        lowRiskBtn.onClick.AddListener(() => EvaluateRiskAnswer("Low"));
        medRiskBtn.onClick.AddListener(() => EvaluateRiskAnswer("Medium"));
        highRiskBtn.onClick.AddListener(() => EvaluateRiskAnswer("High"));
    }

    void ShowCurrentRiskProfile()
    {
        var profile = riskProfiles[currentRiskIndex];
        riskProfileText.text = profile.profileText;
        riskFeedbackText.text = "";
    }

    void EvaluateRiskAnswer(string chosenRisk)
    {
        var profile = riskProfiles[currentRiskIndex];

        if (chosenRisk == profile.correctRiskLevel)
        {
            GameManagerModule3.instance.PopUpShow(0);
            //riskFeedbackText.text = $" Correct! {profile.rationale}";
        }
        else
        {
            GameManagerModule3.instance.PopUpShow(1);
            //riskFeedbackText.text = $"❌ System labels this as {profile.correctRiskLevel} risk.\n{profile.rationale}";
        }

        currentRiskIndex++;

        if (currentRiskIndex < riskProfiles.Length)
        {
            Invoke(nameof(ShowCurrentRiskProfile), 0.5f);
        }
        else
        {
            Invoke(nameof(FinishTask2), 3f);
        }
    }

    void FinishTask2()
    {
        Task2.SetActive(false);
        Task3_13NextBtn();
    }

    public void Retry()
    {
        PlayerPrefs.SetInt("sofia", 13);
        PlayerPrefs.SetInt("gameprogress", 21);
        SceneManager.LoadScene(4);
    }
}

class BorrowerProfile
{
    public string name;
    public string creditScore;
    public string loanAmount;
    public string repaymentHistory;
    public string purpose;
    public bool isIdealChoice;
    public string sixMonthOutcome;
}

class RiskProfile
{
    public string profileText;
    public string correctRiskLevel;
    public string rationale;
}
