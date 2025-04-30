// Lesson4_Controller.cs
// Manages three financial literacy tasks: goal matching, needs vs wants categorization, and savings simulation
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

// Define enums in separate files in production
public enum GoalType { Vacation, Emergency, Education, Business, Retirement }
public enum ReasonType { ShortTermFun, FinancialSecurity, LongTermInvestment }
public enum PurchaseCategory { Need, Want, CanWait }
public enum MoneySourceType { LeoSavedFund, LeoEssentials, AlexWallet }

public class Lesson4_Controller : MonoBehaviour
{
    [Header("Overall Lesson Setup")]
    [Tooltip("Parent GameObject holding Task1, Task2, Task3 children")]
    [SerializeField] private GameObject lesson4MainParentUI;
    [Tooltip("Child GameObject holding Task 1 UI and elements")]
    [SerializeField] private GameObject task1UI;
    [Tooltip("Child GameObject holding Task 2 UI and elements")]
    [SerializeField] private GameObject task2UI;
    [Tooltip("Child GameObject holding Task 3 UI and elements")]
    [SerializeField] private GameObject task3UI;

    // Task 1: Goal Matching - Match financial goals with appropriate reasons
    [Header("Task 1 Specifics")]
    [SerializeField] private int task1_totalGoalsToMatch = 5;
    [SerializeField] private int task1_pointsReward = 15;
    [SerializeField] private int task1_moneyReward = 200;
    private int task1_correctMatchesCount = 0;
    private bool task1_Completed = false;
    public bool task1_IsIncorrectState = true; // Track if any mistakes were made

    // Task 2: Needs vs Wants - Categorize purchases to identify potential savings
    [Header("Task 2 Specifics")]
    [SerializeField] private int task2_totalItemsToMatch = 4;
    [SerializeField] private int task2_pointsReward = 10;
    [SerializeField] private TextMeshProUGUI task2_totalSavingsText;
    private int task2_itemsSortedCount = 0;
    private float task2_totalPotentialSavings = 0f;
    private bool task2_Completed = false;

    // Task 3: Savings Simulation - Handle emergency expenses scenario
    [Header("Task 3 Specifics")]
    [SerializeField] private GameObject task3_forkAreaUI; // Choice buttons panel
    [SerializeField] private GameObject task3_paymentAreaUI; // Payment drop area panel
    [SerializeField] private GameObject task3_paymentDropZoneObject; // The actual drop zone
    [SerializeField] private GameObject task3_savedMoneyObject;
    [SerializeField] private GameObject task3_leoMoneyObject;
    [SerializeField] private GameObject task3_alexMoneyObject;
    [SerializeField] private float task3_repairCost = 500f;
    [SerializeField] private int task3_pointsReward = 15;
    [SerializeField] private string task3_badgeRewardID = "LeosTrust";
    public enum Task3BranchState { None, ChoiceMade, AwaitingPayment_Saved, AwaitingPayment_NotSaved, PaymentInProgress, Completed }
    public Task3BranchState task3_currentState = Task3BranchState.None;
    private GameObject task3_activeMoneySource1 = null;
    private GameObject task3_activeMoneySource2 = null;
    private bool task3_Completed = false;

    // General State
    public int mainCounter = 0; // Controls flow through dialogue and tasks
    private bool lessonStarted = false;
    private bool isProgress = false;

    // Initialization
    void Start()
    {
        Lesson3_14.AnimationLevel3 = false;
        // Ensure GameManager exists
        if (GameManagerModule4.instance == null)
        {
            Debug.LogError("GameManagerModule4 instance not found! Lesson4_Controller cannot function.");
            this.enabled = false;
            return;
        }

        
    }

    /*void Progress()
    {
        int leoProgress = PlayerPrefs.GetInt("leo", 0); // default to 7 if not set

        switch (leoProgress)
        {
            case 0:
                mainCounter = 0;
                isProgress = true;
                 // Start from Task 1
                break;

            case 1:
                mainCounter = 2;
                isProgress = true;
                 // Start from Task 2
                break;

            case 2:
                mainCounter = 4;
                isProgress = true;
                 // Start from Task 3
                break;

            default:
                Debug.LogWarning("Unexpected leo progress value: " + leoProgress);
                mainCounter = 0;
                 // fallback
                break;
        }
    }*/

