using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Lesson2_2 : MonoBehaviour
{
    public int counter,compareCounter,sidepopCount;
    [SerializeField] GameObject Lesson2__2;
    public GameObject patel;
    string[] compareWords = { "Wide Accessibility", "Lower Interest Rates", "Personalized Service", "Advanced Mobile Banking",
                                               "Higher Fees", "Fewer Locations"};
    [SerializeField] Text CompareText;
    [SerializeField] Transform compareProfiles,sidePopUp, player;

    void Start()
    {
        counter = 0;
        compareCounter = 0;
        sidepopCount = 0;
        GameManagerModule2.instance.AddMoney(0);
        GameManagerModule2.instance.AddPoint(0);
        GameManagerModule2.instance.AddBankMoney(0,0);
        GameManagerModule2.instance.LivesCheck();
        GameManagerModule2.instance.NpcImages[0].SetActive(true);
        GameManagerModule2.instance.NPCsNames.text = "Mr.Patel";
    }
    public void Task2_2NextBtn() // next button in dialog box
    {
        GameManagerModule2.instance.CurrentLevelProgress(counter, 11.0f);
        if (counter == 0 && PlayerPrefs.GetInt("patel") == 1)
        {
            GameManagerModule2.instance.DialogPanel.SetActive(true);
            GameManagerModule2.instance.DialogueBox.SetActive(true);
            GameManagerModule2.instance.DialogueText.text = "Ok Alex, now lets take a tour to financial district.";
            counter++;
        }
        else if (counter == 1)
        {
            GameManagerModule2.instance.Toast.Show("Go to financial district", 2);
            GameManagerModule2.instance.DialogPanel.SetActive(false);
            player.eulerAngles = new Vector3(0, 180, 0);
            patel.transform.eulerAngles = new Vector3(0, 180, 0);
            GetComponent<Lesson2_2>().enabled = false;
            patel.transform.localPosition = new Vector3(45, 2.4f, 90);
            counter++;
        }
        else if (counter == 2)
        {
            GameManagerModule2.instance.DialogPanel.SetActive(true);
            GameManagerModule2.instance.DialogueText.text = "Banks are profit-driven, they're a business that offers a service, you make them the guarantor of your money, and they offer you convenience. On the other hand credit unions are community-owned and often have lower fees.";
            counter++;
        }
        else if (counter == 3)
        {
            GameManagerModule2.instance.DialogueText.text = "They are a public endeavor, to make lives and finances easier, but they also limit spending freedom. Let’s explore both of your options then!";
            counter++;
        }
        else if (counter == 4)
        {
            GameManagerModule2.instance.DialogueText.text = "You have to click on whichever one of the two institutions it belongs to.";
            counter++;
        }
        else if (counter == 5)
        {
            GameManagerModule2.instance.NpcImages[0].SetActive(false);
            GameManagerModule2.instance.DialogueBox.SetActive(false);
            Lesson2__2.SetActive(true);
            CompareText.text = compareWords[compareCounter];
            counter++;
        }
        else if (counter == 6)
        {
            Lesson2__2.transform.GetChild(0).gameObject.SetActive(false);
            GameManagerModule2.instance.DialogueBox.SetActive(true);
            GameManagerModule2.instance.NpcImages[0].SetActive(true);
            if(PlayerPrefs.GetInt("lives")==2)
                GameManagerModule2.instance.DialogueText.text = "Brilliant, you have an excellent understanding of banks and credit unions! You clearly know how to evaluate financial institutions for your needs.";
            else if (PlayerPrefs.GetInt("lives") == 1)
                GameManagerModule2.instance.DialogueText.text = "You did well, Alex, though you had to rethink some answers. It’s great that you learned along the way. Always compare carefully before choosing a financial institution.";
            else if (PlayerPrefs.GetInt("lives") == 0)
                GameManagerModule2.instance.DialogueText.text = "That was a bit tricky, wasn’t it? But now you know the key differences. Keep practicing these comparisons, and soon you’ll make decisions like a financial expert.";
            counter++;
        }
        else if(counter == 7)
        {
            GameManagerModule2.instance.DialogueText.text = "Now Alex, see these two people, they have very different financial needs, don't they, you can see that they want different things from their financial institute.";
            counter++;
        }
        else if (counter == 8)
        {
            GameManagerModule2.instance.DialogueText.text = "What you have to do is, see what they need, and based on that, recommend what would be best for them, a bank, or a credit union, now that you know about both of them.";
            counter++;
        }
        else if (counter == 9)
        {
            GameManagerModule2.instance.DialogueBox.SetActive(false);
            GameManagerModule2.instance.NpcImages[0].SetActive(false);
            Lesson2__2.transform.GetChild(1).gameObject.SetActive(true);
            InvokeRepeating("sidePopup", 1, 2);
            counter++;
        }
        else if (counter == 10)
        {
            Lesson2__2.SetActive(false);
            GameManagerModule2.instance.DialogueBox.SetActive(true);
            GameManagerModule2.instance.NpcImages[0].SetActive(true);
            GameManagerModule2.instance.DialogueText.text = "You see Alex, like I said, different people have different banking needs. The right choice depends on your lifestyle and financial goals, and you've figured that out like a pro, wonderful job, now we can move on to learning more about the accounts themselves.";
            counter++;
        }
        else if (counter == 11)
        {
            GameManagerModule2.instance.DialogueBox.SetActive(false);
            GameManagerModule2.instance.NpcImages[0].SetActive(false);
            PlayerPrefs.SetInt("patel", 4);
            PlayerPrefs.SetInt("gameprogress", 6);
            GameManagerModule2.instance.RewardPanell();
            GameManagerModule2.instance.AddBankMoney(100,1);
            GameManagerModule2.instance.AddPoint(15);

        }
    }
    public void CompareBtn(int bankOrcreditUnion)
    {
        if(bankOrcreditUnion == 0)
        {
            if (compareCounter == 0 || compareCounter == 3 || compareCounter == 4)
            {
                GameManagerModule2.instance.PopUpShow(0);
                compareCounter++;
                CompareText.text = compareWords[compareCounter];
                return;
            }
        }
        else if(bankOrcreditUnion == 1)
        {
            if (compareCounter == 1 || compareCounter == 2)
            {
                GameManagerModule2.instance.PopUpShow(0);
                compareCounter++;
                CompareText.text = compareWords[compareCounter];
                return;
            }
            else if (compareCounter == 5)
            {
                GameManagerModule2.instance.PopUpShow(0);
                Task2_2NextBtn();
                return;
            }
        }
        if (compareProfiles.GetChild(2).gameObject.activeInHierarchy)
        {
            if (bankOrcreditUnion == 3)
            {
                compareProfiles.GetChild(2).gameObject.SetActive(false);
                compareProfiles.GetChild(3).gameObject.SetActive(true);
                GameManagerModule2.instance.PopUpShow(0);
                sidepopCount = 3;
                sidePopUp.GetChild(0).gameObject.SetActive(false);
                sidePopUp.GetChild(1).gameObject.SetActive(false);
                sidePopUp.GetChild(2).gameObject.SetActive(false);
                return;
            }
            else if (bankOrcreditUnion == 4)
            {
                //if (PlayerPrefs.GetInt("lives") > 1)
                //    PlayerPrefs.SetString("LastRewardTime", DateTime.Now.ToString());
                GameManagerModule2.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule2.instance.LifeEnd();
                return;
            }
            return;
        }
        else if (compareProfiles.GetChild(3).gameObject.activeInHierarchy)
        {
            if (bankOrcreditUnion == 4)
            {
                compareProfiles.GetChild(3).gameObject.SetActive(false);
                GameManagerModule2.instance.PopUpShow(0);
                CancelInvoke();
                iTween.ScaleTo(sidePopUp.gameObject, iTween.Hash("x", 0, "time", 1f));
                Task2_2NextBtn();
                return;
            }
            else if (bankOrcreditUnion == 3)
            {
                if (PlayerPrefs.GetInt("lives") > 1)
                    PlayerPrefs.SetString("LastRewardTime", DateTime.Now.ToString());
                GameManagerModule2.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule2.instance.LifeEnd();
                return;
            }
            return;
        }
        if (PlayerPrefs.GetInt("lives") > 1)
            PlayerPrefs.SetString("LastRewardTime", DateTime.Now.ToString());
        GameManagerModule2.instance.PopUpShow(1);
        if (PlayerPrefs.GetInt("lives") <= -1)
            GameManagerModule2.instance.LifeEnd();
        return;

    }
    public void Retry()
    {
        PlayerPrefs.SetInt("patel", 1);
        PlayerPrefs.SetInt("gameprogress", 5);
        SceneManager.LoadScene(3);
    }
    public void sidePopup()
    {
        if (compareProfiles.GetChild(2).gameObject.activeInHierarchy)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i == sidepopCount)
                {
                    sidePopUp.GetChild(i).gameObject.SetActive(true);
                    iTween.ScaleTo(sidePopUp.gameObject, iTween.Hash("x", 1f, "time", 0.98f, "easetype", iTween.EaseType.easeOutElastic, "loopType", "pingPong"));
                }
                else
                    sidePopUp.GetChild(i).gameObject.SetActive(false);
            }
            sidepopCount++;
            if (sidepopCount == 3)
                sidepopCount = 0;
        }
        else if (compareProfiles.GetChild(3).gameObject.activeInHierarchy)
        {
            for (int i = 3; i < 6; i++)
            {
                if (i == sidepopCount)
                {
                    sidePopUp.GetChild(i).gameObject.SetActive(true);
                    iTween.ScaleTo(sidePopUp.gameObject, iTween.Hash("x", 1f, "time", 0.98f, "easetype", iTween.EaseType.easeOutElastic, "loopType", "pingPong"));
                }
                else
                    sidePopUp.GetChild(i).gameObject.SetActive(false);
            }
            sidepopCount++;
            if (sidepopCount == 6)
                sidepopCount = 3;
        }
    }
}