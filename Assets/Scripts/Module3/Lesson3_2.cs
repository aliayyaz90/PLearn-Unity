using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lesson3_2 : MonoBehaviour
{
    int counter;
    [SerializeField] GameObject Lesson3__2, Task1, Task2, Task3;
    [SerializeField] GameObject[] Items, task2Credits, task3Items;
    [SerializeField] Text task2HeadingText;
    int task2counter, defVal, defVal1;

    // Start is called before the first frame update
    void Start()
    {
        defVal = 0;
        defVal1 = 0;
        task2counter = 0;
        counter = 0;
        GameManagerModule3.instance.AddMoney(0);
        GameManagerModule3.instance.AddPoint(0);
        GameManagerModule3.instance.AddBankMoney(0, 0);
        GameManagerModule3.instance.LivesCheck();
        GameManagerModule3.instance.NPCsNames.text = "Sofia";
        GameManagerModule3.instance.NpcImages[2].SetActive(true);


    }
    public void Task3_2NextBtn() // next button in dialog box
    {
        GameManagerModule3.instance.CurrentLevelProgress(counter, 7.0f);
        if (counter == 0 && PlayerPrefs.GetInt("sofia") == 1)
        {
            GameManagerModule3.instance.DialogPanel.SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "Alex, I don't understand what I should do. I can get a loan, but I can also get a credit card, while I guess are smaller loans? I don't know which is better, can you help me?";
            counter++;
        }
        else if (counter == 1)
        {
            Lesson3__2.SetActive(true);
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            counter++;
        }
        else if (counter == 2)
        {
            if (Items[0].activeInHierarchy && Items[0].transform.localPosition==new Vector3(-220, -302, 0))
            {
                GameManagerModule3.instance.PopUpShow(0);
                Items[0].SetActive(false);
                return;
            }
            else if (Items[1].activeInHierarchy && Items[1].transform.localPosition == new Vector3(220, -302, 0))
            {
                GameManagerModule3.instance.PopUpShow(0);
                Items[1].SetActive(false);
                return;
            }
            else if (Items[2].activeInHierarchy && Items[2].transform.localPosition == new Vector3(-220, -302, 0))
            {
                GameManagerModule3.instance.PopUpShow(0);
                Items[2].SetActive(false);
                return;
            }
            else if (Items[3].activeInHierarchy && Items[3].transform.localPosition == new Vector3(220, -302, 0))
            {
                GameManagerModule3.instance.PopUpShow(0);
                Items[3].SetActive(false);
                counter++;
                Task1.SetActive(false);
                GameManagerModule3.instance.Toast.Show("+10 Points\n+50 Money", 2);
                GameManagerModule3.instance.AddPoint(10);
                GameManagerModule3.instance.AddBankMoney(50,1);
                Task3_2NextBtn();            }
            else
            {
                GameManagerModule3.instance.PopUpShow(1);
                    if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule3.instance.LifeEnd();
            }

        }
        else if (counter == 3)
        {
            //GameManagerModule3.instance.DialogueBox.SetActive(true);
            //GameManagerModule3.instance.NpcImages[2].SetActive(true);
            Invoke("DialogueShowWithWait", 2);
            GameManagerModule3.instance.NPCsNames.text = "Sofia";
            GameManagerModule3.instance.DialogueText.text = "None of this makes sense, I need to get all this done and there are so many options, how to I pick what will be best for what, please help me, Alex.";
            counter++;
        }
        else if (counter == 4)
        {
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            Task2.SetActive(true);

            if (task2counter == 0 && task2Credits[0].transform.localPosition==new Vector3(0, -257, 0))
            {
                task2counter = 1;
                task2HeadingText.text = "College fund";
                task2Credits[0].transform.localPosition = new Vector3(-150, -540, 0);
                task2Credits[1].transform.localPosition = new Vector3(150, -540, 0);
                GameManagerModule3.instance.PopUpShow(0);
                return;
            }
            else if(task2counter == 1 && task2Credits[0].transform.localPosition == new Vector3(0, -257, 0))
            {
                task2counter = 2; 
                task2HeadingText.text = "Purchasing a Car";
                task2Credits[0].transform.localPosition = new Vector3(-150, -540, 0);
                task2Credits[1].transform.localPosition = new Vector3(150, -540, 0);
                GameManagerModule3.instance.PopUpShow(0);
                return;
            }
            else if (task2counter == 2 && task2Credits[1].transform.localPosition == new Vector3(0, -257, 0))
            {
                GameManagerModule3.instance.PopUpShow(0);
                task2counter = 2;
                Task2.SetActive(false);
                counter++;
                Task3_2NextBtn();
                GameManagerModule3.instance.Toast.Show("+15 Points\n+100 Money", 2);
                GameManagerModule3.instance.AddPoint(15);
                GameManagerModule3.instance.AddBankMoney(100, 1);
            }
            else if(defVal>=1)
            {
                GameManagerModule3.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule3.instance.LifeEnd();
            }
            defVal++;
        }
        else if (counter == 5)
        {
            //GameManagerModule3.instance.DialogueBox.SetActive(true);
            //GameManagerModule3.instance.NpcImages[2].SetActive(true);
            Invoke("DialogueShowWithWait", 2);
            GameManagerModule3.instance.NPCsNames.text = "Sofia";
            GameManagerModule3.instance.DialogueText.text = "Alex, I have seemed to have picked up so many loans to pay, I need a way to manage them, you know keep track of what I have to pay. Can you help?";
            counter++;
        }
        else if (counter == 6)
        {
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            Task3.SetActive(true);
            if ((task3Items[0].transform.localPosition==new Vector3(-220,-207,0) ||
                task3Items[0].transform.localPosition == new Vector3(-320, -322, 0) ||
                task3Items[0].transform.localPosition == new Vector3(-120, -322, 0)) &&
                (task3Items[1].transform.localPosition == new Vector3(220, -207, 0) ||
                task3Items[1].transform.localPosition == new Vector3(320, -322, 0) ||
                task3Items[1].transform.localPosition == new Vector3(120, -322, 0)) &&
                (task3Items[2].transform.localPosition == new Vector3(-220, -207, 0) ||
                task3Items[2].transform.localPosition == new Vector3(-320, -322, 0) ||
                task3Items[2].transform.localPosition == new Vector3(-120, -322, 0)) &&
                (task3Items[3].transform.localPosition == new Vector3(-220, -207, 0) ||
                task3Items[3].transform.localPosition == new Vector3(-320, -322, 0) ||
                task3Items[3].transform.localPosition == new Vector3(-120, -322, 0)) &&
                (task3Items[4].transform.localPosition == new Vector3(220, -207, 0) ||
                task3Items[4].transform.localPosition == new Vector3(320, -322, 0) ||
                task3Items[4].transform.localPosition == new Vector3(120, -322, 0))&&
                (task3Items[5].transform.localPosition == new Vector3(220, -207, 0) ||
                task3Items[5].transform.localPosition == new Vector3(320, -322, 0) ||
                task3Items[5].transform.localPosition == new Vector3(120, -322, 0)))
            {
                GameManagerModule3.instance.PopUpShow(0);
                counter++;
            }
            else if(defVal1>=1)
            {
                GameManagerModule3.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule3.instance.LifeEnd();
            }
            defVal1++;
        }
        else if (counter == 7)
        {
            Task3.SetActive(false);
            PlayerPrefs.SetInt("sofia", 3);
            PlayerPrefs.SetInt("gameprogress", 11);
            GameManagerModule3.instance.RewardPanell();
            GameManagerModule3.instance.AddBankMoney(150, 1);
            GameManagerModule3.instance.AddPoint(10);
        }
    }
    public void DialogueShowWithWait()
    {
        GameManagerModule3.instance.NpcImages[2].SetActive(true);
        GameManagerModule3.instance.DialogueBox.SetActive(true);
    }
    public void Retry()
    {
        PlayerPrefs.SetInt("sofia", 1);
        PlayerPrefs.SetInt("gameprogress", 10);
        SceneManager.LoadScene(4);
    }
}