    // Called when script is enabled
    void OnEnable()
    {
        if (!lessonStarted)
        {
            InitializeStatesAndUI();
            lessonStarted = true;
        }
        else
        {
            Debug.Log("Lesson4_Controller re-enabled.");
            ReactivateCurrentTaskUI();
        }
    }

    // Reset all states and UI elements
    void InitializeStatesAndUI()
    {
        Debug.Log("Initializing Lesson4_Controller States and UI");
        mainCounter = 0;
        lessonStarted = false;

        InitializeTask1State();
        InitializeTask2State();
        InitializeTask3State();

        // Hide all task UIs initially
        task1UI.SetActive(false);
        task2UI.SetActive(false);
        task3UI.SetActive(false);
        //lesson4MainParentUI?.SetActive(true);
    }

    // Show appropriate UI based on current state
    void ReactivateCurrentTaskUI()
    {
        task1UI?.SetActive(mainCounter >= 1 && mainCounter < 2);
        task2UI?.SetActive(mainCounter >= 3 && mainCounter < 4);
        task3UI?.SetActive(mainCounter >= 5 && mainCounter < 8);

        // Handle Task 3 sub-states
        if (task3_currentState == Task3BranchState.ChoiceMade) task3_forkAreaUI?.SetActive(true);
        if (task3_currentState == Task3BranchState.AwaitingPayment_Saved ||
           task3_currentState == Task3BranchState.AwaitingPayment_NotSaved ||
           task3_currentState == Task3BranchState.PaymentInProgress) task3_paymentAreaUI?.SetActive(true);
    }

    // Initialize Task 1 variables
    void InitializeTask1State()
    {
        task1_correctMatchesCount = 0;
        task1_Completed = false;
        task1_IsIncorrectState = true;
    }

    // Initialize Task 2 variables
    void InitializeTask2State()
    {
        task2_itemsSortedCount = 0;
        task2_totalPotentialSavings = 0f;
        task2_Completed = false;
        UpdateTask2SavingsUI();
    }

    // Initialize Task 3 variables
    void InitializeTask3State()
    {
        task3_currentState = Task3BranchState.None;
        task3_Completed = false;
        task3_forkAreaUI?.SetActive(false);
        task3_paymentAreaUI?.SetActive(false);
        task3_savedMoneyObject?.SetActive(false);
        task3_leoMoneyObject?.SetActive(false);
        task3_alexMoneyObject?.SetActive(false);
        task3_activeMoneySource1 = null;
        task3_activeMoneySource2 = null;
    }

