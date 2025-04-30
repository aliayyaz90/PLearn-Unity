using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lesson4_4_Controller : MonoBehaviour
{
    [Header("Overall Lesson Setup")]
    [SerializeField] private GameObject lessonMainParentUI;
    [SerializeField] private GameObject task1UI;
    [SerializeField] private GameObject task2UI;
    [SerializeField] private GameObject task3UI;

    [Header("Task 1: Emergency Fund")]
    [SerializeField] private TMP_Text monthlyExpensesText;
    [SerializeField] private TMP_Text monthsText;
    [SerializeField] private TMP_InputField userInputField;
    [SerializeField] private Slider monthsSlider;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private Button task1SubmitButton;

    [SerializeField] private int monthlyExpenses = 2000;
    [SerializeField] private int task1Points = 10;
    private int targetFundMin;
    private int targetFundMax;

    [Header("Task 2: Fund Prioritization")]
    [SerializeField] private GameObject task2Panel;
    [SerializeField] private Slider rentSlider;
    [SerializeField] private Slider groceriesSlider;
    [SerializeField] private Slider studioRentSlider;
    [SerializeField] private Slider entertainmentSlider;
    [SerializeField] private Slider impulseSlider;

    [SerializeField] private TMP_Text rentLabel;
    [SerializeField] private TMP_Text groceriesLabel;
    [SerializeField] private TMP_Text studioRentLabel;
    [SerializeField] private TMP_Text entertainmentLabel;
    [SerializeField] private TMP_Text impulseLabel;


    [SerializeField] private Slider fundGrowthSlider;
    [SerializeField] private TMP_Text fundGrowthLabel;
    [SerializeField] private Button task2SubmitButton;

    [SerializeField] private int baseExpenseTotal = 2000;
    [SerializeField] private int surplus = 500;
    [SerializeField] private int task2Points = 10;

    private int rentBase = 700;
    private int groceriesBase = 400;
    private int studioRentBase = 500;
    private int entertainmentBase = 200;
    private int impulseBase = 200;

    private int currentFundGrowth = 0;

    [Header("Task 3: Emergency Fund Simulation")]
    [SerializeField] private TMP_Text eventDescriptionText;
    [SerializeField] private TMP_Text emergencyFundText;
    [SerializeField] private TMP_Text monthText;
    [SerializeField] private Button useFundButton;
    [SerializeField] private Button delayExpenseButton;

    [SerializeField] private int task3Points = 15;

    private int emergencyFund = 0; // From Task 2
    private int currentMonth = 0;
    private int totalMonths = 6;
    private int debtCount = 0;

    private List<EmergencyEvent> emergencyEvents;




    [Header("Main")]
    private int mainCounter = 0;
    private bool task1Completed = false;
    private bool task2Completed = false;
    private bool task3Completed = false;
    private bool isProgress = false;

    void Start()
    {
        InitializeAllTasks();

        int leoProgress = PlayerPrefs.GetInt("leo", 9); // default to 7 if not set

        switch (leoProgress)
        {
            case 9:
                mainCounter = 0;
                // Start from Task 1
                break;

            case 10:
                mainCounter = 3;
                // Start from Task 2
                break;

            case 11:
                mainCounter = 5;
                // Start from Task 3
                break;

            default:
                Debug.LogWarning("Unexpected leo progress value: " + leoProgress);
                mainCounter = 0;
                isProgress = true;

                break;
        }
    }

    void InitializeAllTasks()
    {
        lessonMainParentUI.SetActive(false);
        task1UI.SetActive(false);
        task2UI.SetActive(false);
        task3UI.SetActive(false);
    }

    void Progress()
    {
        int leoProgress = PlayerPrefs.GetInt("leo", 9); // default to 7 if not set

        switch (leoProgress)
        {
            case 9:
                mainCounter = 0;
                isProgress = true;
                break;

            case 10:
                mainCounter = 3;
                isProgress = true;
                // Start from Task 2
                break;

            case 11:
                mainCounter = 5;
                isProgress = true;
                // Start from Task 3
                break;

            default:
                Debug.LogWarning("Unexpected leo progress value: " + leoProgress);
                mainCounter = 0;

                break;
        }
    }

    public void TaskFlow_NextBtn()
    {
        if (!isProgress)
        {
            //Progress();
        }
        GameManagerModule4.instance.CurrentLevelProgress(mainCounter, 6.0f);
        switch (mainCounter)
        {
            case 0:
                GameManagerModule4.instance.NPCsNames.text = "Leo";
                GameManagerModule4.instance.DialogueText.text = "A storm just damaged my workshop equipment. Thank goodness for my emergency fund!";
                GameManagerModule4.instance.DialogPanel.SetActive(true);
                GameManagerModule4.instance.DialogueBox.SetActive(true);
                mainCounter++;
                break;

            case 1:
                GameManagerModule4.instance.NPCsNames.text = "Leo";
                GameManagerModule4.instance.DialogueText.text = "You see how my emergency savings account saved me from all these problems? I need you to handle the fund allocation now.";
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
                GameManagerModule4.instance.DialogueText.text = "Every month I’ve got a bit of surplus left — do your magic and see how much more we can save.";
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
                GameManagerModule4.instance.DialogueText.text = "Life throws curveballs. Let’s see how you handle a few surprises over the next 6 months.";
                GameManagerModule4.instance.DialogPanel.SetActive(true);
                GameManagerModule4.instance.DialogueBox.SetActive(true);
                mainCounter++;
                break;

            case 6:
                GameManagerModule4.instance.DialogueBox.SetActive(false);
                lessonMainParentUI.SetActive(true);
                task3UI.SetActive(true);
                SetupTask3();
                break;


        }
    }

    //=========Task 1=========
    void SetupTask1()
    {
        monthlyExpensesText.text = $"Monthly Expenses: ${monthlyExpenses}";
        monthsSlider.minValue = 1;
        monthsSlider.maxValue = 12;
        monthsSlider.wholeNumbers = true;
        monthsSlider.value = 3; // Default to 3 months

        monthsSlider.onValueChanged.RemoveAllListeners();
        monthsSlider.onValueChanged.AddListener(OnSliderChanged);

        userInputField.onValueChanged.RemoveAllListeners();
        userInputField.onValueChanged.AddListener(OnUserInputChanged);

        task1SubmitButton.onClick.RemoveAllListeners();
        task1SubmitButton.onClick.AddListener(ValidateEmergencyFund);

        targetFundMin = monthlyExpenses * 3;
        targetFundMax = monthlyExpenses * 6;

        UpdateFundDisplay();
    }

    void OnSliderChanged(float value)
    {
        monthsText.text = $"{value} months";
        UpdateFundDisplay();
    }

    void OnUserInputChanged(string input)
    {
        UpdateFundDisplay();
    }

    void UpdateFundDisplay()
    {
        int selectedMonths = (int)monthsSlider.value;
        int suggestedAmount = monthlyExpenses * selectedMonths;

        int.TryParse(userInputField.text, out int userAmount);

        if (userAmount == 0)
        {
            feedbackText.text = $"Try estimating {selectedMonths} months: ${suggestedAmount}";
            feedbackText.color = Color.yellow;
        }
        else if (userAmount < targetFundMin)
        {
            feedbackText.text = "Too low. Leo might not be covered!";
            feedbackText.color = Color.red;
        }
        else if (userAmount > targetFundMax)
        {
            feedbackText.text = "A bit too high — maybe Leo is saving too much?";
            feedbackText.color = new Color(1f, 0.6f, 0f); // orange
        }
        else
        {
            feedbackText.text = "Perfect! That’s a healthy emergency fund.";
            feedbackText.color = Color.green;
        }
    }

    void ValidateEmergencyFund()
    {
        int.TryParse(userInputField.text, out int userAmount);

        if (userAmount >= targetFundMin && userAmount <= targetFundMax)
        {
            GameManagerModule4.instance.PopUpShow(0);
            CompleteTask1();
        }
        else
        {
            GameManagerModule4.instance.PopUpShow(1);
            GameManagerModule4.instance.Toast.Show("Try adjusting your estimate to 3–6 months of expenses!", 2f);
        }
    }

    void CompleteTask1()
    {
        task1Completed = true;
        task1UI.SetActive(false);
        GameManagerModule4.instance.AddPoint(task1Points);
        PlayerPrefs.SetInt("leo", 10);
        //GameManagerModule4.instance.UnlockBadge("Rainy Day Ready");
        GameManagerModule4.instance.Toast.Show($"+{task1Points} points! 🏅 Rainy Day Ready unlocked!", 1.5f);

        mainCounter = 3; // Move to Task 2 in future
        Invoke("TaskFlow_NextBtn", 1.5f);
    }

    //=========Task 2=========
    void SetupTask2()
    {
        task2UI.SetActive(true);
        currentFundGrowth = 0;

        // Set base values
        rentSlider.maxValue = rentBase;
        rentSlider.value = rentBase;

        groceriesSlider.maxValue = groceriesBase;
        groceriesSlider.value = groceriesBase;

        studioRentSlider.maxValue = studioRentBase;
        studioRentSlider.value = studioRentBase;

        entertainmentSlider.maxValue = entertainmentBase;
        entertainmentSlider.value = entertainmentBase;

        impulseSlider.maxValue = impulseBase;
        impulseSlider.value = impulseBase;

        // Set all listeners
        rentSlider.onValueChanged.AddListener(delegate { OnExpenseSliderChanged(); });
        groceriesSlider.onValueChanged.AddListener(delegate { OnExpenseSliderChanged(); });
        studioRentSlider.onValueChanged.AddListener(delegate { OnExpenseSliderChanged(); });
        entertainmentSlider.onValueChanged.AddListener(delegate { OnExpenseSliderChanged(); });
        impulseSlider.onValueChanged.AddListener(delegate { OnExpenseSliderChanged(); });

        fundGrowthSlider.interactable = false;
        fundGrowthSlider.minValue = 0;
        fundGrowthSlider.maxValue = surplus;

        OnExpenseSliderChanged(); // Initial update

        task2SubmitButton.onClick.RemoveAllListeners();
        task2SubmitButton.onClick.AddListener(ValidateTask2);



        fundGrowthSlider.minValue = 0;
        fundGrowthSlider.maxValue = 900;
        fundGrowthSlider.value = 0;
        fundGrowthLabel.text = $"Extra Saved: $0 of ${surplus}";

    }

    void OnExpenseSliderChanged()
    {
        // Update labels
        rentLabel.text = $"Rent: ${Mathf.RoundToInt(rentSlider.value)}";
        groceriesLabel.text = $"Groceries: ${Mathf.RoundToInt(groceriesSlider.value)}";
        studioRentLabel.text = $"Studio Rent: ${Mathf.RoundToInt(studioRentSlider.value)}";
        entertainmentLabel.text = $"Entertainment: ${Mathf.RoundToInt(entertainmentSlider.value)}";
        impulseLabel.text = $"Impulse Buys: ${Mathf.RoundToInt(impulseSlider.value)}";

        // Calculate savings from unnecessary cuts
        int entertainmentSaved = entertainmentBase - (int)entertainmentSlider.value;
        int impulseSaved = impulseBase - (int)impulseSlider.value;

        currentFundGrowth = entertainmentSaved + impulseSaved + surplus; // look up HandleChoice(bool useFund), this fund is being ussed in task 3 fund

        fundGrowthSlider.value = currentFundGrowth;
        fundGrowthLabel.text = $"Extra Saved: ${currentFundGrowth} of ${surplus}";

    }


    void ValidateTask2()
    {
        // Check if necessary expenses were reduced (they should NOT be)
        bool rentLowered = rentSlider.value < rentBase;
        bool groceriesLowered = groceriesSlider.value < groceriesBase;
        bool studioLowered = studioRentSlider.value < studioRentBase;

        if (rentLowered || groceriesLowered || studioLowered)
        {
            GameManagerModule4.instance.Toast.Show("You can't reduce necessary expenses like rent or groceries!", 2f);
            GameManagerModule4.instance.PopUpShow(1);
            return;
        }

        // Check if any unnecessary expenses were reduced (they MUST be)
        bool unnecessaryReduced = entertainmentSlider.value < entertainmentBase || impulseSlider.value < impulseBase;

        if (!unnecessaryReduced)
        {
            GameManagerModule4.instance.Toast.Show("Try cutting non-essential expenses like entertainment or impulse buys!", 2f);
            GameManagerModule4.instance.PopUpShow(1);
            return;
        }

        // All good
        GameManagerModule4.instance.PopUpShow(0);
        CompleteTask2();
    }


    void CompleteTask2()
    {
        task2Completed = true;
        task2UI.SetActive(false);

        GameManagerModule4.instance.AddPoint(task2Points);
        GameManagerModule4.instance.AddMoney(currentFundGrowth); // Save into fund
        GameManagerModule4.instance.Toast.Show($"+{task2Points} points, +${currentFundGrowth} saved!", 1.5f);
        PlayerPrefs.SetInt("leo", 11);

        mainCounter = 5; // Proceed to Task 3
        Invoke("TaskFlow_NextBtn", 1.5f);
    }

    //=========Task 3=========
    void SetupTask3()
    {
        task3UI.SetActive(true);
        currentMonth = 0;
        debtCount = 0;

        emergencyFund = currentFundGrowth; // Carry over from Task 2

        emergencyEvents = new List<EmergencyEvent>
    {
        new EmergencyEvent { description = "Car repair needed after a breakdown", cost = 300 },
        new EmergencyEvent { description = "Unexpected medical bill", cost = 200 },
        new EmergencyEvent { description = "Home plumbing emergency", cost = 250 },
        new EmergencyEvent { description = "Leo’s laptop broke", cost = 400 },
        new EmergencyEvent { description = "Lost a freelance gig income", cost = 150 },
        new EmergencyEvent { description = "Dental emergency: tooth filling", cost = 180 }
    };

        useFundButton.onClick.RemoveAllListeners();
        delayExpenseButton.onClick.RemoveAllListeners();

        useFundButton.onClick.AddListener(() => HandleChoice(true));
        delayExpenseButton.onClick.AddListener(() => HandleChoice(false));

        ShowCurrentEvent();
    }

    void ShowCurrentEvent()
    {
        if (currentMonth >= totalMonths)
        {
            CompleteTask3();
            return;
        }

        var currentEvent = emergencyEvents[currentMonth];
        eventDescriptionText.text = $"Month {currentMonth + 1}: {currentEvent.description}\nCost: ${currentEvent.cost}";
        emergencyFundText.text = $"Emergency Fund: ${emergencyFund}";
        monthText.text = $"Month {currentMonth + 1} of {totalMonths}";

        useFundButton.interactable = emergencyFund >= currentEvent.cost;
    }

    void HandleChoice(bool useFund)
    {
        var currentEvent = emergencyEvents[currentMonth];


        if (useFund && emergencyFund >= currentEvent.cost)
        {
            GameManagerModule4.instance.PopUpShow(0);
            emergencyFund -= currentEvent.cost;
            GameManagerModule4.instance.Toast.Show($" Covered with emergency fund. Remaining: ${emergencyFund}", 2f);
        }
        else
        {
            debtCount++;
            GameManagerModule4.instance.Toast.Show($" Expense delayed! You’re now in minor debt.", 2f);
        }

        if (debtCount > 2)
        {
            delayExpenseButton.interactable = false;
        }

        currentMonth++;
        ShowCurrentEvent();
    }

    void CompleteTask3()
    {
        task3Completed = true;
        task3UI.SetActive(false);

        if (debtCount <= 2)
        {
            GameManagerModule4.instance.AddPoint(task3Points);
            //GameManagerModule4.instance.UnlockBadge("Crisis Controller");
            GameManagerModule4.instance.Toast.Show($"+{task3Points} points!  Crisis Controller badge unlocked!", 1.5f);
        }
        else
        {
            GameManagerModule4.instance.Toast.Show("Try to prepare better next time! You had to delay too many expenses.", 1.5f);
        }

        GameManagerModule4.instance.RewardPanell();

        PlayerPrefs.SetInt("gameprogress", 27);
        PlayerPrefs.SetInt("leo", 12);

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
        PlayerPrefs.SetInt("gameprogress", 26);
        SceneManager.LoadScene(5);
    }
}

[System.Serializable]
public class EmergencyEvent
{
    public string description;
    public int cost;
}


