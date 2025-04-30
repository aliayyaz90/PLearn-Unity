using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum SavingsAccountType { RegularSavings, CertificateOfDeposit, MoneyMarket }
public class Lesson4_2_Controller : MonoBehaviour
{
    [Header("Overall Lesson Setup")]
    [SerializeField] private GameObject lessonMainParentUI;
    [SerializeField] private GameObject task1UI;
    [SerializeField] private GameObject task2UI; // Placeholder
    [SerializeField] private GameObject task3UI; // Placeholder

    [Header("Task 1: Savings Account Selection")]
    [SerializeField] private Button[] accountButtons; // 0: Regular, 1: CD, 2: Money Market
    [SerializeField] private GameObject buttonPanelUI;
    [SerializeField] private GameObject prosConsPanel;
    [SerializeField] private TMP_Text prosConsText;
    [SerializeField] private Button task1NextButton;

    [Header("Task 1: Drag-and-Drop Validation")]
    [SerializeField] private GameObject dragDropUI;
    [SerializeField] private DropZone[] dropZones;
    [SerializeField] private DraggablePro[] draggablePros;
    [SerializeField] private int requiredCorrectDrops = 3;

    // Add these new variables to the header
    [Header("Task 2: Compare and Choose")]
    [SerializeField] private Button[] accountChoiceButtons;
    [SerializeField] private Button task2NextButton;
    [SerializeField] private GameObject feedbackPanel;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private int task2PointsReward = 10;
    [SerializeField] private int moneyReward = 50;

    // Add to state tracking
    private bool task2Completed = false;
    private bool correctChoiceMade = false;
    private SavingsAccountType chosenAccount;

    [Header("Task 3: Savings Stack Simulation")]
    [SerializeField] private Slider savingsSlider;
    [SerializeField] private Slider cdSlider;
    [SerializeField] private Slider moneyMarketSlider;

    [SerializeField] private TMP_Text savingsText;
    [SerializeField] private TMP_Text cdText;
    [SerializeField] private TMP_Text moneyMarketText;

    [SerializeField] private TMP_Text liquidityText;
    [SerializeField] private TMP_Text profitabilityText;
    [SerializeField] private TMP_Text summaryText;

    [SerializeField] private Button submitButton;

    private float totalAllocation = 5000f;

     private float[] interestRates = { 0.01f, 0.04f, 0.025f }; // Savings, CD, Money Market

    private bool task3Completed = false;
    [Header("Task 3: Savings Stack Simulation")]
    [SerializeField][Range(0, 1)] private float minLiquidRatio = 0.2f; // 20% in liquid accounts
    [SerializeField][Range(0, 1)] private float minGrowthRatio = 0.3f; // 30% in CD
    [SerializeField] private float requiredTotalReturn = 150f; // Minimum €150 annual return

    // State tracking
    private int mainCounter = 0;
    private bool task1Completed = false;
    private SavingsAccountType selectedAccount;
    private int correctDropsCount = 0;

    private bool isProgress = false;

    void Start()
    {
        InitializeAllTasks();
        //lessonMainParentUI.SetActive(true);


    }

    void InitializeAllTasks()
    {
        task1UI.SetActive(false);
        task2UI.SetActive(false);
        task3UI.SetActive(false);
        prosConsPanel.SetActive(false);
        buttonPanelUI.SetActive(false);
        dragDropUI.SetActive(false);


        // Task 1 button setup
        for (int i = 0; i < accountButtons.Length; i++)
        {
            int index = i;
            accountButtons[i].onClick.AddListener(() => OnAccountSelected(index, accountButtons[index]));
        }
        task1NextButton.onClick.AddListener(OnTask1NextClicked);
    }

