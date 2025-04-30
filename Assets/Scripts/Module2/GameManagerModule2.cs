using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ToastWrapper;

public class GameManagerModule2 : MonoBehaviour
{
    public static GameManagerModule2 instance;

    [SerializeField] public Text InteractionText, CurrentLevelText, CurrentLevelProgressText, CoinsProgressText, PointsProgressText, DialogueText, OtherRewards, NPCsNames, fps, MoneyText, PointsText, LevelText;
    [SerializeField] public GameObject InteractionHolder, DialogPanel, wallet, RewardPanel, PopupBg, DialogueBox, LivesPrompt, BankmImg, WarningPanel, BreakPanel, thompsonNPC, alexGO, PatelGO5;
    [Header("Others")]
    public Toast Toast;
    [SerializeField] public Button nextButton;
    [SerializeField] public Image blackImageForFade;
    [SerializeField] Sprite goodNoti,badNoti;
    public bool fadein = false;
    public int walletMoney;
    public GameObject[] Lives, NpcImages;
    public Transform player;
    [SerializeField] ParticleSystem con1, con2;


    private float polling = 1f, time;
    private int frameCount;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        Invoke("blackScreenOff", 1f);

        CurrentLevel();
        CoinsProgress();
        PointsProgress();
        if (PlayerPrefs.GetInt("patel") == 0)
            GetComponent<Narratives>().enabled=true;

        
    }
    private void Start()
    {
        LessonScriptsActivation();
        Application.targetFrameRate = 60;
        AddMoney(0);
        AddPoint(0);
        AddBankMoney(0, 0);
    }
    private void Update()
    {
        if (blackImageForFade.GetComponent<CanvasGroup>().alpha > 0)
            blackImageForFade.GetComponent<CanvasGroup>().alpha -= Time.deltaTime * 0.8f;
        if (fadein)
        {
            if (blackImageForFade.GetComponent<CanvasGroup>().alpha < 1)
                blackImageForFade.GetComponent<CanvasGroup>().alpha += Time.deltaTime * 1.2f;
        }
        time += Time.deltaTime;
        frameCount++;
        if (time > polling)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            fps.text = frameRate.ToString();
            time -= polling;
            frameCount = 0;
        }
    }
    private void blackScreenOff()
    {
        blackImageForFade.gameObject.SetActive(false);
    }
    public void MainMenu()
    {
        if (GetComponent<Lesson2_1>().enabled)
            GetComponent<Lesson2_1>().Retry();
        else if (GetComponent<Lesson2_2>().enabled)
            GetComponent<Lesson2_2>().Retry();
        else if (GetComponent<Lesson2_3>().enabled)
            GetComponent<Lesson2_3>().Retry();
        else if (GetComponent<Lesson2_4>().enabled)
            GetComponent<Lesson2_4>().Retry();
        else if (GetComponent<Lesson2_5>().enabled)
            GetComponent<Lesson2_5>().Retry();
        SceneManager.LoadScene(1);
    }
    public void OnClickYesBtn()
    {
        InteractionHolder.SetActive(false);
        NextBtn();
        PlayerPrefs.SetInt("warn", 0);
    }
    public void OnClickNoBtn()
    {
        InteractionHolder.SetActive(false);
    }
    public void NextBtn()
    {
        if (GetComponent<Narratives>().enabled)
            GetComponent<Narratives>().ShowNarratives();
        else if (GetComponent<Lesson2_1>().enabled)
            GetComponent<Lesson2_1>().Task2_1NextBtn();
        else if (GetComponent<Lesson2_2>().enabled)
            GetComponent<Lesson2_2>().Task2_2NextBtn();
        else if (GetComponent<Lesson2_3>().enabled)
            GetComponent<Lesson2_3>().Task2_3NextBtn();
        else if (GetComponent<Lesson2_4>().enabled)
            GetComponent<Lesson2_4>().Task2_4NextBtn();
        else if (GetComponent<Lesson2_5>().enabled)
            GetComponent<Lesson2_5>().Task2_5NextBtn();
    }
    public void AddMoney(int amount)
    {
        PlayerPrefs.SetInt("walletmoney", PlayerPrefs.GetInt("walletmoney") + amount);
        wallet.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "$" + PlayerPrefs.GetInt("walletmoney").ToString();
    }
    public void RemoveMoney(int amount)
    {
        if ((Int32)PlayerPrefs.GetInt("walletmoney") - amount >= 0)
        {
            PlayerPrefs.SetInt("walletmoney", PlayerPrefs.GetInt("walletmoney") - amount);
            wallet.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "$" + PlayerPrefs.GetInt("walletmoney").ToString();
        }
    }
    public void AddBankMoney(int amount, int reward)
    {
        if (reward == 0)
        {
            if (PlayerPrefs.GetInt("walletmoney") >= amount)
            {
                PlayerPrefs.SetInt("bankmoney", PlayerPrefs.GetInt("bankmoney") + amount);
                wallet.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "$" + PlayerPrefs.GetInt("bankmoney").ToString();
            }
        }
        else if (reward == 1)
        {
            PlayerPrefs.SetInt("bankmoney", PlayerPrefs.GetInt("bankmoney") + amount);
            wallet.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "$" + PlayerPrefs.GetInt("bankmoney").ToString();
        }

    }
    public void RemoveBankMoney(int amount)
    {
        if ((Int32)PlayerPrefs.GetInt("bankmoney") - amount >= 0)
        {
            PlayerPrefs.SetInt("bankmoney", PlayerPrefs.GetInt("bankmoney") - amount);
            wallet.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "$" + PlayerPrefs.GetInt("bankmoney").ToString();
        }
    }
    public void AddPoint(int amount)
    {
        PlayerPrefs.SetInt("walletpoint", PlayerPrefs.GetInt("walletpoint") + amount);
        wallet.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetInt("walletpoint").ToString();
    }
    public void RemovePoint(int amount)
    {
        if ((Int32)PlayerPrefs.GetInt("walletpoint") - amount >= 0)
        {
            PlayerPrefs.SetInt("walletpoint", PlayerPrefs.GetInt("walletpoint") - amount);
            wallet.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetInt("walletpoint").ToString();
        }
    }
    public void LevelComplete()
    {
        Invoke("LevelTransition", 2);
    }
    public void LevelTransition()
    {
        blackImageForFade.gameObject.SetActive(true);
        fadein = true;
        Invoke("LevelSelectorLoader", 3);
    }
    private void LevelSelectorLoader()
    {
        if (PlayerPrefs.GetInt("gameprogress") == 9 || PlayerPrefs.GetInt("lives") == -1)
            SceneManager.LoadScene(1);
        else
            SceneManager.LoadScene(3);
    }
    public void LivesDeduct()
    {
        int ActiveChecker = PlayerPrefs.GetInt("lives");
        if (ActiveChecker >= 0)
        {
            Lives[ActiveChecker].gameObject.SetActive(false);
            PlayerPrefs.SetInt("lives", --ActiveChecker);
        }
    }
    public void LivesAdd()
    {
        int ActiveChecker = PlayerPrefs.GetInt("lives");
        if (ActiveChecker < 2)
        {
            ActiveChecker++;
            Lives[ActiveChecker].gameObject.SetActive(true);
            PlayerPrefs.SetInt("lives", ActiveChecker);
        }
    }
    public void LivesCheck()
    {
        int ActiveChecker = PlayerPrefs.GetInt("lives");
        for (int i = 0; i < Lives.Length; i++)
        {
            if (i <= ActiveChecker)
                Lives[i].gameObject.SetActive(true);
            else if (i > ActiveChecker)
                Lives[i].gameObject.SetActive(false);
        }
    }
    public void RewardPanell()
    {
        if (RewardPanel.activeInHierarchy)
            RewardPanel.SetActive(false);
        else
        {
            RewardPanel.SetActive(true);
            if (PlayerPrefs.GetInt("gameprogress") == 5)
            {
                LevelText.text = "Lesson 2.1 Completed";
                MoneyText.text = "$100 added to Alex’s account";
                PointsText.text = "+10";
            }
            else if (PlayerPrefs.GetInt("gameprogress") == 6)
            {
                LevelText.text = "Lesson 2.2 Completed";
                MoneyText.text = "$100 added to Alex’s account";
                PointsText.text = "+15";
                OtherRewards.gameObject.SetActive(true);
                OtherRewards.text = "+ Insight into banking options";
                


            }
            else if (PlayerPrefs.GetInt("gameprogress") == 7)
            {
                LevelText.text = "Lesson 2.3 Completed";
                MoneyText.text = "$200 added to Alex’s account";
                PointsText.text = "+15";
                OtherRewards.gameObject.SetActive(true);
                OtherRewards.text = "+ Reduced bank fees for Alex’s future transactions";
                
            }
            else if (PlayerPrefs.GetInt("gameprogress") == 8)
            {
                LevelText.text = "Lesson 2.4 Completed";
                MoneyText.text = "$50 as a reward from Jordan";
                PointsText.text = "+15";
                OtherRewards.gameObject.SetActive(true);
                OtherRewards.text = "+ Alex also gains mobile banking access";
            }
            else if (PlayerPrefs.GetInt("gameprogress") == 9)
            {
                LevelText.text = "Lesson 2.5 Completed";
                MoneyText.text = "€100 security bonus for Alex’s account";
                PointsText.text = "+20";
            }
        }
        PlayerPrefs.SetInt("lives", 2);
    }
    public void NextRewardPanelBtn()
    {
        RewardPanell();
        LevelComplete();
        CoinsProgress();
        PointsProgress();
    }
    public void HomeRewardPanelBtn()
    {
        RewardPanell();
        LevelComplete();
        SceneManager.LoadScene(0);
    }
    public void LifeEnd()
    {
        Invoke("LevelSelectorLoader", 2);
    }
    private void LessonScriptsActivation()
    {
        if (PlayerPrefs.GetInt("patel") == 2)
        {
            GetComponent<Lesson2_1>().enabled = true;
            player.localPosition = new Vector3(5, -6.8f, 55);
            player.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (PlayerPrefs.GetInt("gameprogress") == 5)
        {
            GetComponent<Lesson2_1>().enabled = false;
            GetComponent<Lesson2_2>().enabled = true;
            player.localPosition = new Vector3(5, -6.8f, 55);
            player.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (PlayerPrefs.GetInt("gameprogress") == 6)
        {
            GetComponent<Lesson2_1>().enabled = false;
            GetComponent<Lesson2_2>().enabled = false;
            GetComponent<Lesson2_3>().enabled = true;
            thompsonNPC.SetActive(true);
            player.localPosition = new Vector3(106, -7f, 135);
            player.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (PlayerPrefs.GetInt("gameprogress") == 7)
        {
            GetComponent<Lesson2_1>().enabled = false;
            GetComponent<Lesson2_2>().enabled = false;
            GetComponent<Lesson2_3>().enabled = false;
            GetComponent<Lesson2_4>().enabled = true;
            player.localPosition = new Vector3(5, -6.8f, 55);
            player.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (PlayerPrefs.GetInt("gameprogress") == 8)
        {
            GetComponent<Lesson2_1>().enabled = false;
            GetComponent<Lesson2_2>().enabled = false;
            GetComponent<Lesson2_3>().enabled = false;
            GetComponent<Lesson2_4>().enabled = false;
            GetComponent<Lesson2_5>().enabled = true;
            player.localPosition = new Vector3(5, -6.8f, 55);
            player.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
    private void CurrentLevel()
    {
        CurrentLevelText.text = "Task: " + (PlayerPrefs.GetInt("gameprogress") -3).ToString() + "/5";
        CurrentLevelText.transform.parent.GetChild(0).GetComponent<Image>().fillAmount = (PlayerPrefs.GetInt("gameprogress") -3) / 5.0f;
    }
    public void CurrentLevelProgress(int Progress, float ProgressMax)
    {
        CurrentLevelProgressText.transform.parent.GetChild(0).GetComponent<Image>().fillAmount = (Progress / ProgressMax);
    }
    public void CoinsProgress()
    {
        CoinsProgressText.text = "Money: $" + (PlayerPrefs.GetInt("walletmoney")+ PlayerPrefs.GetInt("bankmoney")).ToString() + "/5300";
        CoinsProgressText.transform.parent.GetChild(0).GetComponent<Image>().fillAmount = (PlayerPrefs.GetInt("walletmoney") + PlayerPrefs.GetInt("bankmoney")) / 5300.0f;
    }
    public void PointsProgress()
    {
        PointsProgressText.text = "Points: " + PlayerPrefs.GetInt("walletpoint").ToString() + "/160";
        PointsProgressText.transform.parent.GetChild(0).GetComponent<Image>().fillAmount = PlayerPrefs.GetInt("walletpoint") / 160.0f;
    }
    public void PopUpShow(int good_bad)
    {
      
        if (good_bad == 0)
        {
            PopupBg.transform.GetChild(0).gameObject.SetActive(true);
            PopupBg.transform.GetChild(1).gameObject.SetActive(false);
            PopupBg.transform.GetChild(2).gameObject.SetActive(false);
            PopupBg.transform.GetChild(3).gameObject.SetActive(false);
            PopupBg.GetComponent<Image>().sprite = goodNoti;
            Invoke("confettiPlay", 0.5f);
            //PopupBg.GetComponent<Image>().color = Color.white;
        }
        else if (good_bad == 1)
        {
            PopupBg.transform.GetChild(1).gameObject.SetActive(true);
            PopupBg.transform.GetChild(0).gameObject.SetActive(false);
            PopupBg.transform.GetChild(3).gameObject.SetActive(false);
            PopupBg.transform.GetChild(2).gameObject.SetActive(false);
            PopupBg.GetComponent<Image>().sprite = badNoti;
            // PopupBg.GetComponent<Image>().color = Color.red;
            if (PlayerPrefs.GetInt("warn") == 1)
            {
                if (PlayerPrefs.GetInt("lives") > 1)
                    PlayerPrefs.SetString("LastRewardTime", DateTime.Now.ToString());
                LivesDeduct();
                LivesPrompt.SetActive(true);
                int lives = PlayerPrefs.GetInt("lives") + 1;
                LivesPrompt.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "You have " + lives + " attempt(s) left";

            }
            else
                WarningPanel.SetActive(true);
        }
        else if (good_bad == 2)
        {
            PopupBg.transform.GetChild(2).gameObject.SetActive(true);
            PopupBg.transform.GetChild(0).gameObject.SetActive(false);
            PopupBg.transform.GetChild(1).gameObject.SetActive(false);
            PopupBg.transform.GetChild(3).gameObject.SetActive(false);
            PopupBg.GetComponent<Image>().sprite = goodNoti;
        }
        else if (good_bad == 3)
        {
            PopupBg.transform.GetChild(3).gameObject.SetActive(true);
            PopupBg.transform.GetChild(2).gameObject.SetActive(false);
            PopupBg.transform.GetChild(0).gameObject.SetActive(false);
            PopupBg.transform.GetChild(1).gameObject.SetActive(false);
            PopupBg.GetComponent<Image>().sprite = badNoti;
        }
        else if (good_bad == 4)
        {
            PopupBg.transform.GetChild(1).gameObject.SetActive(true);
            PopupBg.transform.GetChild(0).gameObject.SetActive(false);
            PopupBg.transform.GetChild(3).gameObject.SetActive(false);
            PopupBg.transform.GetChild(2).gameObject.SetActive(false);
            PopupBg.GetComponent<Image>().sprite = badNoti;
        }
        else if (good_bad == 5)
        {
            PopupBg.transform.GetChild(4).gameObject.SetActive(true);
            PopupBg.transform.GetChild(2).gameObject.SetActive(false);
            PopupBg.transform.GetChild(3).gameObject.SetActive(false);
            PopupBg.transform.GetChild(0).gameObject.SetActive(false);
            PopupBg.transform.GetChild(1).gameObject.SetActive(false);
            PopupBg.GetComponent<Image>().sprite = badNoti;
        }
        //if (good_bad != 1)
        //{
            iTween.ScaleTo(PopupBg, iTween.Hash("y", 1f, "time", 1f, "easetype", iTween.EaseType.easeOutElastic, "loopType", "pingPong"));
            Invoke("PopUpHide", 2);
       // }

    }
    private void confettiPlay()
    {
        con1.Play();
        con2.Play();
    }
    public void PopUpHide()
    {
        iTween.ScaleTo(PopupBg, iTween.Hash("y", 0, "time", 1f));
    }
    public void LivesPromptBtn()
    {
        if (PlayerPrefs.GetInt("lives") >= 0)
            LivesPrompt.SetActive(false);
        else
            SceneManager.LoadScene(1);
    }

    public void Warning()
    {
        PlayerPrefs.SetInt("warn", 1);
        WarningPanel.SetActive(false);
    }

    public void BreakOkBtn()
    {
        SceneManager.LoadScene(1);
    }
}