using System;
using ToastWrapper;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] public Text InteractionText, CurrentLevelText, CurrentLevelProgressText, CoinsProgressText, PointsProgressText, DialogueText, fps, MoneyText, PointsText, LevelText;
    [SerializeField] public GameObject InteractionHolder, DialogPanel, wallet, RewardPanel, PopupBg, DialogueBox, LivesPrompt, WarningPanel;
    [Header("Others")]
    public Toast Toast;
    [SerializeField] public Slider slider1,slider2;
    [SerializeField] public Button nextButton;
    [SerializeField] public Image blackImageForFade;
    public bool fadein = false;
    public int walletMoney;
    public GameObject[] Lives, NpcImages;
    public Transform player, Arrow, NpcGrandma, NpcGreen, NpcEmma;
    [SerializeField] ParticleSystem con1, con2;

    private float polling = 1f, time;
    private int frameCount;
    //public static string toastMsg="";
    private void Awake()
    {
        if(instance == null)
            instance = this;
        Invoke("blackScreenOff", 1f);
        LessonScriptsActivation();
        CurrentLevel();
        CoinsProgress();
        PointsProgress();
    }
    private void Start()
    {
        Application.targetFrameRate = 60;
    }
    private void Update()
    {
        if (blackImageForFade.GetComponent<CanvasGroup>().alpha  >0)
            blackImageForFade.GetComponent<CanvasGroup>().alpha -= Time.deltaTime*0.8f;        
        if (fadein)
        {
            if (blackImageForFade.GetComponent<CanvasGroup>().alpha < 1)
                blackImageForFade.GetComponent<CanvasGroup>().alpha += Time.deltaTime*1.2f;        
        }
        ArrowDirection();

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
    private void ArrowDirection()
    {
       // Vector3 arrowRot = new Vector3(0, 0, 0);
        //Debug.Log(Arrow.position.x + " x      " + Arrow.position.z + " z");
        if (GetComponent<Lesson1_1>().enabled)
            Arrow.LookAt(NpcGrandma, Vector3.up);
        else if (GetComponent<Lesson1_2>().enabled)
            Arrow.LookAt(NpcGreen, Vector3.up);
        else if (GetComponent<Lesson1_3>().enabled && PlayerPrefs.GetInt("grandma") != 3)
            Arrow.LookAt(NpcGrandma, Vector3.up);
        else if (GetComponent<Lesson1_3>().enabled && (PlayerPrefs.GetInt("grandma") == 3 && PlayerPrefs.GetInt("emma") != 1) && GetComponent<Lesson1_3>().counterr >= 2)
            Arrow.LookAt(NpcEmma, Vector3.up);
        else if (GetComponent<Lesson1_4>().enabled && PlayerPrefs.GetInt("grandma") != 5)
            Arrow.LookAt(NpcGrandma, Vector3.up);

    }
    private void blackScreenOff()
    {
        blackImageForFade.gameObject.SetActive(false);
    }
    public void MainMenu()
    {
        if (GetComponent<Lesson1_1>().enabled)
            GetComponent<Lesson1_1>().Retry();
        else if (GetComponent<Lesson1_2>().enabled)
            GetComponent<Lesson1_2>().Retry();
        else if (GetComponent<Lesson1_3>().enabled)
            GetComponent<Lesson1_3>().Retry();
        else if (GetComponent<Lesson1_4>().enabled)
            GetComponent<Lesson1_4>().Retry();
        SceneManager.LoadScene(1);
    }
    public void OnClickYesBtn()
    {
        InteractionHolder.SetActive(false);
        NextBtn();
        if(GetComponent<Lesson1_2>().enabled && PlayerPrefs.GetInt("mrgreen")!=2)
            Toast.Show("Go to Mr.Green", 3);
        else if (GetComponent<Lesson1_3>().enabled && PlayerPrefs.GetInt("grandma") != 3)
            Toast.Show("Go to Grandma Millie", 3);
        else if (GetComponent<Lesson1_3>().enabled && ( PlayerPrefs.GetInt("grandma") == 3 && PlayerPrefs.GetInt("emma") != 1 )&& GetComponent<Lesson1_3>().counterr >=2)
            Toast.Show("Go to Emma Baker", 3);
        else if (GetComponent<Lesson1_4>().enabled && PlayerPrefs.GetInt("grandma") != 5)
            Toast.Show("Go to Grandma Millie", 3);

        PlayerPrefs.SetInt("warn", 0);
    }
    public void OnClickNoBtn()
    {
        InteractionHolder.SetActive(false);
    }
    public void NextBtn()
    {
        if (GetComponent<Lesson1_1>().enabled)//SceneManager.GetActiveScene().buildIndex == 2
            GetComponent<Lesson1_1>().Task1NextBtn();
        else if (GetComponent<Lesson1_2>().enabled)//SceneManager.GetActiveScene().buildIndex == 3
            GetComponent<Lesson1_2>().Task2NextBtn();
        else if (GetComponent<Lesson1_3>().enabled)//SceneManager.GetActiveScene().buildIndex == 4
            GetComponent<Lesson1_3>().Task3NextBtn();
        else if (GetComponent<Lesson1_4>().enabled)//SceneManager.GetActiveScene().buildIndex == 5
            GetComponent<Lesson1_4>().Task4NextBtn();
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
        if (PlayerPrefs.GetInt("gameprogress") == 4 || PlayerPrefs.GetInt("lives")==-1)
            SceneManager.LoadScene(1);
        else
            SceneManager.LoadScene(2);
    }
    public void LivesDeduct()
    {
        if(PlayerPrefs.GetInt("warn") == 1)
        {
            if (PlayerPrefs.GetInt("lives") > 1)
                PlayerPrefs.SetString("LastRewardTime", DateTime.Now.ToString());
            int ActiveChecker = PlayerPrefs.GetInt("lives");
            if (ActiveChecker >= 0)
            {
                Lives[ActiveChecker].gameObject.SetActive(false);
                PlayerPrefs.SetInt("lives", --ActiveChecker);
            }
            
        }
        else
        {
            WarningPanel.SetActive(true);
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
                Lives [i].gameObject.SetActive(true);           
            else if (i > ActiveChecker)
            {              
                Lives[i].gameObject.SetActive(false);
            }
        }
    }
    public void RewardPanell()
    {
        if (RewardPanel.activeInHierarchy)
            RewardPanel.SetActive(false);
        else
        {
            RewardPanel.SetActive(true);
            if(PlayerPrefs.GetInt("gameprogress") == 1)
            {
                LevelText.text = "Lesson 1.1 Completed";
                MoneyText.text = "+$"+ PlayerPrefs.GetInt("walletmoney").ToString();
                PointsText.text = "+20";
                if (PlayerPrefs.GetInt("walletmoney") >= 250)
                {
                    RewardPanel.transform.GetChild(1).gameObject.SetActive(true);
                    float d = PlayerPrefs.GetInt("walletmoney");
                    d *= 0.5f;
                    RemoveMoney((Int32)d + 60);
                    MoneyText.text = "+$"+ PlayerPrefs.GetInt("walletmoney").ToString();                   
                }
            }
            else if (PlayerPrefs.GetInt("gameprogress") == 2)
            {
                LevelText.text = "Lesson 1.2 Completed";
                MoneyText.text = "+$" + PlayerPrefs.GetInt("walletmoney").ToString();
                PointsText.text = "+20";
                if (PlayerPrefs.GetInt("mrgreenmoney") >= 250)
                {
                    RewardPanel.transform.GetChild(1).gameObject.SetActive(true);
                    float d = PlayerPrefs.GetInt("mrgreenmoney");                   
                    d *= 0.5f;
                    RemoveMoney((Int32)d + 100);
                    MoneyText.text = "+$" + ((Int32)d).ToString();
                }
            }
            else if (PlayerPrefs.GetInt("gameprogress") == 3)
            {
                LevelText.text = "Lesson 1.3 Completed";
                MoneyText.text = "+$520";
                PointsText.text = "+20";
            }
            else if (PlayerPrefs.GetInt("gameprogress") == 4)
            {
                LevelText.text = "Lesson 1.4 Completed";
                MoneyText.text = "+$450";
                PointsText.text = "+25";
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
        if (PlayerPrefs.GetInt("gameprogress") == 1)
        {
            GetComponent<Lesson1_1>().enabled = false;
            GetComponent<Lesson1_2>().enabled = true;
            player.localPosition = new Vector3(2, -6.8f, 56);
            player.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (PlayerPrefs.GetInt("gameprogress") == 2)
        {
            GetComponent<Lesson1_1>().enabled = false;
            GetComponent<Lesson1_2>().enabled = false;
            GetComponent<Lesson1_3>().enabled = true;
            player.localPosition = new Vector3(54, -6.8f, 109);
            player.localRotation = Quaternion.Euler(0, -90, 0);
        }
        else if (PlayerPrefs.GetInt("gameprogress") == 3)
        {
            GetComponent<Lesson1_1>().enabled = false;
            GetComponent<Lesson1_2>().enabled = false;
            GetComponent<Lesson1_3>().enabled = false;
            GetComponent<Lesson1_4>().enabled = true;
            player.localPosition = new Vector3(168, -9.3f, 194);
            player.localRotation = Quaternion.Euler(0, -90, 0);
        }
    }
    private void CurrentLevel()
    {
        CurrentLevelText.text = "Task: " + (PlayerPrefs.GetInt("gameprogress") + 1).ToString() + "/4";
        CurrentLevelText.transform.parent.GetChild(0).GetComponent<Image>().fillAmount = (PlayerPrefs.GetInt("gameprogress") + 1) / 4.0f;
    }
    public void CurrentLevelProgress(int Progress,float ProgressMax)
    {       
        CurrentLevelProgressText.transform.parent.GetChild(0).GetComponent<Image>().fillAmount = (Progress/ProgressMax);
    }
    public void CoinsProgress()
    {
        CoinsProgressText.text = "Money:" + PlayerPrefs.GetInt("walletmoney").ToString() + "/1300";
        CoinsProgressText.transform.parent.GetChild(0).GetComponent<Image>().fillAmount = PlayerPrefs.GetInt("walletmoney") / 1300.0f;
    }
    public void PointsProgress()
    {
        PointsProgressText.text = "Points: " + PlayerPrefs.GetInt("walletpoint").ToString() + "/70";
        PointsProgressText.transform.parent.GetChild(0).GetComponent<Image>().fillAmount = PlayerPrefs.GetInt("walletpoint") / 70.0f;
    }    
    public void PopUpShow(int good_bad)
    {
        if(good_bad == 0)
        {
            PopupBg.transform.GetChild(0).gameObject.SetActive(true);
            PopupBg.transform.GetChild(1).gameObject.SetActive(false);
            Invoke("confettiPlay", 0.5f);
            
         
           // PopupBg.GetComponent<Image>().color = Color.green;
        }
        else if(good_bad == 1)
        {
            //PopupBg.transform.GetChild(1).gameObject.SetActive(true);
            //PopupBg.transform.GetChild(0).gameObject.SetActive(false);
            //PopupBg.GetComponent<Image>().color = Color.red;
            //if (PlayerPrefs.GetInt("lives") > 1)
            //    PlayerPrefs.SetString("LastRewardTime", DateTime.Now.ToString());
            LivesDeduct();
            if (!WarningPanel.activeInHierarchy)
            {
                LivesPrompt.SetActive(true);
                int lives = PlayerPrefs.GetInt("lives") + 1;
                LivesPrompt.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "You have " + lives + " attempt(s) left";
            }

        }
        if (good_bad != 1)
        {
            iTween.ScaleTo(PopupBg, iTween.Hash("y", 1f, "time", 1f, "easetype", iTween.EaseType.easeOutElastic, "loopType", "pingPong"));
            Invoke("PopUpHide", 2);
        } 
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
}