    // Main flow control - called by GameManager's Next Button
    public void TaskFlow_NextBtn()
    {
        GameManagerModule4.instance.CurrentLevelProgress(mainCounter, 8.0f);
        if (!isProgress)
        {
            //Progress();
            isProgress = true;
        }
        Debug.Log($"Lesson4_Controller TaskFlow_NextBtn called. Advancing from mainCounter: {mainCounter}");

        switch (mainCounter)
        {
            // Task 1 Intro
            case 0:
                InitializeStatesAndUI();
                lesson4MainParentUI?.SetActive(true);
                lessonStarted = true;

                GameManagerModule4.instance.DialogueText.text = "I saved up enough to start my own business, but I still always keep a certain part of my earnings to a side, you know, cuz it's important. People don't realize how great of a tool saving us. Let me show you";
                GameManagerModule4.instance.NPCsNames.text = "Leo";
                GameManagerModule4.instance.DialogPanel.SetActive(true);
                GameManagerModule4.instance.DialogueBox.SetActive(true);
                mainCounter++;
                break;

            // Task 1 Activity
            case 1:
                GameManagerModule4.instance.DialogueBox.SetActive(false);
                task1UI?.SetActive(true);
                Debug.Log("Task 1 active.");
                break;

            // Task 2 Intro
            case 2:
                task1UI?.SetActive(false);

                GameManagerModule4.instance.DialogueText.text = "I know you learned a lot about needs and wants, so, show me what can do, go over my purchases, and tell me how much I could have saved by categorizing what I spend.";
                GameManagerModule4.instance.DialogPanel.SetActive(true);
                GameManagerModule4.instance.DialogueBox.SetActive(true);
                mainCounter++;
                break;

            // Task 2 Activity
            case 3:
                GameManagerModule4.instance.DialogueBox.SetActive(false);
                UpdateTask2SavingsUI();
                Debug.Log("Task 2 active.");
                lesson4MainParentUI?.SetActive(true);
                task2UI?.SetActive(true);
                break;

            // Task 3 Intro
            case 4:
                task2UI?.SetActive(false);

                GameManagerModule4.instance.DialogueText.text = "Okay, Alex, let's see. You can go over my savings, to help me deal with the broke lights in my studio.";
                GameManagerModule4.instance.DialogPanel.SetActive(true);
                GameManagerModule4.instance.DialogueBox.SetActive(true);
                mainCounter++;
                break;

            // Task 3 Emergency
            case 5:
            
                GameManagerModule4.instance.DialogueText.text = "Oh dear, Alex! Emergency! The main lights... cost 500€!";
                mainCounter++;
                break;

            // Task 3 Choice Intro
            case 6:
                GameManagerModule4.instance.DialogueText.text = "Let's simulate two possibilities... Pick a scenario!";
                mainCounter++;
                break;

            // Task 3 Choice Panel
            case 7:

                GameManagerModule4.instance.DialogueBox.SetActive(false);
                lesson4MainParentUI?.SetActive(true);
                task3UI?.SetActive(true);
                task3_forkAreaUI?.SetActive(true);
                task3_currentState = Task3BranchState.ChoiceMade;
                Debug.Log("Task 3 choice active.");
                mainCounter++;
                break;

            // Task 3 Payment
            case 8:
                GameManagerModule4.instance.DialogueBox.SetActive(false);
                task3_forkAreaUI?.SetActive(false);
                task3_paymentAreaUI?.SetActive(true);
                mainCounter++;
                break;
            case 9 :
                GameManagerModule4.instance.DialogueBox.SetActive(false);
                task3_paymentAreaUI?.SetActive(false);
                task3UI?.SetActive(false);
                lesson4MainParentUI?.SetActive(false);
                GameManagerModule4.instance.DialogPanel.SetActive(false);
                GameManagerModule4.instance.DialogueBox.SetActive(false);
                
                Invoke("TriggerFinalRewards", 0.5f);
                break;

            default:
                Debug.LogWarning($"TaskFlow_NextBtn: Reached end or unhandled counter: {mainCounter}");
                break;
        }
    }

    // --- TASK 1: GOAL MATCHING ---

    // Check if goal and reason are correctly matched
    public bool Task1_IsMatchCorrect(GoalType goal, ReasonType reason)
    {
        switch (goal)
        {
            case GoalType.Vacation: return reason == ReasonType.ShortTermFun;
            case GoalType.Emergency: return reason == ReasonType.FinancialSecurity;
            case GoalType.Education: case GoalType.Business: case GoalType.Retirement: return reason == ReasonType.LongTermInvestment;
            default: return false;
        }
    }

    // Handle correct match
    public void Task1_ProcessCorrectMatch(GoalData matchedGoalData, GameObject matchedObject, Transform dropZoneTransform)
    {
        if (task1_Completed) return;
        GameManagerModule4.instance.PopUpShow(0);
        task1_correctMatchesCount++;
        task1_IsIncorrectState = false;

        // Visual feedback
        var draggable = matchedObject.GetComponent<DraggableGoal>(); if (draggable != null) draggable.enabled = false;
        var cg = matchedObject.GetComponent<CanvasGroup>(); if (cg != null) cg.interactable = false;
        matchedObject.transform.SetParent(dropZoneTransform, true);
        matchedObject.transform.position = dropZoneTransform.position;
        matchedGoalData.feedbackPanel?.SetActive(true);

        Debug.Log($"Task 1: Correct Match {task1_correctMatchesCount}/{task1_totalGoalsToMatch}");
        CheckTask1Completion();
    }

    // Handle incorrect match
    public void Task1_ProcessIncorrectMatch(GoalData attemptedGoalData)
    {
        if (task1_Completed) return;
        Debug.Log($"Task 1: Incorrect match for {attemptedGoalData.goalType}");
        task1_IsIncorrectState = true;
        attemptedGoalData.ResetPosition();
        GameManagerModule4.instance.PopUpShow(1);
    }

    // Check if Task 1 is complete
    void CheckTask1Completion()
    {
        if (!task1_Completed && task1_correctMatchesCount >= task1_totalGoalsToMatch)
        {
            task1_Completed = true;
            Debug.Log("Task 1 Completed!");
            HandleTask1Completion();
        }
    }

