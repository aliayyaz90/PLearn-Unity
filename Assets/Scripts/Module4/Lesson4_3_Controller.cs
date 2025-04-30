using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum BudgetCategory { Needs, Wants, Savings }
public enum Task1State { NeedsPhase, WantsPhase, SavingsPhase, Completed }

public class Lesson4_3_Controller : MonoBehaviour
{
    [Header("Overall Lesson Setup")]
    [SerializeField] private GameObject lessonMainParentUI;
    [SerializeField] private GameObject task1UI;
    [SerializeField] private GameObject task2UI;
    [SerializeField] private GameObject task3UI;

    [Header("Task 1: 50/30/20 Rule")]
    [SerializeField] private Button task1NextButton;
    [SerializeField] private DropZone4 budgetDropZone;
    [SerializeField] private Slider allocationSlider;
    [SerializeField] private DraggableBudgetItem[] budgetItems;
    [SerializeField] private TMP_Text percentageText;
    [SerializeField] private int totalBudget = 3000;
    private float remainingBudget;
    [SerializeField] private TextMeshProUGUI remainingBudgetText;
    [SerializeField] private float tolerance = 2f;
    [SerializeField] private int task1Points = 10;

    [Header("Task 2 - Smart Saving Tips")]
    [SerializeField] private List<SavingTip> savingTips;
    [SerializeField] private Transform goodDropZone;
    [SerializeField] private Transform badDropZone;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private int task2Points = 10;
    [SerializeField] private int rewardCash = 150;
    private int currentTipIndex = 0;
    [SerializeField] private Transform tipStartPosition;

    [Header("Task 3 - Weekly Options")]
    [SerializeField] private List<WeeklyAction> weeklyActions = new List<WeeklyAction>();

    [SerializeField] private GameObject actionButtonPrefab; // Prefab with TMP_Texts for label/value
    [SerializeField] private Transform actionButtonContainer; // Parent where the 3 buttons go
    [SerializeField] private Slider savingsProgressSlider;
    [SerializeField] private TMP_Text savingsAmountText;
    [SerializeField] private TMP_Text weekText;
    [SerializeField] private TMP_Text savingsPercentText;

    [SerializeField] private int task3Points = 15;
    [SerializeField] private int initialSavings = 0;
    [SerializeField] private int maxPossibleSavings = 400;
    [SerializeField] private int maxWeeks = 8;

    private List<int> savingsOverTime = new List<int>();
    private int totalSavings = 0;
    private int currentWeek = 0;


    // State tracking
    private int mainCounter = 0;
    private bool task1Completed = false;
    private bool task2Completed = false;
    private bool task3Completed = false;
    private BudgetCategory currentCategory;
    private bool isProgress = false;

    void Start()
    {
        InitializeAllTasks();
        remainingBudget = totalBudget;
        UpdateRemainingBudgetText();
        lessonMainParentUI.SetActive(true);

    }

    void InitializeAllTasks()
    {
        task1UI.SetActive(false);
        task2UI.SetActive(false);
        task3UI.SetActive(false);
        allocationSlider.gameObject.SetActive(false);

        foreach (var item in budgetItems)
        {
            item.gameObject.SetActive(true);
            item.Initialize(this);
        }
    }
    void Progress()
    {
        int leoProgress = PlayerPrefs.GetInt("leo", 6); // default to 7 if not set

        switch (leoProgress)
        {
            case 6:
                mainCounter = 0;
                isProgress = true;
                // Start from Task 1
                break;

            case 7:
                mainCounter = 3;
                // Start from Task 2
                isProgress = true;
                break;

            case 8:
                mainCounter = 5;
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
        }

        Debug.Log($"Lesson4_3 Advancing from counter: {mainCounter}");
        GameManagerModule4.instance.CurrentLevelProgress(mainCounter, 6.0f);
        switch (mainCounter)
        {
            case 0: // Lesson start
                GameManagerModule4.instance.NPCsNames.text = "Leo";
                GameManagerModule4.instance.DialogueText.text = "Alright Alex, here's $3000 - use the 50/30/20 rule!";
                GameManagerModule4.instance.DialogPanel.SetActive(true);
                GameManagerModule4.instance.DialogueBox.SetActive(true);
                mainCounter++;
                break;

            case 1:
                lessonMainParentUI.SetActive(true);
                task1UI.SetActive(true);
                GameManagerModule4.instance.DialogueBox.SetActive(false);
                mainCounter++;
                break;

            case 2: // Task 1 active
                if (!task1Completed)
                {
                    GameManagerModule4.instance.Toast.Show("Complete the budget allocation first!", 2f);
                    return;
                }
                mainCounter++;
                break;

            case 3: // Transition to Task 2
                GameManagerModule4.instance.NPCsNames.text = "Leo";
                GameManagerModule4.instance.DialogueText.text = "Now, Alex, you have to think carefully, of all the activities that have been laid out in front of you, which ones do you think will be the most effective or help full?";
                GameManagerModule4.instance.DialogPanel.SetActive(true);
                GameManagerModule4.instance.DialogueBox.SetActive(true);

                SetupTask2();
                mainCounter++;
                break;

            case 4: // Task 2 active
                lessonMainParentUI.SetActive(true);
                task2UI.SetActive(true);
                GameManagerModule4.instance.DialogueBox.SetActive(false);

                mainCounter++;
                break;

            case 5: // Task 3 intro
                GameManagerModule4.instance.NPCsNames.text = "Leo";
                GameManagerModule4.instance.DialogueText.text = "Let’s build a savings habit! Each week, choose how to save money.";
                GameManagerModule4.instance.DialogPanel.SetActive(true);
                GameManagerModule4.instance.DialogueBox.SetActive(true);

                mainCounter++;
                break;

            case 6: // Activate Task 3 UI
                GameManagerModule4.instance.DialogueBox.SetActive(false);
                lessonMainParentUI.SetActive(true);
                task3UI.SetActive(true);
                SetupTask3();
                mainCounter++;
                break;

        }
    }