    void Progress()
    {
        int leoProgress = PlayerPrefs.GetInt("leo", 3); // default to 7 if not set

        switch (leoProgress)
        {
            case 3:
                mainCounter = 0;
                isProgress = true;
                // Start from Task 1
                break;

            case 4:
                mainCounter = 4;
                isProgress = true;
                // Start from Task 2
                break;

            case 5:
                mainCounter = 6;
                isProgress = true;
                // Start from Task 3
                break;

            default:
                Debug.LogWarning("Unexpected leo progress value: " + leoProgress);
                mainCounter = 0;
                isProgress = true;
                // fallback
                break;
        }
    }
    // ======== CORE FLOW CONTROL ========
    public void TaskFlow_NextBtn()
    {
        if (!isProgress)
        {
            //Progress();
            isProgress = true;
        }
        Debug.Log($"lesson4_2_Controller Advancing from counter: {mainCounter}");
        GameManagerModule4.instance.CurrentLevelProgress(mainCounter, 8.0f);

        switch (mainCounter)
        {
            // Start lesson
            case 0:
                GameManagerModule4.instance.NPCsNames.text = "Leo";
                GameManagerModule4.instance.DialogueText.text = "Alright Alex, I see that you're ready to learn something more.";
                GameManagerModule4.instance.DialogPanel.SetActive(true);
                GameManagerModule4.instance.DialogueBox.SetActive(true);
                lessonMainParentUI.SetActive(true);

                mainCounter++;
                break;

            case 1:
                GameManagerModule4.instance.DialogueText.text = " So, let's do this. Let me show you the difference kinds of Saving methods I have learned.";
                mainCounter++;
                break;
            // Task 1: Account selection phase
            case 2:
                GameManagerModule4.instance.DialogueBox.SetActive(false);

                lessonMainParentUI.SetActive(true);
                task1UI.SetActive(true);
                buttonPanelUI.SetActive(true);
                mainCounter++;
                break;

            // Task 1: Drag-and-drop phase
            case 3:
                if (!task1Completed)
                {
                    GameManagerModule4.instance.Toast.Show("Complete Task 1 first!", 2f);
                    return;
                }
                // Proceed to Task 2 (placeholder)
                mainCounter++;
                break;

            // Task 2 Introduction
            case 4:
                GameManagerModule4.instance.NPCsNames.text = "Leo";
                GameManagerModule4.instance.DialogueText.text = "So, Alex, you've learned so much about Savings, right? Well I need some very expensive new equipment, and I only have 6 months to save up. How about you tell me how I should save?";
                GameManagerModule4.instance.DialogPanel.SetActive(true);
                InitializeTask2();
                mainCounter++;
                break;

            // Task 2 Activity
            case 5:
                GameManagerModule4.instance.DialogueBox.SetActive(false);
                // Let the choice handling manage completion
                break;

            case 6:
                GameManagerModule4.instance.NPCsNames.text = "Leo";
                GameManagerModule4.instance.DialogueText.text = "Alright Alex, this may not be real money, but treat it like real! Use all saving tools to grow it. ";
                GameManagerModule4.instance.DialogPanel.SetActive(true);
                GameManagerModule4.instance.DialogueBox.SetActive(true);

                mainCounter++;
                break;

            case 7:
                GameManagerModule4.instance.DialogueBox.SetActive(false);
                lessonMainParentUI.SetActive(true);
                InitializeTask3();
                break;

            // Task 3 Completion
            case 8:
                if (!task3Completed)
                {
                    GameManagerModule4.instance.Toast.Show("Complete the allocation first!", 2f);
                    return;
                }
                // Lesson completion logic
                mainCounter++;
                break;

                // Add Task 3 cases as needed...

                // Add cases for Tasks 2/3...
        }
    }

    // ======== TASK 1 LOGIC ========
    void OnAccountSelected(int accountIndex, Button button)
    {
        foreach (var btn in accountButtons)
        {
            btn.GetComponent<Image>().color = Color.white; // Reset color
        }
        task1NextButton.interactable = true;


        selectedAccount = (SavingsAccountType)accountIndex;
        prosConsPanel.SetActive(true);

        switch (selectedAccount)
        {
            case SavingsAccountType.RegularSavings:
                prosConsText.text = "Pros: Easy access\nCons: Low interest";
                button.GetComponent<Image>().color = Color.green; // Highlight selected button
                task1NextButton.interactable = true;
                break;
            case SavingsAccountType.CertificateOfDeposit:
                prosConsText.text = "Pros: High interest\nCons: Withdrawal penalty";
                button.GetComponent<Image>().color = Color.red;
                break;
            case SavingsAccountType.MoneyMarket:
                prosConsText.text = "Pros: Moderate interest\nCons: Minimum balance";
                button.GetComponent<Image>().color = Color.red;
                break;
        }
    }