    // Called by Submit Button
    public void Task1_SubmitBtn()
    {
        // Validate completion
        if (task1_correctMatchesCount < task1_totalGoalsToMatch)
        {
            GameManagerModule4.instance.Toast?.Show("Please match all the goals first!", 2f);
            GameManagerModule4.instance.PopUpShow(1);
            return;
        }

        // Check for errors
        if (task1_IsIncorrectState)
        {
            GameManagerModule4.instance.Toast?.Show("Double-check your matches, something isn't right.", 2f);
            GameManagerModule4.instance.PopUpShow(1);
            return;
        }

        // Complete if all valid
        if (!task1_Completed)
        {
            task1_Completed = true;
            Debug.Log("Task 1 Completed via Submit!");
            HandleTask1Completion();
            
        }
    }

    // Process Task 1 completion
    void HandleTask1Completion()
    {
        task1UI.SetActive(false);
        // Award rewards
        GameManagerModule4.instance.AddPoint(task1_pointsReward);
        GameManagerModule4.instance.AddBankMoney(task1_moneyReward, 1);
        PlayerPrefs.SetInt("leo", 1);

        // Show feedback
        GameManagerModule4.instance.Toast?.Show($"Task 1 Complete! +{task1_pointsReward} Points, +{task1_moneyReward}€ Banked", 1.5f);

        // Advance to next task
        mainCounter = 2;
        Debug.Log("Advancing mainCounter to " + mainCounter + " for Task 2 dialogue.");
        Invoke("TaskFlow_NextBtn", 1.5f);
    }

    // --- TASK 2: NEEDS VS WANTS ---

    // Check if item was dropped in correct category
    public bool Task2_IsValidDrop(PurchaseItemData itemData, PurchaseCategory targetCategory)
    {
        switch (itemData.itemName)
        {
            case "Groceries": case "Rent": return targetCategory == PurchaseCategory.Need;
            case "Headphones": case "New Sneakers": return targetCategory == PurchaseCategory.Want || targetCategory == PurchaseCategory.CanWait;
            default: Debug.LogWarning($"Task 2: Unknown item: {itemData.itemName}"); return false;
        }
    }

    // Handle successful item categorization
    public void Task2_ProcessItemSorted(PurchaseItemData itemData, GameObject itemObject, PurchaseCategory category, Transform dropZoneTransform)
    {
        if (task2_Completed || itemData.isSorted) return;

        itemData.isSorted = true;
        task2_itemsSortedCount++;

        // Update UI
        var draggable = itemObject.GetComponent<DraggableGoal>(); if (draggable != null) draggable.enabled = false;
        var cg = itemObject.GetComponent<CanvasGroup>(); if (cg != null) cg.interactable = false;
        itemObject.transform.SetParent(dropZoneTransform, true);
        itemObject.transform.position = dropZoneTransform.position;

        // Track potential savings
        if (category == PurchaseCategory.CanWait)
        {
            task2_totalPotentialSavings += itemData.cost;
            UpdateTask2SavingsUI();
            GameManagerModule4.instance.PopUpShow(0);
        }

        if(category == PurchaseCategory.Need)
        {
            GameManagerModule4.instance.PopUpShow(0);
        }

        // Show feedback
        //GameManagerModule4.instance.Toast?.Show(itemData.feedbackTip, 3f);
        CheckTask2Completion();
    }

    // Handle incorrect categorization
    public void Task2_ProcessIncorrectDrop(PurchaseItemData itemData)
    {
        if (task2_Completed) return;
        itemData.ResetPosition();
        GameManagerModule4.instance.PopUpShow(1);
    }

    // Update savings display
    void UpdateTask2SavingsUI()
    {
        if (task2_totalSavingsText != null)
        {
            task2_totalSavingsText.text = $"Potential Savings: €{task2_totalPotentialSavings:F2}";
        }
    }

    // Check if Task 2 is complete
    void CheckTask2Completion()
    {
        if (!task2_Completed && task2_itemsSortedCount >= task2_totalItemsToMatch)
        {
            task2_Completed = true;
            Debug.Log("Task 2 Completed!");
            
            HandleTask2Completion();
        }
    }

    // Process Task 2 completion
    void HandleTask2Completion()
    {
        // Award rewards
        task2UI.SetActive(false);
        GameManagerModule4.instance.AddPoint(task2_pointsReward);
        GameManagerModule4.instance.AddMoney((int)task2_totalPotentialSavings);
        PlayerPrefs.SetInt("leo", 2);

        // Show feedback
        GameManagerModule4.instance.Toast?.Show($"Task 2 Complete! +{task2_pointsReward} Points, +{(int)task2_totalPotentialSavings}€ Saved!", 1.5f);

        // Advance to next task
        mainCounter = 4;
        Debug.Log("Advancing mainCounter to " + mainCounter + " for Task 3 dialogue.");
        Invoke("TaskFlow_NextBtn", 1.5f);
    }

