using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lesson3_14 : MonoBehaviour
{
    int counter;

    [SerializeField] GameObject Lesson3__14, Task1, Task2;

    [Header("Task 1: FinTech Match-Up")]
    [SerializeField] List<FintechMatch> fintechMatches;

    private int correctCount = 0;
    public static bool AnimationLevel3 = false;

    [Header("Task 2: Tech Explorer")]
    [SerializeField] GameObject miniTaskPanel;
    [SerializeField] TMP_Text miniTaskTitle;
    [SerializeField] GameObject aiCreditScoringUI;
    [SerializeField] GameObject blockchainLendingUI;
    [SerializeField] GameObject digitalIDUI;
    [SerializeField] GameObject bnplCornerUI;
    [SerializeField] GameObject smartWalletUI;

    private HashSet<string> visitedFintechTools = new HashSet<string>();
    private int fintechToolsVisited = 0;
    private bool task2Completed = false;

    [Header("Mini-Task: AI Credit Scoring")]
    [SerializeField] Slider incomeSlider;
    [SerializeField] Slider expensesSlider;
    [SerializeField] Slider debtSlider;
    [SerializeField] TMP_Text creditScoreText;
    [SerializeField] TMP_Text creditScoreFeedbackText;

    private float defaultIncome, defaultExpenses = 500, defaultDebt = 500;
    private bool aiTaskStarted = false;


    [Header("Mini-Task: Blockchain Lending")]
    [SerializeField] TMP_Text blockchainLoanFeedbackText;

    private string selectedLoanAmount = "";
    private string selectedCollateral = "";


    [Header("Mini-Task: Digital ID")]
    [SerializeField] TMP_Text idVerificationFormText;
    [SerializeField] TMP_Text idVerificationResultText;

    private bool idTaskStarted = false;

    [Header("Mini-Task: BNPL Corner")]
    [SerializeField] TMP_Text bnplDescriptionText;
    [SerializeField] TMP_Text bnplResultText;

    private bool bnplTaskStarted = false;


    [Header("Mini-Task: Smart Wallet")]
    [SerializeField] Toggle roundUpToggle;
    [SerializeField] Toggle savePercentToggle;
    [SerializeField] TMP_Text smartWalletSummaryText;
    [SerializeField] Slider savingsGrowthSlider;

    private bool smartWalletTaskStarted = false;







    [Header("Task 3: Final Quiz")]
    [SerializeField] GameObject Task3;
    [SerializeField] TMP_Text questionText;
    [SerializeField] TMP_Text feedbackText;
    [SerializeField] Button[] answerButtons;
    [SerializeField] GameObject resultPanel;
    [SerializeField] TMP_Text resultText;

    private int currentQuestionIndex = 0;
    private int correctAnswers = 0;
    bool gotAnyWrong = false;
    private QuizQuestion[] quizQuestions;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        GameManagerModule3.instance.AddMoney(0);
        GameManagerModule3.instance.AddPoint(0);
        GameManagerModule3.instance.AddBankMoney(0, 0);
        GameManagerModule3.instance.LivesCheck();
    }

    public void Task3_14NextBtn()
    {
        GameManagerModule3.instance.CurrentLevelProgress(counter, 7.0f);
        if (counter == 0 && PlayerPrefs.GetInt("sofia") == 14)
        {
            GameManagerModule3.instance.NPCsNames.text = "Sofia";
            GameManagerModule3.instance.NpcImages[2].SetActive(true);
            GameManagerModule3.instance.DialogPanel.SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "Wow…this is brilliant, can you show me how this tech works?";
            counter++;
        }
        else if (counter == 1)
        {
            Lesson3__14.SetActive(true);
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            Task1.SetActive(true);
            StartTask1();
            counter++;
        }
        else if (counter == 2)
        {
            // Task 1 is finished
            GameManagerModule3.instance.Toast.Show("+10 Points+", 2);
            GameManagerModule3.instance.AddPoint(10);

            Task1.SetActive(false);
            GameManagerModule3.instance.NpcImages[2].SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "see those laptops over there, go to them to learn more about the FinTech tools. You need to learn about at least 3 of them. I will be waiting for you here.";
            counter++;
        }
        else if (counter == 3)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            GameManagerModule3.instance.DialogPanel.SetActive(false);

            //Lesson3__14.SetActive(true);
            //Task2.SetActive(true);
            counter++;
        }
        else if (counter == 4 && task2Completed)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            GameManagerModule3.instance.DialogPanel.SetActive(true);
            Lesson3__14.SetActive(true);
            Task2.SetActive(false);
            StartTask3();
            counter++;
        }
        else if (counter == 5)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "You did great, Alex; Grandma Millie was right about you; you're brilliant. I know where you have to go next. You have to go to Barcelona, to my cousin, Leo. He can help you.";
            counter++;
        }
        else if (counter == 6)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            Lesson3__14.SetActive(false);
            PlayerPrefs.SetInt("sofia", 15);
            PlayerPrefs.SetInt("gameprogress", 23);
            GameManagerModule3.instance.RewardPanell();
            GameManagerModule3.instance.AddBankMoney(100, 1);
            GameManagerModule3.instance.AddPoint(15);
            PlayerPrefs.SetInt("modules", 3);
            counter++;
        }
    }

    void StartTask1()
    {
        correctCount = 0;

        foreach (var match in fintechMatches)
        {
            var drag = match.draggableItem.GetComponent<DraggableItem>();
            drag.matchController = this;
            drag.correctDropZone = match.correctDropZone;
            drag.toolName = match.toolName;
        }
    }

    public void OnItemDropped(string toolName, GameObject dropZone)
    {
        var match = fintechMatches.Find(m => m.toolName == toolName);

        if (match != null && match.correctDropZone == dropZone)
        {
            correctCount++;
            match.draggableItem.SetActive(false);
            GameManagerModule3.instance.PopUpShow(0);
        }
        else
        {
            match.draggableItem.GetComponent<DraggableItem>().ResetToOriginal();
            GameManagerModule3.instance.PopUpShow(1);
        }


        CheckAllMatched();
    }


    void CheckAllMatched()
    {
        if (correctCount == fintechMatches.Count)
        {
            Invoke(nameof(Task3_14NextBtn), 2f);
        }
    }

    //===============Task 2=================

    public void OpenMiniTask(string fintechToolName)
    {
        if (task2Completed) return;

        Task2.SetActive(true);
        Lesson3__14.SetActive(true);
        GameManagerModule3.instance.DialogPanel.SetActive(true);
        miniTaskTitle.text = $"Exploring: {fintechToolName}";

        // Hide all mini-task UIs first
        aiCreditScoringUI.SetActive(false);
        blockchainLendingUI.SetActive(false);
        digitalIDUI.SetActive(false);
        bnplCornerUI.SetActive(false);
        smartWalletUI.SetActive(false);

        // Open the specific mini-task UI
        switch (fintechToolName)
        {
            case "AI Credit Scoring":
                aiCreditScoringUI.SetActive(true);
                defaultIncome = incomeSlider.value;
                defaultExpenses = expensesSlider.value;
                defaultDebt = debtSlider.value;
                aiTaskStarted = true;
                break;
            case "Blockchain Lending":
                blockchainLendingUI.SetActive(true);
                break;
            case "Digital ID Verification":
                digitalIDUI.SetActive(true);
                idVerificationFormText.text =
                    "Pre-Filled Form:\n\nName: Sofia Müller\nID#: 1234-5678-910\nAddress: Zürich, Bahnhofstrasse 1";
                idVerificationResultText.text = "Is this information correct?";
                idTaskStarted = true; ;
                break;
            case "BNPL Corner":
                bnplCornerUI.SetActive(true);
                bnplDescriptionText.text = "Sofia wants to buy a refrigerator. How should she pay?";
                bnplResultText.text = "Sofia wants to buy a refrigerator. How should she pay?";
                bnplTaskStarted = true;
                break;
            case "Smart Wallet Setup":
                smartWalletUI.SetActive(true);
                roundUpToggle.isOn = false;
                savePercentToggle.isOn = false;
                smartWalletSummaryText.text = "";
                savingsGrowthSlider.value = 0f;
                smartWalletTaskStarted = true;
                break;
        }
    }

    public void CompleteMiniTask(string fintechToolName)
    {
        if (!visitedFintechTools.Contains(fintechToolName))
        {
            visitedFintechTools.Add(fintechToolName);
            fintechToolsVisited++;
        }

        Task2.SetActive(false);
        Lesson3__14.SetActive(false);
        GameManagerModule3.instance.DialogPanel.SetActive(false);

        if (fintechToolsVisited >= 3 && !task2Completed)
        {
            task2Completed = true;
            GameManagerModule3.instance.Toast.Show("+20 Points\n🏅 FinTech Explorer Badge Unlocked!", 3);
            GameManagerModule3.instance.AddPoint(20);
            Task3_14NextBtn(); // Move to Task 3 automatically
        }
    }

    // Mini Task  : 1 ===============================================

    public void UpdateCreditScore()
    {
        float income = incomeSlider.value;
        float expenses = expensesSlider.value;
        float debt = debtSlider.value;

        // Simple simulation logic
        float rawScore = 650 + (income - expenses - debt) / 5f;
        float score = Mathf.Clamp(rawScore, 300, 850);

        creditScoreText.text = $"Credit Score: {(int)score}";

        string feedback;
        if (score >= 750)
            feedback = " Excellent! Your high income and low debt mean a great score.";
        else if (score >= 650)
            feedback = " Decent score — try reducing expenses or debt for improvement.";
        else
            feedback = " Your debt or expenses are hurting your score. Be careful!";

        creditScoreFeedbackText.text = feedback;
    }

    public void CompleteAICreditScoringTask()
    {
        if (!aiTaskStarted) return;

        float currentIncome = incomeSlider.value;
        float currentExpenses = expensesSlider.value;
        float currentDebt = debtSlider.value;

        if (Mathf.Approximately(currentIncome, defaultIncome) &&
            Mathf.Approximately(currentExpenses, defaultExpenses) &&
            Mathf.Approximately(currentDebt, defaultDebt))
        {
            GameManagerModule3.instance.PopUpShow(1); // No interaction
            creditScoreFeedbackText.text = "Adjust the sliders to see your score before submitting!";
            return;
        }

        // Calculate simulated score
        float score = 650 + (currentIncome - currentExpenses - currentDebt) / 5f;
        score = Mathf.Clamp(score, 300, 850);

        string feedback;
        if (score >= 750)
            feedback = " Excellent! Your high income and low debt mean a great score.";
        else if (score >= 650)
            feedback = " Decent score — try reducing expenses or debt for improvement.";
        else
            feedback = " Your debt or expenses are hurting your score. Be careful!";

        creditScoreText.text = $"Credit Score: {(int)score:0}";
        creditScoreFeedbackText.text = feedback;

        GameManagerModule3.instance.PopUpShow(0);

        Invoke(nameof(CompleteAICreditScoringAndExit), 2f);
        aiTaskStarted = false;
    }
    void CompleteAICreditScoringAndExit()
    {
        CompleteMiniTask("AI Credit Scoring");
    }

    //Blockchain mini task : 2 ===============================================

    public void SelectLoanAmount(string amount)
    {
        selectedLoanAmount = amount;
        blockchainLoanFeedbackText.text = $"Loan selected: {amount}";
    }

    public void SelectCollateral(string cryptoAmount)
    {
        selectedCollateral = cryptoAmount;
        blockchainLoanFeedbackText.text = $"Collateral selected: {cryptoAmount}";
    }

    public void ConfirmBlockchainLoan()
    {
        if (string.IsNullOrEmpty(selectedLoanAmount) || string.IsNullOrEmpty(selectedCollateral))
        {
            GameManagerModule3.instance.PopUpShow(1); // Missing selection
            blockchainLoanFeedbackText.text = " Please select both a loan amount and collateral.";
            return;
        }

        // Success
        GameManagerModule3.instance.PopUpShow(0);
        blockchainLoanFeedbackText.text =
            $" Smart Contract Created!\nLoan: {selectedLoanAmount}\nCollateral: {selectedCollateral}";

        // Delay before completion
        Invoke(nameof(CompleteBlockchainLoanTask), 2.5f);
    }

    void CompleteBlockchainLoanTask()
    {
        CompleteMiniTask("Blockchain Lending");
        selectedLoanAmount = "";
        selectedCollateral = "";
    }


    // mini task : 3 ===============================================
    public void AnswerIDVerification(bool isYesSelected)
    {
        if (!idTaskStarted)
        {
            GameManagerModule3.instance.PopUpShow(1);
            idVerificationResultText.text = "Please start the task first.";
            return;
        }

        if (isYesSelected)
        {
            idVerificationResultText.text = "KYC Verification Passed!";
            GameManagerModule3.instance.PopUpShow(0);
            Invoke(nameof(CompleteDigitalIDTask), 2.5f);
        }
        else
        {
            idVerificationResultText.text = "Verification failed. Info actually matched!";
            GameManagerModule3.instance.PopUpShow(1);
        }

        idTaskStarted = false;
    }

    void CompleteDigitalIDTask()
    {
        CompleteMiniTask("Digital ID Verification");
    }


    // mini task : 4 ===============================================
    public void ChoosePaymentMethod(string method)
    {
        if (!bnplTaskStarted)
        {
            GameManagerModule3.instance.PopUpShow(1);
            bnplResultText.text = " Please start the task first.";
            return;
        }

        if (method == "BNPL")
        {
            bnplTaskStarted = false;
            bnplResultText.text = " BNPL selected.\nSofia’s Paid off total price of the refrigrator in 3 months.";
            GameManagerModule3.instance.PopUpShow(0);
            Invoke(nameof(CompleteBNPLTask), 2.5f);
        }
        else
        {
            bnplResultText.text = $" {method} selected.\nBNPL was the correct choice for flexibility!";
            GameManagerModule3.instance.PopUpShow(1);
            bnplTaskStarted = true;
        }


    }

    void CompleteBNPLTask()
    {
        CompleteMiniTask("BNPL Corner");
    }


    // mini task : 5 ===============================================

    public void UpdateSavingsGrowthVisual()
    {
        float growth = 0f;
        if (roundUpToggle.isOn) growth += 0.5f;
        if (savePercentToggle.isOn) growth += 0.5f;

        savingsGrowthSlider.value = growth;
    }


    public void SetupSmartWallet()
    {
        List<string> selectedRules = new List<string>();

        if (roundUpToggle.isOn) selectedRules.Add("Round-up purchases");
        if (savePercentToggle.isOn) selectedRules.Add("Save 10% of each paycheck");

        UpdateSavingsGrowthVisual();

    }

    public void ConfirmSmartWalletSetup()
    {
        bool rule1 = roundUpToggle.isOn;
        bool rule2 = savePercentToggle.isOn;

        UpdateSavingsGrowthVisual(); // Optional: refresh slider for final state

        if (!rule1 || !rule2)
        {
            smartWalletSummaryText.text = " Please enable both savings rules to complete this task.";
            GameManagerModule3.instance.PopUpShow(1);
            return;
        }

        smartWalletSummaryText.text = " Smart rules set up!\nSavings automation is live.";
        GameManagerModule3.instance.PopUpShow(0);
        Invoke(nameof(CompleteSmartWalletTask), 2.5f);
    }

    void CompleteSmartWalletTask()
    {
        CompleteMiniTask("Smart Wallet Setup");
    }



    //Task 3: Final Quiz

    void StartTask3()
    {
        Task3.SetActive(true);
        resultPanel.SetActive(false);
        feedbackText.text = "";
        currentQuestionIndex = 0;
        correctAnswers = 0;

        quizQuestions = new QuizQuestion[]
        {
        new QuizQuestion {
            question = "What does BNPL stand for?",
            options = new[] { "Banking Network Payment Ledger", "Buy Now Pay Later", "Basic Net Purchase Law" },
            correctIndex = 1,
            hint = "Think about shopping with delay!"
        },
        new QuizQuestion {
            question = "Why is a high credit score useful?",
            options = new[] { "Lower interest rates", "Faster bank queues", "Free rewards" },
            correctIndex = 0,
            hint = "It’s about trust and cost!"
        },
        new QuizQuestion {
            question = "Which FinTech tool helps those with no credit history?",
            options = new[] { "AI Credit Scoring", "Loan Sharking", "Paper applications" },
            correctIndex = 0,
            hint = "Tech-based evaluation!"
        },
        new QuizQuestion {
            question = "Which loan type carries highest risk?",
            options = new[] { "Secured Loan", "Mortgage", "Payday Loan" },
            correctIndex = 2,
            hint = "Short-term, high interest..."
        },
        new QuizQuestion {
            question = "What does a loan term represent?",
            options = new[] { "Loan amount", "Duration of repayment", "Interest rate" },
            correctIndex = 1,
            hint = "It’s about how long you have to pay!"
        }
        };

        ShowNextQuestion();
    }

    void ShowNextQuestion()
    {
        if (correctAnswers == quizQuestions.Length)
        {
            gotAnyWrong = false; // Reset for next quiz
        }

        if (currentQuestionIndex >= quizQuestions.Length)
        {
            EndQuiz();
            return;
        }

        QuizQuestion q = quizQuestions[currentQuestionIndex];
        questionText.text = $"Q{currentQuestionIndex + 1}: {q.question}";
        feedbackText.text = "";

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = q.options[i];
            int capturedIndex = i; // closure fix
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(capturedIndex));
        }
    }


    void OnAnswerSelected(int selectedIndex)
    {
        QuizQuestion q = quizQuestions[currentQuestionIndex];

        if (selectedIndex == q.correctIndex)
        {
            GameManagerModule3.instance.PopUpShow(0);
            correctAnswers++;
        }
        else
        {
            gotAnyWrong = true; //  Track if anything was wrong
            GameManagerModule3.instance.PopUpShow(1);
        }

        Invoke(nameof(AdvanceQuestion), 0.4f);
    }

    void EndQuiz()
    {
        //Task3.SetActive(false);
        resultPanel.SetActive(true);

        float percent = (correctAnswers / (float)quizQuestions.Length) * 100f;

        if (percent >= 80f)
        {
            resultText.text = $" You got {percent}%! You're a FinTech Genius!\n\nCertificate unlocked!";
            Invoke(nameof(CompleteLesson3), 1.5f);
            AnimationLevel3 = true;
        }
        else
        {

            resultText.text = $" You scored {percent}%.\nYou must get 4/5 questions right to pass.\nRetrying quiz...";
            GameManagerModule3.instance.Toast.Show(" Retrying the quiz… Get them all right next time! ", 2.5f);
            Invoke(nameof(RetryTask3Quiz), 2.5f);
        }
    }

    void RetryTask3Quiz()
    {
        Task3.SetActive(true);
        resultPanel.SetActive(false);
        StartTask3(); // resets progress and starts again
    }



    void CompleteLesson3()
    {
        Task3.SetActive(false);
        resultPanel.SetActive(false);
        Task3_14NextBtn();
    }



    void AdvanceQuestion()
    {
        currentQuestionIndex++;
        ShowNextQuestion();
    }

    public void Retry()
    {
        PlayerPrefs.SetInt("sofia", 14);
        PlayerPrefs.SetInt("gameprogress", 22);
        SceneManager.LoadScene(4);
    }
}

[System.Serializable]
public class FintechMatch
{
    public string toolName;
    public GameObject draggableItem;
    public GameObject correctDropZone;
}

class QuizQuestion
{
    public string question;
    public string[] options;
    public int correctIndex;
    public string hint;
}