    void OnTask1NextClicked()
    {
        if (selectedAccount != SavingsAccountType.RegularSavings)
        {
            GameManagerModule4.instance.PopUpShow(1);
            return;
        }

        // Transition to drag-and-drop validation
        buttonPanelUI.SetActive(false);
        dragDropUI.SetActive(true);
        InitializeDragDropPhase();
        mainCounter = 3; // Prepare for next phase
    }

    void InitializeDragDropPhase()
    {
        correctDropsCount = 0;
        foreach (var pro in draggablePros)
        {
            pro.gameObject.SetActive(true);
            if (pro.name == "Pro1")
                pro.Initialize(GetProText(pro), SavingsAccountType.MoneyMarket);
            else if (pro.name == "Pro2")
                pro.Initialize(GetProText(pro), SavingsAccountType.CertificateOfDeposit);
            else if (pro.name == "Pro3")
                pro.Initialize(GetProText(pro), SavingsAccountType.RegularSavings);
        }
    }

    public void HandleProDrop(DraggablePro pro, DropZone zone)
    {
        if (zone.zoneType == pro.assignedAccount)
        {
            correctDropsCount++;
            //pro.gameObject.SetActive(false);

            if (correctDropsCount >= requiredCorrectDrops)
            {
                CompleteTask1();
            }
        }
        else
        {
            pro.ResetPosition();
            GameManagerModule4.instance.PopUpShow(1);
        }
    }

    void CompleteTask1()
    {
        // Show feedback

        task1Completed = true;
        GameManagerModule4.instance.AddPoint(15);
        PlayerPrefs.SetInt("leo", 4);
        //GameManagerModule4.instance.UnlockGlossaryTerm("SavingsInstruments");
        dragDropUI.SetActive(false);
        task1UI.SetActive(false);
        Debug.Log("Task 1 Complete!");

        GameManagerModule4.instance.Toast?.Show($"Task 1 Complete! + 15 Points, + new Saving Account", 1.5f);

        // Advance to Task 2
        mainCounter = 4;
        Invoke("TaskFlow_NextBtn", 1.5f);
    }

    // ======== HELPER METHODS ========

    string GetProText(DraggablePro pro)
    {
        // Add your actual pros list here
        Debug.Log($"Pro text for {pro.name}");
        return pro.name switch
        {
            "Pro1" => "Moderate interest",
            "Pro2" => "High interest",
            "Pro3" => "Easy access",
            _ => ""
        };
    }

    // ======== TASK 2 LOGIC ========
    void InitializeTask2()
    {
        lessonMainParentUI.SetActive(true);
        task2UI.SetActive(true);
        feedbackPanel.SetActive(false);
        task2NextButton.interactable = false;

        // Reset button colors
        foreach (var btn in accountChoiceButtons)
        {
            btn.GetComponent<Image>().color = Color.white;
        }

        // Set up buttons
        for (int i = 0; i < accountChoiceButtons.Length; i++)
        {
            int index = i;
            accountChoiceButtons[i].onClick.AddListener(() =>
            {
                OnAccountChosen((SavingsAccountType)index, accountChoiceButtons[index]);
            });
        }
        task2NextButton.onClick.AddListener(OnTask2NextClicked);
    }

