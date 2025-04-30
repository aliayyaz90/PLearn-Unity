using UnityEngine;
public class Narratives4 : MonoBehaviour
{
    int counter;
    [SerializeField] GameObject GrandmaPic;
    [SerializeField] GameObject SophiaPic;
    [SerializeField] GameObject LeoPic;
    void Start()
    {
        if (PlayerPrefs.GetInt("leo") == 0)
        {
            GameManagerModule4.instance.GetComponent<Narratives4>().enabled = true;
            Invoke("ShowNarratives", 1.5f);
            counter = 0;
            GameManagerModule4.instance.NPCsNames.text = "";
        }
        else
        {
            GameManagerModule4.instance.GetComponent<Narratives4>().enabled = false;
        }


    }
    public void ShowNarratives()
    {
        if (counter == 0)
        {
            Time.timeScale = 0;
            GameManagerModule4.instance.DialogPanel.SetActive(true);
            GameManagerModule4.instance.DialogueBox.SetActive(true);
            GameManagerModule4.instance.DialogueText.text = "After Traveling to Barcelona, Alex teams up with Leo, a tech-savvy entrepreneur who is Sophia's cousin and recently opened a digital design studio.";
            counter++;
        }
        else if (counter == 1)
        {
            GameManagerModule4.instance.NPCsNames.text = "Sophia";
            SophiaPic.SetActive(true);
            GameManagerModule4.instance.DialogueText.text = "You gotta go to Barcelona, Alex, my cousin Leo has his own business, I know he can help you grow, the same way you've helped me";
            counter++;
        }
        else if (counter == 2)
        {
            SophiaPic.SetActive(false);
            Time.timeScale = 1;
            GameManagerModule4.instance.DialogPanel.SetActive(false);
            GameManagerModule4.instance.DialogueBox.SetActive(false);
            GetComponent<Narratives4>().enabled = false;
            GetComponent<Lesson4_Controller>().enabled = true;
        }
    }
}