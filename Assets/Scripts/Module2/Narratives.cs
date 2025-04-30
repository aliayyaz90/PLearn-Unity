using UnityEngine;
public class Narratives : MonoBehaviour
{
    int counter;
    [SerializeField] GameObject GrandmaPic;
    void Start()
    {
        Invoke("ShowNarratives", 2f);
        counter = 0;
        GameManagerModule2.instance.NPCsNames.text = "";
    }
    public void ShowNarratives()
    {   
        if (counter == 0)
        {
            Time.timeScale = 0;
            GameManagerModule2.instance.DialogPanel.SetActive(true);
            GameManagerModule2.instance.DialogueBox.SetActive(true);
            GameManagerModule2.instance.DialogueText.text = "After proving their financial responsibility in London, Alex takes Grandma Millie’s advice and moves out to broaden their horizons. They know about financial responsibility, but that is just the first step in understanding how money works.";
            counter++;
        }
        else if (counter == 1)
        {
            GameManagerModule2.instance.DialogueText.text = "Following the path Grandma Millie laid out for them, Alex arrives in Belfast, a fast-paced financial hub known for its business district and the cutting-edge banking services provided by its banking sector.";
            counter++;
        }
        else if (counter == 2)
        {
            GameManagerModule2.instance.DialogueText.text = "Grandma Millie sent Alex here to understand how banks operate, what they do, and how.";
            counter++;
        }
        else if (counter == 3)
        {
            GameManagerModule2.instance.NPCsNames.text = "Grandma Millie";
            GrandmaPic.SetActive(true);
            GameManagerModule2.instance.DialogueText.text = "You have to earn your own money, and you get to spend it however you like, but money can't function without a bank. Banks let you store money, move money, and use money to make more money.";
            counter++;
        }
        else if (counter == 4)
        {
            GameManagerModule2.instance.DialogueText.text = "You'll learn more about banks from my friend Mr. Patel, he's a veteran, he'll show you the ropes. Just remember Alex, the bank may hold your money, but they don't own it, it's still your money, you just need to be smart about how you use it.";
            counter++;
        }
        else if (counter == 5)
        {
            GrandmaPic.SetActive(false);
            Time.timeScale = 1;
            GameManagerModule2.instance.DialogPanel.SetActive(false);
            GameManagerModule2.instance.DialogueBox.SetActive(false);
            GetComponent<Narratives>().enabled = false;
            GetComponent<Lesson2_1>().enabled = true;
        }
    }
}