    void OnAccountChosen(SavingsAccountType account, Button button)
    {
        // Highlight selected button
        foreach (var btn in accountChoiceButtons)
        {
            btn.GetComponent<Image>().color = Color.white;
        }
        button.GetComponent<Image>().color = Color.green;

        chosenAccount = account;
        feedbackPanel.SetActive(true);
        task2NextButton.interactable = true;

        // Store correct choice state
        correctChoiceMade = (account == SavingsAccountType.RegularSavings);

        // Update feedback
        feedbackText.text = account switch
        {
            SavingsAccountType.RegularSavings => "Selected: Regular Savings\n(Good for short-term goals)",
            SavingsAccountType.CertificateOfDeposit => "Selected: Certificate of Deposit\n(Warning: Early withdrawal penalties)",
            SavingsAccountType.MoneyMarket => "Selected: Money Market\n(Note: Minimum balance requirements)",
            _ => ""
        };
    }

    void OnTask2NextClicked()
    {
        if (!correctChoiceMade)
        {
            GameManagerModule4.instance.PopUpShow(1);
            feedbackText.text += "\n\nTry again! Regular Savings is best for short-term needs.";
            return;
        }

        HandleTask2Completion();
    }

    void HandleTask2Completion()
    {
        if (task2Completed) return;

        task2Completed = true;
        GameManagerModule4.instance.AddPoint(task2PointsReward);
        GameManagerModule4.instance.AddMoney(moneyReward);
        PlayerPrefs.SetInt("leo", 5);

        GameManagerModule4.instance.Toast?.Show($"Task 2 Complete! +{task2PointsReward} Points, +{moneyReward}€ Banked", 1.5f);

        //feedbackText.text += $"\n\nCorrect! +{task2PointsReward} Points\n+{moneyReward}€ Added!";
        CloseTask2();
    }

    void CloseTask2()
    {
        task2UI.SetActive(false);
        feedbackPanel.SetActive(false);
        // Proceed to Task 3
        mainCounter = 6;
        Invoke("TaskFlow_NextBtn", 1.5f);

    }

    // ======== TASK 3 LOGIC ========
    void InitializeTask3()
    {
        task3UI.SetActive(true);
        summaryText.text = "";

        // Set slider limits
        savingsSlider.minValue = 0;
        cdSlider.minValue = 0;
        moneyMarketSlider.minValue = 0;

        savingsSlider.maxValue = totalAllocation;
        cdSlider.maxValue = totalAllocation;
        moneyMarketSlider.maxValue = totalAllocation;

        // Add listeners
        savingsSlider.onValueChanged.AddListener(delegate { OnAllocationChanged(); });
        cdSlider.onValueChanged.AddListener(delegate { OnAllocationChanged(); });
        moneyMarketSlider.onValueChanged.AddListener(delegate { OnAllocationChanged(); });

        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(OnSubmitAllocation);

        ResetSlidersEvenly();
    }

    void ResetSlidersEvenly()
    {
        savingsSlider.SetValueWithoutNotify(1666);
        cdSlider.SetValueWithoutNotify(1666);
        moneyMarketSlider.SetValueWithoutNotify(1668);
        OnAllocationChanged();
    }

    void OnAllocationChanged()
    {
        // Snap to 100s
        float reg = Mathf.Round(savingsSlider.value / 100f) * 100f;
        float cd = Mathf.Round(cdSlider.value / 100f) * 100f;
        float mm = Mathf.Round(moneyMarketSlider.value / 100f) * 100f;

        // Adjust third slider to maintain total
        float total = reg + cd + mm;
        float diff = totalAllocation - total;

        if (Mathf.Abs(diff) >= 1f)
        {
            // Adjust the last slider (money market) to make total 5000
            mm += diff;
            mm = Mathf.Clamp(mm, 0, totalAllocation);
            moneyMarketSlider.SetValueWithoutNotify(mm);
        }

        // Update labels
        savingsText.text = $"Regular Savings: €{reg}";
        cdText.text = $"Certificate of Deposit: €{cd}";
        moneyMarketText.text = $"Money Market: €{mm}";

        // Calculate liquidity score (CD is not liquid)
        float liquidAmount = reg + mm;
        float liquidityScore = liquidAmount / totalAllocation;

        // Calculate profitability score
        float totalReturn = (reg * interestRates[0]) + (cd * interestRates[1]) + (mm * interestRates[2]);
        float profitabilityScore = totalReturn / 200f; // Normalize, assuming ~€200 max return

        // Update meter labels
        liquidityText.text = $"Liquidity: {(liquidAmount):€#,##0}";
        profitabilityText.text = $"Profit / Year: {(totalReturn):€#,##0}";

        // Real-time feedback summary

        string feedback = $"You’ve kept €{liquidAmount:0} accessible and might earn around €{totalReturn:0} this year.";

        if (cd >= 4000)
        {
            feedback += "\n\n Most of your money is locked away! What if you need it quickly?";
        }
        else if (reg >= 4000)
        {
            feedback += "\n\n You’re being very safe — which is okay! But you could grow your money more.";
        }
        else if (liquidAmount >= 2000 && cd >= 1500 && totalReturn >= 150)
        {
            feedback += "\n\n Great job! You balanced safety and smart saving!";
        }
        else
        {
            // ✨ Friendly hint to guide them
            feedback += "\n\n Hint: Try putting at least €2,000 in easy-to-access spots, and €1,500 in the CD to grow!";
        }

        summaryText.text = feedback;


    }