    // --- TASK 3: SAVINGS SIMULATION ---

    // Handle "Saved Money" branch selection
    public void Task3_ChooseBranch_Saved()
    {
        if (task3_currentState != Task3BranchState.ChoiceMade) return;
        task3_currentState = Task3BranchState.AwaitingPayment_Saved;
        task3_forkAreaUI?.SetActive(false);
        //task3_paymentAreaUI?.SetActive(true);
        Task3_ActivateMoneyObject(task3_savedMoneyObject, ref task3_activeMoneySource1);
        task3_activeMoneySource2 = null;
        GameManagerModule4.instance.DialogueText.text = "Phew! Drag the saved money to the payment slot.";

        GameManagerModule4.instance.DialogueBox.SetActive(true);
    }

    // Handle "No Savings" branch selection
    public void Task3_ChooseBranch_NotSaved()
    {
        if (task3_currentState != Task3BranchState.ChoiceMade) return;
        task3_currentState = Task3BranchState.AwaitingPayment_NotSaved;
        task3_forkAreaUI?.SetActive(false);
        //task3_paymentAreaUI?.SetActive(true);
        Task3_ActivateMoneyObject(task3_leoMoneyObject, ref task3_activeMoneySource1);
        Task3_ActivateMoneyObject(task3_alexMoneyObject, ref task3_activeMoneySource2);
        GameManagerModule4.instance.DialogueText.text = "Oh no! Drag either your money or my budget cuts to pay.";
        GameManagerModule4.instance.DialogueBox.SetActive(true);
    }

    // Process payment drop on Task 3
    public void Task3_ProcessPaymentDrop(MoneySourceType sourceType, float amount, GameObject droppedMoneyObject)
    {
        // Validate state
        if (task3_currentState == Task3BranchState.Completed ||
           (task3_currentState != Task3BranchState.AwaitingPayment_Saved && task3_currentState != Task3BranchState.AwaitingPayment_NotSaved))
        {
            droppedMoneyObject?.SendMessage("ResetPosition", SendMessageOptions.DontRequireReceiver);
            return;
        }

        Debug.Log($"Processing drop of {sourceType}, amount {amount}");

        // Validate amount
        if (amount < task3_repairCost)
        {
            Task3_RejectDrop(droppedMoneyObject, $"This doesn't cover the full {task3_repairCost:C} cost.");
            return;
        }

        // Validate source matches scenario
        if (task3_currentState == Task3BranchState.AwaitingPayment_Saved && sourceType != MoneySourceType.LeoSavedFund)
        {
            Task3_RejectDrop(droppedMoneyObject, "That's not the money for this scenario.");
            return;
        }
        if (task3_currentState == Task3BranchState.AwaitingPayment_NotSaved && sourceType == MoneySourceType.LeoSavedFund)
        {
            Task3_RejectDrop(droppedMoneyObject, "Leo didn't have saved funds in this scenario.");
            return;
        }

        // Process payment
        bool paymentSuccessful = false;
        switch (sourceType)
        {
            case MoneySourceType.LeoSavedFund:
                paymentSuccessful = true;
                break;

            case MoneySourceType.LeoEssentials:
                paymentSuccessful = true;
                break;

            case MoneySourceType.AlexWallet:
                int currentAlexMoney = PlayerPrefs.GetInt("walletmoney", 0);
                if (currentAlexMoney >= (int)task3_repairCost)
                {
                    GameManagerModule4.instance.RemoveMoney((int)task3_repairCost);
                    paymentSuccessful = true;
                }
                else
                {
                    Task3_RejectDrop(droppedMoneyObject, "You don't have enough money in your wallet!");
                    paymentSuccessful = false;
                }
                break;
        }

        // Complete task if payment successful
        if (paymentSuccessful)
        {
            Task3_HandleSuccessfulPayment(sourceType, droppedMoneyObject);
        }
    }