    // ======== TASK 1: BUDGET ALLOCATION ========
    public void HandleBudgetItemDrop(BudgetCategory category)
    {
        if (task1Completed) return;

        if (mainCounter == 1 && category != BudgetCategory.Needs)
        {
            RejectDrop("Start with Needs category!");
            return;
        }

        currentCategory = category;
        allocationSlider.gameObject.SetActive(true);
        percentageText.text = "0%";
        allocationSlider.value = 0f;
        allocationSlider.interactable = true;

        float target = GetTargetPercentage();

        allocationSlider.onValueChanged.RemoveAllListeners();
        allocationSlider.onValueChanged.AddListener(v => UpdateSlider(v, target));

        task1NextButton.interactable = true; // 🔹 Always enabled
        task1NextButton.onClick.RemoveAllListeners(); // avoid duplicate listeners
        task1NextButton.onClick.AddListener(() => OnClickTask1NextButton(target));

    }



    void UpdateSlider(float value, float target)
    {
        float snappedPercentage = Mathf.Round(value / 10f) * 10f;
        percentageText.text = $"{snappedPercentage:0}%";

        // Color only for visual feedback
        percentageText.color = Mathf.Abs(snappedPercentage - target) <= tolerance
            ? Color.green
            : Color.red;
    }



    public void OnClickTask1NextButton(float target)
    {
        float snappedPercentage = Mathf.Round(allocationSlider.value / 10f) * 10f;

        if (Mathf.Abs(snappedPercentage - target) <= tolerance)
        {
            CompleteCategory();
            task1NextButton.interactable = false;
        }
        else
        {
            RejectDrop("Start with Needs category!");
            percentageText.color = Color.red;

            // Optional: Shake animation or visual feedback can go here
        }
    }


    void CompleteCategory()
    {
        GameManagerModule4.instance.PopUpShow(0);

        allocationSlider.gameObject.SetActive(false);

        float percentage = GetTargetPercentage();
        float allocatedAmount = (percentage / 100f) * totalBudget;
        remainingBudget -= allocatedAmount;
        UpdateRemainingBudgetText();

        foreach (var item in budgetItems)
        {
            if (item.category == currentCategory)
            {
                item.gameObject.SetActive(false);
                break;
            }
        }

        if (budgetItems.All(i => !i.gameObject.activeSelf))
        {
            //GameManagerModule4.instance.PopUpShow(0);
            CompleteTask1();
        }
    }

    void RejectDrop(string message)
    {
        GameManagerModule4.instance.PopUpShow(1);
        GameManagerModule4.instance.Toast.Show(message, 2f);
    }

    void UpdateRemainingBudgetText()
    {
        remainingBudgetText.text = $"{remainingBudget} / {totalBudget}";
    }


    float GetTargetPercentage()
    {
        return currentCategory switch
        {
            BudgetCategory.Needs => 50f,
            BudgetCategory.Wants => 30f,
            BudgetCategory.Savings => 20f,
            _ => 0f
        };
    }

    void CompleteTask1()
    {
        task1UI.SetActive(false);
        task1Completed = true;
        GameManagerModule4.instance.AddPoint(task1Points);
        GameManagerModule4.instance.Toast.Show($"+{task1Points} points!", 1.5f);
        PlayerPrefs.SetInt("leo", 7);

        mainCounter = 3;


        Invoke("TaskFlow_NextBtn", 1.5f);
    }

    // ======== TASK 2: BUDGET Decicion ========

    void SetupTask2()
    {
        //task2UI.SetActive(true);
        feedbackText.text = "";
        currentTipIndex = 0;

        // Deactivate all tip objects
        foreach (var tip in savingTips)
        {
            tip.uiElement.SetActive(false);
        }



        // Assign reference scripts to each
        foreach (var tip in savingTips)
        {
            var refScript = tip.uiElement.GetComponent<SavingTipReference>();
            if (refScript == null)
            {
                refScript = tip.uiElement.AddComponent<SavingTipReference>();
            }
            refScript.tipData = tip;
            refScript.controller = this; // ✅ link controller for callback
            tip.uiElement.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tip.description;
        }

        LoadNextTip();
    }

