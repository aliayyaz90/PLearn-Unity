using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lesson4_5_Controller : MonoBehaviour
{
    [Header("Overall Lesson Setup")]
    [SerializeField] private GameObject lessonMainParentUI;
    [SerializeField] private GameObject task1UI;
    [SerializeField] private GameObject task2UI;
    [SerializeField] private GameObject task3UI;

    [Header("Task 1: Set It and Forget It")]
    [SerializeField] private TMP_Dropdown frequencyDropdown;
    [SerializeField] private TMP_InputField amountInput;
    [SerializeField] private TMP_Dropdown targetAccountDropdown;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button appButton;
    [SerializeField] private TMP_Text confirmationMessage;

    [SerializeField] private int task1Points = 10;
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private GameObject appPanel;

    [Header("Task 2: Gamify Your Savings")]
    [SerializeField] private TMP_Text totalBudgetText;
    [SerializeField] private TMP_Text totalSavedText;
    [SerializeField] private Button nextTurnButton;

    [SerializeField] private List<MiniSavingGoalUI> goalUIs = new List<MiniSavingGoalUI>();

    [Header("Task 3: The 90-Day Saving Sprint")]
    [SerializeField] private TMP_Text weekText;
    [SerializeField] private TMP_Text savingsText;
    [SerializeField] private TMP_Text incomeText;
    [SerializeField] private TMP_Text decisionText;
    [SerializeField] private Button optionAButton;
    [SerializeField] private Button optionBButton;
    [SerializeField] private TMP_Text leaderboardResult;

    [SerializeField] private int task3Points = 30;

    private int currentWeek = 0;
    private int task3Savings = 0;
    private int task3Income = 0;
    private int targetSavings = 2000;

    private List<SavingDecision> weeklyDecisions;

    private int totalBudget = 2000;
    private int totalSaved = 0;
    private int turnCount = 0;

    private List<MiniSavingGoal> miniGoals;


    private int mainCounter = 0;
    private bool task1Completed = false;
    private bool task2Completed = false;
    private bool task3Completed = false;
    private bool isProgress = false;

    void Start()
    {
        InitializeAllTasks();

        int leoProgress = PlayerPrefs.GetInt("leo", 12); // default to 7 if not set

        switch (leoProgress)
        {
            case 12:
                mainCounter = 0;
                // Start from Task 1
                break;

            case 13:
                mainCounter = 3;
                // Start from Task 2
                break;

            case 14:
                mainCounter = 5;
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

    void InitializeAllTasks()
    {
        lessonMainParentUI.SetActive(false);
        task1UI.SetActive(false);
        task2UI.SetActive(false);
        task3UI.SetActive(false);
        confirmationPanel.SetActive(false);
    }

    void Progress()
    {
        int leoProgress = PlayerPrefs.GetInt("leo", 12); // default to 7 if not set

        switch (leoProgress)
        {
            case 12:
                mainCounter = 0;
                isProgress = true;
                // Start from Task 1
                break;

            case 13:
                mainCounter = 3;
                isProgress = true;
                // Start from Task 2
                break;

            case 14:
                mainCounter = 5;
                isProgress = true;
                // Start from Task 3
                break;

            default:
                Debug.LogWarning("Unexpected leo progress value: " + leoProgress);
                mainCounter = 0;
                // fallback
                break;
        }
    }

    public void TaskFlow_NextBtn()
    {
        if (!isProgress)
        {
            //Progress();
            isProgress = true;
        }
        GameManagerModule4.instance.CurrentLevelProgress(mainCounter, 6.0f);
        switch (mainCounter)
        {
            case 0:
                GameManagerModule4.instance.NPCsNames.text = "Leo";
                GameManagerModule4.instance.DialogueText.text = "My best savings hack? Automation. I treat saving like a bill I have to pay.";
                GameManagerModule4.instance.DialogPanel.SetActive(true);
                GameManagerModule4.instance.DialogueBox.SetActive(true);
                mainCounter++;
                break;

            case 1:
                GameManagerModule4.instance.NPCsNames.text = "Leo";
                GameManagerModule4.instance.DialogueText.text = "You give it a try, Alex. Use the app to set up an automatic savings plan.";
                mainCounter++;
                break;

            case 2:
                GameManagerModule4.instance.DialogueBox.SetActive(false);
                lessonMainParentUI.SetActive(true);
                task1UI.SetActive(true);
                SetupTask1();
                break;
            case 3:
                GameManagerModule4.instance.NPCsNames.text = "Leo";
                GameManagerModule4.instance.DialogueText.text = "I like to turn saving into a game. Let’s see how many goals you can hit!";
                GameManagerModule4.instance.DialogPanel.SetActive(true);
                GameManagerModule4.instance.DialogueBox.SetActive(true);
                mainCounter++;
                break;

            case 4:
                GameManagerModule4.instance.DialogueBox.SetActive(false);
                lessonMainParentUI.SetActive(true);
                task1UI.SetActive(false);
                task2UI.SetActive(true);
                SetupTask2();
                break;

            case 5:
                GameManagerModule4.instance.NPCsNames.text = "Leo";
                GameManagerModule4.instance.DialogueText.text = "Now you’re on your own. 90 days, €2000 income every 30 days — let’s see if you’ve got what it takes.";
                GameManagerModule4.instance.DialogPanel.SetActive(true);
                GameManagerModule4.instance.DialogueBox.SetActive(true);
                mainCounter++;
                break;

            case 6:
                GameManagerModule4.instance.DialogueBox.SetActive(false);
                lessonMainParentUI.SetActive(true);
                task2UI.SetActive(false);
                task3UI.SetActive(true);
                SetupTask3();
                break;


        }

    }

    void SetupTask1()
    {


        frequencyDropdown.ClearOptions();
        frequencyDropdown.AddOptions(new List<string> { "Weekly", "Monthly" });

        targetAccountDropdown.ClearOptions();
        targetAccountDropdown.AddOptions(new List<string> { "Emergency Fund", "Travel Fund", "Investments" });

        amountInput.text = "";
        confirmationPanel.SetActive(false);
        confirmationMessage.text = "";

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(ValidateAndConfirmTransfer);

        Debug.Log("Task 1 Setup Complete");
        appButton.onClick.RemoveAllListeners();
        appButton.onClick.AddListener(() =>
        {
            appPanel.SetActive(true);
            confirmationPanel.SetActive(false);
        });
    }

    void ValidateAndConfirmTransfer()
    {
        if (!int.TryParse(amountInput.text, out int amount) || amount <= 0)
        {
            GameManagerModule4.instance.Toast.Show("Enter a valid amount to save!", 2f);
            GameManagerModule4.instance.PopUpShow(1);
            return;
        }

        string frequency = frequencyDropdown.options[frequencyDropdown.value].text;
        string targetAccount = targetAccountDropdown.options[targetAccountDropdown.value].text;

        // Define acceptable saving ranges
        int minAllowed = frequency == "Monthly" ? 100 : 50;
        int maxAllowed = frequency == "Monthly" ? 1000 : 500;

        if (amount < minAllowed)
        {
            GameManagerModule4.instance.Toast.Show(
                $"That amount feels a bit too low. Try Amount between 100 - 50 or 1000 - 500!", 3f);
            GameManagerModule4.instance.PopUpShow(1);
            return;
        }

        if (amount > maxAllowed)
        {
            GameManagerModule4.instance.Toast.Show(
                $"That’s a lot! Try Amount between 100 - 50 or 1000 - 500.", 3f);
            GameManagerModule4.instance.PopUpShow(1);
            return;
        }

        int months = 6;
        int totalSaved = frequency == "Monthly"
            ? amount * months
            : amount * (months * 4); // Approx 4 weeks per month

        confirmationPanel.SetActive(true);
        confirmationMessage.text = $"Auto-transfer setup complete!\n" +
                                    $"Saving ${amount} {frequency.ToLower()} into your {targetAccount}.\n" +
                                    $"Projected savings in 6 months: **${totalSaved}**";

        GameManagerModule4.instance.PopUpShow(0);
        CompleteTask1();
    }


    void CompleteTask1()
    {
        if (task1Completed) return;

        task1Completed = true;
        GameManagerModule4.instance.AddPoint(task1Points);
        PlayerPrefs.SetInt("leo", 13);
        //GameManagerModule4.instance.UnlockBadge("Auto-Saver");
        GameManagerModule4.instance.Toast.Show($"+{task1Points} points!  Auto-Saver badge unlocked!", 1.5f);

        mainCounter = 3; // Task 2 ready
        Invoke("TaskFlow_NextBtn", 1.5f);
    }

    //=======Task 2=========

    void SetupTask2()
    {
        task2UI.SetActive(true);
        totalBudget = 2000;
        totalSaved = 0;
        turnCount = 0;

        miniGoals = new List<MiniSavingGoal>
    {
        new MiniSavingGoal { title = "Save €100 in 1 turn", targetAmount = 100, duration = 1 },
        new MiniSavingGoal { title = "Save €300 in 2 turns", targetAmount = 300, duration = 2 },
        new MiniSavingGoal { title = "Save €600 in 3 turns", targetAmount = 600, duration = 3 }
    };

        for (int i = 0; i < goalUIs.Count; i++)
        {
            goalUIs[i].UpdateUI(miniGoals[i]);
        }

        UpdateBudgetUI();

        nextTurnButton.onClick.RemoveAllListeners();
        nextTurnButton.onClick.AddListener(() => RunTurn());
    }

    void RunTurn()
    {
        turnCount++;

        for (int i = 0; i < miniGoals.Count; i++)
        {
            var goal = miniGoals[i];

            if (goal.isComplete) continue;

            int contribution = Mathf.CeilToInt((float)goal.targetAmount / goal.duration);

            if (totalBudget >= contribution)
            {
                goal.savedAmount += contribution;
                goal.turnsUsed++;
                totalBudget -= contribution;

                if (goal.savedAmount >= goal.targetAmount)
                {
                    goal.savedAmount = goal.targetAmount;
                    goal.isComplete = true;
                    totalSaved += goal.savedAmount;

                    GameManagerModule4.instance.AddPoint(10);
                    GameManagerModule4.instance.Toast.Show($" Goal Completed: {goal.title}\n+10 points, €{goal.savedAmount} saved!", 3f);
                }

                goalUIs[i].UpdateUI(goal);
            }
        }

        UpdateBudgetUI();

        if (miniGoals.TrueForAll(g => g.isComplete))
        {
            GameManagerModule4.instance.PopUpShow(0);
            CompleteTask2();
        }
    }

    void UpdateBudgetUI()
    {
        totalBudgetText.text = $"Budget Left: €{totalBudget}";
        totalSavedText.text = $"Total Saved: €{totalSaved}";
    }

    void CompleteTask2()
    {
        task2Completed = true;
        task2UI.SetActive(false);

        PlayerPrefs.SetInt("leo", 14);

        GameManagerModule4.instance.Toast.Show(" All mini goals completed! Great job saving smart!", 1.5f);
        mainCounter = 5;
        Invoke("TaskFlow_NextBtn", 1.5f);
    }

    //=======Task 3=========

    void SetupTask3()
    {
        task3UI.SetActive(true);
        currentWeek = 0;
        task3Savings = 0;
        task3Income = 0;

        weeklyDecisions = new List<SavingDecision>
    {
        new SavingDecision { description = "Buy new shoes or skip this month?", optionA = "Buy Shoes (-€100)", optionB = "Skip & Save (€100)", saveImpact = 100 },
        new SavingDecision { description = "Dinner out with friends?", optionA = "Go Out (-€150)", optionB = "Cook at Home (€150)", saveImpact = 150 },
        new SavingDecision { description = "New phone or delay purchase?", optionA = "Buy Now (-€700)", optionB = "Wait & Save (€700)", saveImpact = 700 },
        new SavingDecision { description = "Weekend trip or staycation?", optionA = "Trip (-€250)", optionB = "Stay Home (€250)", saveImpact = 250 },
        new SavingDecision { description = "Fancy coffee every day?", optionA = "Daily Coffee (-€50)", optionB = "Make at Home (€50)", saveImpact = 50 },
        new SavingDecision { description = "Buy video game?", optionA = "Buy Game (-€120)", optionB = "Wait for Sale (€120)", saveImpact = 120 },
        new SavingDecision { description = "Streaming subscriptions?", optionA = "Keep All (-€70)", optionB = "Cut Unused (€70)", saveImpact = 70 },
        new SavingDecision { description = "Monthly clothes shopping?", optionA = "Buy Clothes (-€500)", optionB = "Pause Shopping (€500)", saveImpact = 500 },
        new SavingDecision { description = "New gadget or wait?", optionA = "Buy Gadget (-€200)", optionB = "Save for Later (€200)", saveImpact = 200 },
        new SavingDecision { description = "Order food tonight?", optionA = "Order Food (-€40)", optionB = "Cook Meal (€40)", saveImpact = 40 },
        new SavingDecision { description = "Rent a movie or use free platform?", optionA = "Rent Movie (-€15)", optionB = "Use Free App (€15)", saveImpact = 15 },
        new SavingDecision { description = "Buy bonus decor or skip?", optionA = "Buy Decor (-€90)", optionB = "Skip Decor (€90)", saveImpact = 90 }
    };

        optionAButton.onClick.RemoveAllListeners();
        optionBButton.onClick.RemoveAllListeners();

        optionAButton.onClick.AddListener(() => HandleDecision(false));
        optionBButton.onClick.AddListener(() => HandleDecision(true));

        ShowCurrentWeek();
    }

    void ShowCurrentWeek()
    {
        if (currentWeek >= 12)
        {
            CompleteTask3();
            return;
        }

        // Add income every 4 weeks
        if (currentWeek % 4 == 0)
        {
            task3Income += 2000;
            GameManagerModule4.instance.Toast.Show("+€2000 income received", 2f);
        }

        weekText.text = $"Week {currentWeek + 1} / 12";
        savingsText.text = $"Saved: €{task3Savings} / €{targetSavings}";
        incomeText.text = $"Income Available: €{task3Income}";

        var decision = weeklyDecisions[currentWeek];
        decisionText.text = decision.description;
        optionAButton.GetComponentInChildren<TMP_Text>().text = decision.optionA;
        optionBButton.GetComponentInChildren<TMP_Text>().text = decision.optionB;
    }

    void HandleDecision(bool saveOption)
    {
        var decision = weeklyDecisions[currentWeek];

        if (saveOption)
        {
            task3Savings += decision.saveImpact;
            task3Income -= decision.saveImpact;
        }
        else
        {
            task3Income -= decision.saveImpact; // negative impact (spend)
        }

        currentWeek++;
        ShowCurrentWeek();
    }


    void CompleteTask3()
    {
        task3Completed = true;
        task3UI.SetActive(false);

        if (task3Savings >= targetSavings)
        {
            GameManagerModule4.instance.AddPoint(task3Points);
            //GameManagerModule4.instance.UnlockBadge("Super Savers");
            GameManagerModule4.instance.PopUpShow(0);
            GameManagerModule4.instance.Toast.Show($" You saved €{task3Savings}! +{task3Points} points!\n🏅 Certificate of Savings Expertise unlocked!", 1.5f);
        }
        else
        {
            int penalty = targetSavings - task3Savings;
            GameManagerModule4.instance.Toast.Show($" You fell short of the goal.\n€{penalty} has been deducted from your savings account.", 2f);
        }
        leaderboardResult.text = $"Final Savings: €{task3Savings} / €{targetSavings}";
        leaderboardResult.gameObject.SetActive(true);

        GameManagerModule4.instance.RewardPanell();

        PlayerPrefs.SetInt("gameprogress", 28);
        PlayerPrefs.SetInt("leo", 15);


        GameManagerModule4.instance.rewardPanelNextButton.onClick.RemoveAllListeners();
        GameManagerModule4.instance.rewardPanelNextButton.onClick.AddListener(() =>
        {
            Debug.Log("Module 4 Finished. Proceeding via GameManager.");
            this.enabled = false;
            GameManagerModule4.instance.NextRewardPanelBtn();
        });
    }

    public void Retry()
    {
        PlayerPrefs.SetInt("leo", 0);
        PlayerPrefs.SetInt("gameprogress", 27);
        SceneManager.LoadScene(5);
    }


}


[System.Serializable]
public class MiniSavingGoal
{
    public string title;
    public int targetAmount;
    public int duration; // in turns
    public int savedAmount = 0;
    public int turnsUsed = 0;
    public bool isComplete = false;
}

[System.Serializable]
public class SavingDecision
{
    public string description;
    public string optionA; // Spend
    public string optionB; // Save or Smart decision
    public int saveImpact; // Positive if saved, negative if spent
}