    // Helper method to activate money objects
    private void Task3_ActivateMoneyObject(GameObject moneyObject, ref GameObject activeRef)
    {
        if (moneyObject != null)
        {
            moneyObject.SetActive(true);
            moneyObject.SendMessage("StoreStartPosition", SendMessageOptions.DontRequireReceiver);
            moneyObject.SendMessage("ResetPosition", SendMessageOptions.DontRequireReceiver);
            var dragScript = moneyObject.GetComponent<DraggableMoneySource>(); if (dragScript != null) dragScript.enabled = true;
            var cg = moneyObject.GetComponent<CanvasGroup>(); if (cg != null) { cg.interactable = true; cg.blocksRaycasts = true; }
            activeRef = moneyObject;
        }
        else Debug.LogError("Task 3: Money object ref missing!");
    }

    // Process successful payment
    void Task3_HandleSuccessfulPayment(MoneySourceType sourceType, GameObject droppedMoneyObject)
    {
        task3UI?.SetActive(false);
        task3_currentState = Task3BranchState.Completed;
        task3_Completed = true;
        Debug.Log($"Task 3: Payment successful using {sourceType}");

        // Update UI
        if (droppedMoneyObject != null)
        {
            droppedMoneyObject.transform.position = task3_paymentDropZoneObject.transform.position;
            var dragScript = droppedMoneyObject.GetComponent<DraggableMoneySource>(); if (dragScript != null) dragScript.enabled = false;
            var cg = droppedMoneyObject.GetComponent<CanvasGroup>(); if (cg != null) { cg.interactable = false; cg.blocksRaycasts = false; }
            droppedMoneyObject.SetActive(false);
        }
        GameObject otherSource = (droppedMoneyObject == task3_activeMoneySource1) ? task3_activeMoneySource2 : task3_activeMoneySource1;
        if (otherSource != null) { otherSource.SetActive(false); }
        task3_paymentAreaUI?.SetActive(false);
        GameManagerModule4.instance.DialogPanel.SetActive(false);
        GameManagerModule4.instance.DialogueBox.SetActive(false);

        // Show outcome dialogue
        string outcomeDialogue = "";
        switch (sourceType)
        {
            case MoneySourceType.LeoSavedFund: outcomeDialogue = "Leo: Phew! Good thing I had that emergency fund..."; break;
            case MoneySourceType.LeoEssentials: outcomeDialogue = "Leo: Okay, repair paid... but I had to cut back..."; break;
            case MoneySourceType.AlexWallet: outcomeDialogue = "Leo: Thanks for covering it, Alex! I really owe you..."; break;
        }
        GameManagerModule4.instance.DialogueText.text = outcomeDialogue;
        GameManagerModule4.instance.DialogPanel.SetActive(true);
        GameManagerModule4.instance.DialogueBox.SetActive(true);

        //Invoke("TriggerFinalRewards", 0.5f);
    }

    // Handle rejected payment attempts
    void Task3_RejectDrop(GameObject droppedMoneyObject, string reason)
    {
        Debug.Log($"Task 3: Drop rejected: {reason}");
        droppedMoneyObject?.SendMessage("ResetPosition", SendMessageOptions.DontRequireReceiver);
        GameManagerModule4.instance.PopUpShow(1);
        GameManagerModule4.instance.Toast?.Show(reason, 1.5f);
    }

    // Process final rewards after lesson completion
    void TriggerFinalRewards()
    {
        if (!task3_Completed) return;
        Debug.Log("Triggering FINAL Rewards for Module 4");
        GameManagerModule4.instance.PopUpShow(0);

        GameManagerModule4.instance.DialogPanel.SetActive(false);
        GameManagerModule4.instance.DialogueBox.SetActive(false);

        GameManagerModule4.instance.AddPoint(task3_pointsReward);


        GameManagerModule4.instance.RewardPanell();

        PlayerPrefs.SetInt("gameprogress", 24);
        PlayerPrefs.SetInt("leo", 3);

        if (GameManagerModule4.instance.rewardPanelNextButton != null)
        {
            GameManagerModule4.instance.rewardPanelNextButton.onClick.RemoveAllListeners();
            GameManagerModule4.instance.rewardPanelNextButton.onClick.AddListener(() =>
            {
                Debug.Log("Module 4 Finished. Proceeding via GameManager.");
                this.enabled = false;
                GameManagerModule4.instance.NextRewardPanelBtn();
            });
        }
        else Debug.LogError("Final Reward Panel Next Button missing!");
    }

    public void Retry()
    {
        PlayerPrefs.SetInt("leo", 0);
        PlayerPrefs.SetInt("gameprogress", 23);
        SceneManager.LoadScene(5);
    }
}