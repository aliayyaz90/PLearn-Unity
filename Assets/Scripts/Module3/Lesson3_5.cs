using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lesson3_5 : MonoBehaviour
{
    int counter;
    [SerializeField] GameObject Lesson3__5, Task1, Task2;
    [SerializeField] Text CreditMeterText;
    [SerializeField] Toggle[] toggle;
    [SerializeField] Button[] task2MCQ;
    public Slider[] sliders;
    int s1Amount,s2Amount, s3Amount, s4Amount, s5Amount, task2mcqCount;

    // Start is called before the first frame update
    void Start()
    {
        task2mcqCount = 0;
        counter = 0;
        GameManagerModule3.instance.AddMoney(0);
        GameManagerModule3.instance.AddPoint(0);
        GameManagerModule3.instance.AddBankMoney(0, 0);
        GameManagerModule3.instance.LivesCheck();
  


    }

    public void Update()
    {
        if (sliders[0].value % 10 != 0 || sliders[1].value % 10 != 0 || sliders[2].value % 10 != 0 || sliders[3].value % 10 != 0 || sliders[4].value % 10 != 0)
        {
            sliders[0].value /= 10;
            sliders[0].value *= 10;
            sliders[1].value /= 10;
            sliders[1].value *= 10;
            sliders[2].value /= 10;
            sliders[2].value *= 10;
            sliders[3].value /= 10;
            sliders[3].value *= 10;
            sliders[4].value /= 10;
            sliders[4].value *= 10;
            sliders[0].transform.GetChild(4).GetComponent<Text>().text = sliders[0].value.ToString() + "%";
            sliders[1].transform.GetChild(4).GetComponent<Text>().text = sliders[1].value.ToString() + "%";
            sliders[2].transform.GetChild(4).GetComponent<Text>().text = sliders[2].value.ToString() + "%";
            sliders[3].transform.GetChild(4).GetComponent<Text>().text = sliders[3].value.ToString() + "%";
            sliders[4].transform.GetChild(4).GetComponent<Text>().text = sliders[4].value.ToString() + "%";
        }
        if (sliders[0].value + sliders[1].value + sliders[2].value + sliders[3].value + sliders[4].value > 100)
        {
            if (sliders[0].value > 40)
                sliders[0].value -= 10;
            else if (sliders[1].value > 20)
                sliders[1].value -= 10;
            else if (sliders[2].value > 20)
                sliders[2].value -= 10;
            else if (sliders[3].value > 10)
                sliders[3].value -= 10;
            else if (sliders[4].value > 10)
                sliders[4].value -= 10;
            sliders[0].transform.GetChild(4).GetComponent<Text>().text = sliders[0].value.ToString() + "%";
            sliders[1].transform.GetChild(4).GetComponent<Text>().text = sliders[1].value.ToString() + "%";
            sliders[2].transform.GetChild(4).GetComponent<Text>().text = sliders[2].value.ToString() + "%";
            sliders[3].transform.GetChild(4).GetComponent<Text>().text = sliders[3].value.ToString() + "%";
            sliders[4].transform.GetChild(4).GetComponent<Text>().text = sliders[4].value.ToString() + "%";
        }
        if (sliders[0].value == 40)
            s1Amount = 170;
        else
            s1Amount = 50;
        if (sliders[1].value == 20)
            s2Amount = 160;
        else
            s2Amount = 70;
        if (sliders[2].value == 20)
            s3Amount = 160;
        else
            s3Amount = 30;
        if (sliders[3].value == 10)
            s4Amount = 160;
        else
            s4Amount = 100;
        if (sliders[4].value == 10)
            s5Amount = 160;
        else
            s5Amount = 80;

        CreditMeterText.text= (s1Amount+s2Amount+ s3Amount+ s4Amount+ s5Amount).ToString();
    }
    public void Task3_5NextBtn() // next button in dialog box
    {
       // Debug.Log(counter);
        GameManagerModule3.instance.CurrentLevelProgress(counter, 4.0f);
        if (counter == 0 && PlayerPrefs.GetInt("sofia") == 5)
        {
            GameManagerModule3.instance.NPCsNames.text = "Sofia";
            GameManagerModule3.instance.NpcImages[2].SetActive(true);
            GameManagerModule3.instance.DialogPanel.SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "I honestly don't know how this could have happened, I mean, I have been pretty careful, I just don't get how this could have happened. Can you help me figure it out and fix it.";
            counter++;
        }
        else if (counter == 1)
        {

            Lesson3__5.SetActive(true);
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            counter++;
        }
        else if (counter == 2)
        {
            if(s1Amount + s2Amount + s3Amount + s4Amount + s5Amount == 810)
            {
                GameManagerModule3.instance.PopUpShow(0);
                Invoke("DialogueShowWithWait", 2f);
                GameManagerModule3.instance.Toast.Show("+15 Points\n+100 Money",2);
                GameManagerModule3.instance.AddPoint(15);
                GameManagerModule3.instance.AddBankMoney(100,1);
                Task1.SetActive(false);
                //Task2.SetActive(true);
                counter++;

            }
            else
            {
                GameManagerModule3.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule3.instance.LifeEnd();
            }

        }

        else if (counter == 3)
        {
           // Debug.Log("i rann");
            if (toggle[0].isOn&& toggle[2].isOn&& toggle[3].isOn && !toggle[1].isOn)
            {
                GameManagerModule3.instance.PopUpShow(0);
                Task2.transform.GetChild(0).gameObject.SetActive(false);
                counter++;
                Task3_5NextBtn();
            }
            else
            {
               
                GameManagerModule3.instance.PopUpShow(1);
                if (PlayerPrefs.GetInt("lives") <= -1)
                    GameManagerModule3.instance.LifeEnd();
            }
        }
        else if (counter == 4)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(true);
            GameManagerModule3.instance.DialogueBox.SetActive(true);
            GameManagerModule3.instance.DialogueText.text = "Click to select 2 corrective actions to increase credit score.";
            counter++;
        }
        else if (counter == 5)
        {
            GameManagerModule3.instance.NpcImages[2].SetActive(false);
            GameManagerModule3.instance.DialogueBox.SetActive(false);
            Task2.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (counter == 6)
        {
            Lesson3__5.SetActive(false);
            PlayerPrefs.SetInt("sofia", 6);
            PlayerPrefs.SetInt("gameprogress", 14);
            GameManagerModule3.instance.RewardPanell();
            GameManagerModule3.instance.AddBankMoney(200, 1);
            GameManagerModule3.instance.AddPoint(20);
        }
    }

    public void MCQs(int mcqBtn)
    {
        if (mcqBtn == 0 || mcqBtn == 3)
        {
            task2MCQ[mcqBtn].GetComponent<Image>().color = Color.green;
            GameManagerModule3.instance.PopUpShow(0);
            task2mcqCount++;
            Debug.Log(task2mcqCount);
            if (task2mcqCount == 2)
            {
                counter++;
                Task3_5NextBtn();
            }
        }
        else if (mcqBtn == 1 || mcqBtn == 2)
        {
            task2MCQ[mcqBtn].GetComponent<Image>().color = Color.red;
            GameManagerModule3.instance.PopUpShow(1);
            if (PlayerPrefs.GetInt("lives") <= -1)
                GameManagerModule3.instance.LifeEnd();
        }
    }
    public void DialogueShowWithWait()
    {
        Task2.SetActive(true);
    }
    public void Retry()
    {
        PlayerPrefs.SetInt("sofia", 5);
        PlayerPrefs.SetInt("gameprogress", 13);
        SceneManager.LoadScene(4);
    }
}
