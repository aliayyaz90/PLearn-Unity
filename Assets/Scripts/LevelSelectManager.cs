using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen, LifeButton;
    [SerializeField] Image loadingBarFill;
    [SerializeField] Image blackImageForFade;
    [SerializeField] Button[] LessonButtons;
    [SerializeField] GameObject[] Lives, DottedLineAni;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] RectTransform scrollContent;
    [SerializeField] RectTransform[] Targets;
    [SerializeField] RectTransform[] Maps;
    [SerializeField] Text pointsText;
    [SerializeField] Transform content;
    int dottedCount, dottedCount3; 
    bool camera_move_enabled = false;
    bool fadein = false;
    bool fadeout = false;
    Vector2 targetPos = new Vector2(290,5500);
    Vector2 prevtargetPos = new Vector2(290,4300);
    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (PlayerPrefs.GetInt("gameprogress") >= 1)
        {
            blackImageForFade.gameObject.SetActive(true);
            blackImageForFade.GetComponent<CanvasGroup>().alpha = 1;
            fadeout = true;
            Invoke("BlackScreenOff", 1.5f);     
        }
        //if (PlayerPrefs.GetInt("grandma") == 0)
        //{
        //    PlayerPrefs.SetInt("lives", 2);
        //    PlayerPrefs.SetInt("modules", 0);
        //}
        
        
        pointsText.text = "Points: "+PlayerPrefs.GetInt("walletpoint").ToString();
        if (Lesson1_4.AnimationLevel)
            Invoke("StartContentMove", 2);
        dottedCount = 0;

        if (Lesson3_14.AnimationLevel3)
        {
            Invoke("StartContentMove", 2);
            content.localPosition = prevtargetPos;
        }
        dottedCount3 = 3;

        PlayerPrefs.SetInt("modules", 2);
        if (PlayerPrefs.GetInt("gameprogress") >= 23)
        {
            Debug.Log("i ran");
            PlayerPrefs.SetInt("modules", 3);
        }
        //PlayerPrefs.SetInt("gameprogress", 9);
        PlayerPrefs.SetInt("lives", 2);
    }
    public void BackGame()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadingScene()
    {
        blackImageForFade.gameObject.SetActive(true);
        fadein = true;
        Invoke("LoadWithFade", 2);
    }
    private void Update()
    {
        LivesCheck();
        if (fadein)
        {
            if (blackImageForFade.GetComponent<CanvasGroup>().alpha < 1)
                blackImageForFade.GetComponent<CanvasGroup>().alpha += Time.deltaTime;         
        }
        else if (fadeout)
        {
            if (blackImageForFade.GetComponent<CanvasGroup>().alpha > 0)
                blackImageForFade.GetComponent<CanvasGroup>().alpha -= Time.deltaTime * 0.7f;          
        }
        if (dottedCount < 3 && Lesson1_4.AnimationLevel)
        {
            if (DottedLineAni[dottedCount].activeInHierarchy && DottedLineAni[dottedCount].GetComponent<Image>().fillAmount < 1)
                DottedLineAni[dottedCount].GetComponent<Image>().fillAmount += Time.deltaTime*0.6f;
            if (DottedLineAni[dottedCount].activeInHierarchy && DottedLineAni[dottedCount].GetComponent<Image>().fillAmount == 1)
                dottedCount++;
        }
        if (dottedCount3 < 5 && Lesson3_14.AnimationLevel3)
        {
            if (DottedLineAni[dottedCount3].activeInHierarchy && DottedLineAni[dottedCount3].GetComponent<Image>().fillAmount < 1)
                DottedLineAni[dottedCount3].GetComponent<Image>().fillAmount += Time.deltaTime * 0.6f;
            if (DottedLineAni[dottedCount3].activeInHierarchy && DottedLineAni[dottedCount3].GetComponent<Image>().fillAmount == 1)
                dottedCount3++;
        }

        ProgressChecker();
        if (PlayerPrefs.GetInt("lives") < 2 && PlayerPrefs.GetInt("walletpoint") >= 30)
            LifeButton.SetActive(true);
        if (camera_move_enabled && Lesson3_14.AnimationLevel3)
            content.localPosition= Vector3.Lerp(content.localPosition, targetPos, 0.8f * Time.deltaTime);      
    }
    private void LoadWithFade()
    {   if(PlayerPrefs.GetInt("gameprogress")<4)    
            SceneManager.LoadScene(2);
        else if(PlayerPrefs.GetInt("gameprogress") < 9)
            SceneManager.LoadScene(3);
        else if (PlayerPrefs.GetInt("gameprogress") < 23)
            SceneManager.LoadScene(4);
        else
            SceneManager.LoadScene(5);
    }   
    private void BlackScreenOff()
    {
        blackImageForFade.gameObject.SetActive(false);
    }
    public void ProgressChecker()
    {
        if (PlayerPrefs.GetInt("lives") > -1)
        {
            int progress = PlayerPrefs.GetInt("gameprogress");
            LessonButtons[progress].interactable = true;
            LessonButtons[progress].transform.GetChild(0).gameObject.SetActive(true);
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
    public void onClickScrollTo()
    {
        Vector3 MapPos = Maps[0].localPosition;
        if (PlayerPrefs.GetInt("modules") == 0)
            MapPos = Maps[0].localPosition;
        else if (PlayerPrefs.GetInt("modules") == 1)
            MapPos = Maps[1].localPosition;
        else if (PlayerPrefs.GetInt("modules") == 2)
            MapPos = Maps[2].localPosition;
        else if (PlayerPrefs.GetInt("modules") == 3)
            MapPos = Maps[3].localPosition;
        Canvas.ForceUpdateCanvases();
        Vector2 viewPortLocalPos = scrollRect.transform.GetChild(0).localPosition;
        Vector2 targetLocalPos = Targets[PlayerPrefs.GetInt("gameprogress")].localPosition + MapPos;
        Vector2 newTargetLocalPos = new Vector2(0 - (targetLocalPos.x), 0 - (targetLocalPos.y));
        scrollContent.localPosition = newTargetLocalPos;
    }
    public void GetALife()
    {
        int life= PlayerPrefs.GetInt("lives");
        PlayerPrefs.SetInt("lives", ++life);
        int points= PlayerPrefs.GetInt("walletpoint");
        points -= 30;
        PlayerPrefs.SetInt("walletpoint",points);
        pointsText.text = "Points: " + PlayerPrefs.GetInt("walletpoint").ToString();
        LifeButton.SetActive(false);
    }
    private void StartContentMove()
    {
        camera_move_enabled = true;
    }
}