    public void EvaluateTip(SavingTipReference tipRef, bool isDroppedInGoodZone)
    {
        bool isCorrect = (tipRef.tipData.isGood && isDroppedInGoodZone) || (!tipRef.tipData.isGood && !isDroppedInGoodZone);

        if (isCorrect)
        {
            GameManagerModule4.instance.PopUpShow(0);

            tipRef.gameObject.SetActive(false);
            currentTipIndex++;

            if (currentTipIndex >= savingTips.Count)
            {

                CompleteTask2();
            }
            else
            {
                LoadNextTip();
            }
        }
        else
        {
            GameManagerModule4.instance.PopUpShow(1);
            feedbackText.text = $" {tipRef.tipData.description} is actually {(tipRef.tipData.isGood ? "GOOD" : "BAD")} for saving.";
        }
    }

    void LoadNextTip()
    {
        var nextTip = savingTips[currentTipIndex];
        nextTip.uiElement.SetActive(true);
        nextTip.uiElement.transform.SetParent(task2UI.transform);
        nextTip.uiElement.transform.position = tipStartPosition.position;
    }

    void CompleteTask2()
    {
        GameManagerModule4.instance.PopUpShow(0);

        task2Completed = true;
        task2UI.SetActive(false);
        feedbackText.text = "✅ All done! You mastered saving tips!";
        GameManagerModule4.instance.AddPoint(task2Points);
        GameManagerModule4.instance.AddMoney(rewardCash);
        PlayerPrefs.SetInt("leo", 8);

        GameManagerModule4.instance.Toast.Show($"+{task2Points} points, +${rewardCash}!", 1.5f);
        GameManagerModule4.instance.DialogueText.text = "Leo: You're getting smart with money, Alex!";
        mainCounter = 5;
        Invoke("TaskFlow_NextBtn", 1.5f);
    }

    // ======== TASK 2: BUDGET Decicion ========


    void SetupTask3()
    {
        Debug.Log("Setting up Task 3");

        if (weeklyActions.Count == 0)
        {
            weeklyActions = new List<WeeklyAction>
    {
        new WeeklyAction { description = "Cancel streaming subscription", savingsImpact = 20 },
        new WeeklyAction { description = "Sell old clothes", savingsImpact = 30 },
        new WeeklyAction { description = "Cook at home", savingsImpact = 25 },
        new WeeklyAction { description = "Save cashback", savingsImpact = 15 },
        new WeeklyAction { description = "Skip takeout", savingsImpact = 20 },
        new WeeklyAction { description = "Bike instead of Uber", savingsImpact = 10 },
        new WeeklyAction { description = "No-spend weekend", savingsImpact = 40 },
    };
        }

        totalSavings = initialSavings;
        currentWeek = 0;
        savingsOverTime.Clear();
        savingsOverTime.Add(totalSavings);
        UpdateSavingsUI();

        GenerateWeeklyChoices();

    }

    void GenerateWeeklyChoices()
    {
        // Clear previous buttons
        foreach (Transform child in actionButtonContainer)
        {
            Destroy(child.gameObject);
        }

        // Shuffle and take 3 random actions
        List<WeeklyAction> shuffled = weeklyActions.OrderBy(a => Random.value).ToList();
        List<WeeklyAction> currentChoices = shuffled.Take(3).ToList();

        foreach (var action in currentChoices)
        {
            GameObject btnObj = Instantiate(actionButtonPrefab, actionButtonContainer);
            btnObj.GetComponentInChildren<TMP_Text>().text = action.description + " : $" + action.savingsImpact;

            Button btn = btnObj.GetComponent<Button>();
            btn.onClick.AddListener(() => OnActionChosen(action));
        }

        weekText.text = $"Week {currentWeek + 1} of {maxWeeks}";
    }

    void OnActionChosen(WeeklyAction action)
    {
        totalSavings += action.savingsImpact;
        savingsOverTime.Add(totalSavings);
        currentWeek++;

        UpdateSavingsUI();

        if (currentWeek >= maxWeeks)
        {
            GameManagerModule4.instance.PopUpShow(0);
            CompleteTask3();
        }
        else
        {
            GenerateWeeklyChoices();
        }
    }
    void UpdateSavingsUI()
    {
        float progress = Mathf.Clamp01((float)totalSavings / maxPossibleSavings);
        savingsProgressSlider.value = progress;

        savingsAmountText.text = $"Leo's Savings: ${totalSavings}";
        savingsPercentText.text = $"+{Mathf.RoundToInt(progress * 100)}% Savings";
    }
    void CompleteTask3()
    {
        task3UI.SetActive(false);
        task3Completed = true;

        int gift = totalSavings / 2;

        GameManagerModule4.instance.AddPoint(task3Points);
        GameManagerModule4.instance.AddMoney(gift);
        //GameManagerModule4.instance.UnlockBadge("Habit Builder");


        GameManagerModule4.instance.RewardPanell();

        PlayerPrefs.SetInt("gameprogress", 26);
        PlayerPrefs.SetInt("leo", 9);

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
        PlayerPrefs.SetInt("gameprogress", 25);
        SceneManager.LoadScene(5);
    }


}

[System.Serializable]
public class WeeklyAction
{
    public string description;
    public int savingsImpact;
}