    void OnSubmitAllocation()
    {
        float reg = savingsSlider.value;
        float cd = cdSlider.value;
        float mm = moneyMarketSlider.value;
        float totalReturn = (reg * interestRates[0]) + (cd * interestRates[1]) + (mm * interestRates[2]);
        float liquid = reg + mm;

        bool hasEnoughLiquidity = (reg + mm) >= 1500;
        bool hasEnoughGrowth = cd >= 1500;
        bool hasEnoughReturn = totalReturn >= 150;

        if (hasEnoughLiquidity && hasEnoughGrowth && hasEnoughReturn)
        {
            // ✅ WIN — give full reward
            //GameManagerModule4.instance.AddPoint(20);
            //GameManagerModule4.instance.UnlockBadge("Savings Architect");
            //GameManagerModule4.instance.UnlockGlossaryTerm("Liquidity vs. Profitability");
            //GameManagerModule4.instance.Toast.Show("🎉 Well done! You balanced access and growth perfectly! +20 points", 4f);
            // Disable sliders
            savingsSlider.interactable = false;
            cdSlider.interactable = false;
            moneyMarketSlider.interactable = false;
            submitButton.interactable = false;

            HandleTask3Completion();
        }
        else
        {
            GameManagerModule4.instance.Toast.Show("You can still do better! Try balancing access and growth.", 2.5f);
            GameManagerModule4.instance.PopUpShow(1);
        }
        // Disable sliders
        //savingsSlider.interactable = false;
        //cdSlider.interactable = false;
        //moneyMarketSlider.interactable = false;
        //submitButton.interactable = false;
    }

    void HandleTask3Completion()
    {
        task3Completed = true;
        GameManagerModule4.instance.AddPoint(20);
        GameManagerModule4.instance.AddMoney(moneyReward);
        //GameManagerModule4.instance.UnlockBadge(badgeRewardID);

        GameManagerModule4.instance.RewardPanell();

        PlayerPrefs.SetInt("gameprogress", 25);
        PlayerPrefs.SetInt("leo", 6);

        GameManagerModule4.instance.rewardPanelNextButton.onClick.RemoveAllListeners();
        GameManagerModule4.instance.rewardPanelNextButton.onClick.AddListener(() =>
        {
            Debug.Log("Module 4 Finished. Proceeding via GameManager.");
            this.enabled = false;
            GameManagerModule4.instance.NextRewardPanelBtn();
        });

        task3UI.SetActive(false);
        Debug.Log("Task 3 Completed!");

        // Final lesson completion logic
        //GameManagerModule4.instance.Toast.Show($"Savings Architect! +{task3PointsReward} Points +{moneyReward}€", 3f);
    }

    public void Retry()
    {
        PlayerPrefs.SetInt("leo", 0);
        PlayerPrefs.SetInt("gameprogress", 24);
        SceneManager.LoadScene(5);
    }
}