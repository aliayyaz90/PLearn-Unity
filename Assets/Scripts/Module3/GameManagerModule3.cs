using ToastWrapper;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManagerModule3 : MonoBehaviour
{
    public static GameManagerModule3 instance;
    [SerializeField] public Text InteractionText, CurrentLevelText, CurrentLevelProgressText, CoinsProgressText, PointsProgressText, DialogueText, OtherRewards, NPCsNames, fps, MoneyText, PointsText, LevelText;
    [SerializeField] public GameObject InteractionHolder, DialogPanel, wallet, RewardPanel, PopupBg, DialogueBox, LivesPrompt, BankmImg, WarningPanel, BreakPanel, thompsonNPC, alexGO, PatelGO5;
    [Header("Others")]
    public Toast Toast;
    [SerializeField] public Button nextButton;
    [SerializeField] public Image blackImageForFade;
    [SerializeField] Sprite goodNoti, badNoti;
    public bool fadein = false;
    public int walletMoney;
    public GameObject[] Lives, NpcImages;
    public Transform player;
    [SerializeField] ParticleSystem con1, con2;


    private float polling = 1f, time;
    private int frameCount;
    //public static string toastMsg="";
    private void Awake()
    {
        if (instance == null)
            instance = this;
        Invoke("blackScreenOff", 1f);

        CurrentLevel();
        CoinsProgress();
        PointsProgress();
        //if (PlayerPrefs.GetInt("patel") == 0)
        //    GetComponent<Narratives>().enabled = true;


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
        if (GetComponent<Lesson3_1>().enabled)
            GetComponent<Lesson3_1>().Retry();
        else if (GetComponent<Lesson3_2>().enabled)
            GetComponent<Lesson3_2>().Retry();
        else if (GetComponent<Lesson3_3>().enabled)
            GetComponent<Lesson3_3>().Retry();
        else if (GetComponent<Lesson3_4>().enabled)
            GetComponent<Lesson3_4>().Retry();
        else if (GetComponent<Lesson3_5>().enabled)
            GetComponent<Lesson3_5>().Retry();
        else if (GetComponent<Lesson3_6>().enabled)
            GetComponent<Lesson3_6>().Retry();
        else if (GetComponent<Lesson3_7>().enabled)
            GetComponent<Lesson3_7>().Retry();
        else if (GetComponent<Lesson3_8>().enabled)
            GetComponent<Lesson3_8>().Retry();
        else if (GetComponent<Lesson3_9>().enabled)
            GetComponent<Lesson3_9>().Retry();
        else if (GetComponent<Lesson3_10>().enabled)
            GetComponent<Lesson3_10>().Retry();
        else if (GetComponent<Lesson3_11>().enabled)
            GetComponent<Lesson3_11>().Retry();
        else if (GetComponent<Lesson3_12>().enabled)
            GetComponent<Lesson3_12>().Retry();
        else if (GetComponent<Lesson3_13>().enabled)
            GetComponent<Lesson3_13>().Retry();
        else if (GetComponent<Lesson3_14>().enabled)
            GetComponent<Lesson3_14>().Retry();
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
        if (GetComponent<Lesson3_1>().enabled)
            GetComponent<Lesson3_1>().Task3_1NextBtn();
        else if (GetComponent<Lesson3_2>().enabled)
            GetComponent<Lesson3_2>().Task3_2NextBtn();
        else if (GetComponent<Lesson3_3>().enabled)
            GetComponent<Lesson3_3>().Task3_3NextBtn();
        else if (GetComponent<Lesson3_4>().enabled)
            GetComponent<Lesson3_4>().Task3_4NextBtn();
        else if (GetComponent<Lesson3_5>().enabled)
            GetComponent<Lesson3_5>().Task3_5NextBtn();
        else if (GetComponent<Lesson3_6>().enabled)
            GetComponent<Lesson3_6>().Task3_6NextBtn();
        else if (GetComponent<Lesson3_7>().enabled)
            GetComponent<Lesson3_7>().Task3_7NextBtn();
        else if (GetComponent<Lesson3_8>().enabled)
            GetComponent<Lesson3_8>().Task3_8NextBtn();
        else if (GetComponent<Lesson3_9>().enabled)
            GetComponent<Lesson3_9>().Task3_9NextBtn();
        else if (GetComponent<Lesson3_10>().enabled)
            GetComponent<Lesson3_10>().Task3_10NextBtn();
        else if (GetComponent<Lesson3_11>().enabled)
            GetComponent<Lesson3_11>().Task3_11NextBtn();
        else if (GetComponent<Lesson3_12>().enabled)
            GetComponent<Lesson3_12>().Task3_12NextBtn();
        else if (GetComponent<Lesson3_13>().enabled)
            GetComponent<Lesson3_13>().Task3_13NextBtn();
        else if (GetComponent<Lesson3_14>().enabled)
            GetComponent<Lesson3_14>().Task3_14NextBtn();
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
        if (PlayerPrefs.GetInt("gameprogress") == 23 || PlayerPrefs.GetInt("lives") == -1)
            SceneManager.LoadScene(1);
        else
            SceneManager.LoadScene(4);
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
            if (PlayerPrefs.GetInt("gameprogress") == 10)
            {
                LevelText.text = "Lesson 3.1 Completed";
                MoneyText.text = "$100 added to Alex’s account";
                PointsText.text = "+10";
                OtherRewards.gameObject.SetActive(true);
                OtherRewards.text = "+ Credit Rookie";            
            }
            else if (PlayerPrefs.GetInt("gameprogress") == 11)
            {
                LevelText.text = "Lesson 3.2 Completed";
                MoneyText.text = "$150 added to Alex’s account";
                PointsText.text = "+10";
                OtherRewards.gameObject.SetActive(true);
                OtherRewards.text = "+ Credit Balancer";



            }
            else if (PlayerPrefs.GetInt("gameprogress") == 12)
            {
                LevelText.text = "Lesson 3.3 Completed";
                MoneyText.text = "$150 added to Alex’s account";
                PointsText.text = "+10";
                OtherRewards.gameObject.SetActive(true);
                OtherRewards.text = "+ Smart Spender";

            }
            else if (PlayerPrefs.GetInt("gameprogress") == 13)
            {
                LevelText.text = "Lesson 3.4 Completed";
                MoneyText.text = "$50 as a reward from Sofia";
                PointsText.text = "+15";
            }
            else if (PlayerPrefs.GetInt("gameprogress") == 14)
            {
                LevelText.text = "Lesson 3.5 Completed";
                MoneyText.text = "$200 added to Alex’s account";
                PointsText.text = "+20";
            }
            else if (PlayerPrefs.GetInt("gameprogress") == 15)
            {
                LevelText.text = "Lesson 3.6 Completed";
                MoneyText.text = "$100 added to Alex’s account";
                PointsText.text = "+15";
                OtherRewards.gameObject.SetActive(true);
                OtherRewards.text = "+ Fine Print Pro";
            }
            else if (PlayerPrefs.GetInt("gameprogress") == 16)
            {
                LevelText.text = "Lesson 3.7 Completed";
                //MoneyText.text = "$100 added to Alex’s account";
                PointsText.text = "+20";
                MoneyText.text = "$100 added to Alex’s account";
                OtherRewards.gameObject.SetActive(true);
                OtherRewards.text = "+ Credit Watcher";
            }
            else if (PlayerPrefs.GetInt("gameprogress") == 17)
            {
                LevelText.text = "Lesson 3.8 Completed";
                MoneyText.text = "$100 added to Alex’s account";
                PointsText.text = "+15";
            }
            else if (PlayerPrefs.GetInt("gameprogress") == 18)
            {
                LevelText.text = "Lesson 3.9 Completed";
                MoneyText.text = "$150 added to Alex’s account";
                PointsText.text = "+10";
                OtherRewards.gameObject.SetActive(true);
                OtherRewards.text = "+ Loan Strategist";
            }
            else if (PlayerPrefs.GetInt("gameprogress") == 19)
            {
                LevelText.text = "Lesson 3.10 Completed";
                MoneyText.text = "$100 added to Alex’s account";
                PointsText.text = "+20";
                OtherRewards.gameObject.SetActive(true);
                OtherRewards.text = "+ Interest Aware";
            }
            else if (PlayerPrefs.GetInt("gameprogress") == 20)
            {
                LevelText.text = "Lesson 3.11 Completed";
                MoneyText.text = "$100 added to Alex’s account";
                PointsText.text = "+25";
                OtherRewards.gameObject.SetActive(true);
                OtherRewards.text = "+ Lifesaver";
            }
            else if (PlayerPrefs.GetInt("gameprogress") == 21)
            {
                LevelText.text = "Lesson 3.12 Completed";
                MoneyText.text = "$100 added to Alex’s account";
                PointsText.text = "+15";
            }
            else if (PlayerPrefs.GetInt("gameprogress") == 22)
            {
                LevelText.text = "Lesson 3.13 Completed";
                MoneyText.text = "$100 added to Alex’s account";
                PointsText.text = "+15";
            }
            else if (PlayerPrefs.GetInt("gameprogress") == 23)
            {
                LevelText.text = "Lesson 3.14 Completed";
                MoneyText.text = "$100 added to Alex’s account";
                PointsText.text = "+15";
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
        player.localPosition = new Vector3(5, -6.8f, 55);
        player.localRotation = Quaternion.Euler(0, 0, 0);
        if (PlayerPrefs.GetInt("sofia") == 2 || PlayerPrefs.GetInt("sofia") == 0)
            GetComponent<Lesson3_1>().enabled = true;    
        else if (PlayerPrefs.GetInt("gameprogress") == 10)
            GetComponent<Lesson3_2>().enabled = true;
        else if (PlayerPrefs.GetInt("gameprogress") == 11)
            GetComponent<Lesson3_3>().enabled = true;
        else if (PlayerPrefs.GetInt("gameprogress") == 12)
            GetComponent<Lesson3_4>().enabled = true;
        else if (PlayerPrefs.GetInt("gameprogress") == 13)
            GetComponent<Lesson3_5>().enabled = true;
        else if (PlayerPrefs.GetInt("gameprogress") == 14)
            GetComponent<Lesson3_6>().enabled = true;
        else if (PlayerPrefs.GetInt("gameprogress") == 15)
            GetComponent<Lesson3_7>().enabled = true;
        else if (PlayerPrefs.GetInt("gameprogress") == 16)
            GetComponent<Lesson3_8>().enabled = true;
        else if (PlayerPrefs.GetInt("gameprogress") == 17)
            GetComponent<Lesson3_9>().enabled = true;
        else if (PlayerPrefs.GetInt("gameprogress") == 18)
            GetComponent<Lesson3_10>().enabled = true;
        else if (PlayerPrefs.GetInt("gameprogress") == 19)
            GetComponent<Lesson3_11>().enabled = true;
        else if (PlayerPrefs.GetInt("gameprogress") == 20)
            GetComponent<Lesson3_12>().enabled = true;
        else if (PlayerPrefs.GetInt("gameprogress") == 21)
            GetComponent<Lesson3_13>().enabled = true;
        else if (PlayerPrefs.GetInt("gameprogress") == 22)
            GetComponent<Lesson3_14>().enabled = true;
    }
    private void CurrentLevel()
    {
        CurrentLevelText.text = "Task: " + (PlayerPrefs.GetInt("gameprogress") - 8).ToString() + "/14";
        CurrentLevelText.transform.parent.GetChild(0).GetComponent<Image>().fillAmount = (PlayerPrefs.GetInt("gameprogress") - 8) / 14.0f;
    }
    public void CurrentLevelProgress(int Progress, float ProgressMax)
    {
        CurrentLevelProgressText.transform.parent.GetChild(0).GetComponent<Image>().fillAmount = (Progress / ProgressMax);
    }
    public void CoinsProgress()
    {
        CoinsProgressText.text = "Money: $" + (PlayerPrefs.GetInt("walletmoney") + PlayerPrefs.GetInt("bankmoney")).ToString() + "/5300";
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
