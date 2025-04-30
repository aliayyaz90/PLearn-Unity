using UnityEngine;
using UnityEngine.SceneManagement;
public class EnvironmentalDetections : MonoBehaviour
{  
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "grandma")
        {      
            GameManager.instance.InteractionText.text = "Do you want to interact with Grandma Millie?";
            if (PlayerPrefs.GetInt("grandma") == 0)
                PlayerPrefs.SetInt("grandma", 2);
            else if(PlayerPrefs.GetInt("grandma") == 1 && PlayerPrefs.GetInt("mrgreen")==1)
                PlayerPrefs.SetInt("grandma", 3);
            else if(PlayerPrefs.GetInt("grandma") == 4 && PlayerPrefs.GetInt("emma")!=0)
                PlayerPrefs.SetInt("grandma", 5);
        }
        if (other.gameObject.tag == "mrgreen")
        {       
            GameManager.instance.InteractionText.text = "Do you want to interact with Mr.Green?";
            if(PlayerPrefs.GetInt("mrgreen") == 0)
                PlayerPrefs.SetInt("mrgreen", 2);
        }
        if (other.gameObject.tag == "emma")
        {        
            GameManager.instance.InteractionText.text = "Do you want to interact with Emma?";
            if (PlayerPrefs.GetInt("emma") == 0)
                PlayerPrefs.SetInt("emma", 2);
        }
        if (other.gameObject.tag == "patel")
        {          
            if (GameManagerModule2.instance.GetComponent<Lesson2_3>().enabled)
                GameManagerModule2.instance.InteractionText.text = "Do you want to interact with Mr.Thompson?";
            else
                GameManagerModule2.instance.InteractionText.text = "Do you want to interact with Mr.Patel?";
            if (PlayerPrefs.GetInt("patel") == 0)
                PlayerPrefs.SetInt("patel", 2);
        }
        if (other.gameObject.tag == "sofia")
        {
            GameManagerModule3.instance.InteractionText.text = "Do you want to interact with Sofia?";
            if (PlayerPrefs.GetInt("sofia") == 0)
                PlayerPrefs.SetInt("sofia", 2);
            //else if (PlayerPrefs.GetInt("sofia") == 1 && PlayerPrefs.GetInt("mrgreen") == 1)
            //    PlayerPrefs.SetInt("grandma", 3);
            //else if (PlayerPrefs.GetInt("grandma") == 4 && PlayerPrefs.GetInt("emma") != 0)
            //    PlayerPrefs.SetInt("grandma", 5);
        }



        if (other.gameObject.tag == "leo")
        {
            //GameManagerModule4.instance.InteractionText.text = "Do you want to interact with leo?";
            if (PlayerPrefs.GetInt("gameprogress") == 23)
            {
                //GameManagerModule4.instance.GetComponent<Lesson4_1>().enabled = true;
                //GameManagerModule4.instance.GetComponent<Lesson4_1>().Lesson4__1.gameObject.SetActive(true);

                GameManagerModule4.instance.GetComponent<Lesson4_Controller>().enabled = true;
                GameManagerModule4.instance.transform.GetComponent<Lesson4_Controller>().TaskFlow_NextBtn();

                Debug.Log("leo 0");
            }
            if (PlayerPrefs.GetInt("gameprogress") == 24)
            {
                //GameManagerModule4.instance.GetComponent<Lesson4_2>().enabled = true;
                //GameManagerModule4.instance.GetComponent<Lesson4_1>().enabled = false;

                //GameManagerModule4.instance.GetComponent<Lesson4_1>().Lesson4__1.transform.GetChild(0).gameObject.SetActive(false);

                //GameManagerModule4.instance.GetComponent<Lesson4_2>().Lesson4__2.gameObject.SetActive(true);
                //
                GameManagerModule4.instance.GetComponent<Lesson4_2_Controller>().enabled = true;
                GameManagerModule4.instance.transform.GetComponent<Lesson4_2_Controller>().TaskFlow_NextBtn();
            }
            if (PlayerPrefs.GetInt("gameprogress") == 25)
            {
                //GameManagerModule4.instance.GetComponent<Lesson4_2>().enabled = true;
                //GameManagerModule4.instance.GetComponent<Lesson4_1>().enabled = false;

                //GameManagerModule4.instance.GetComponent<Lesson4_1>().Lesson4__1.transform.GetChild(0).gameObject.SetActive(false);

                //GameManagerModule4.instance.GetComponent<Lesson4_2>().Lesson4__2.gameObject.SetActive(true);
                //
                GameManagerModule4.instance.GetComponent<Lesson4_3_Controller>().enabled = true;
                GameManagerModule4.instance.transform.GetComponent<Lesson4_3_Controller>().TaskFlow_NextBtn();
            }
            if (PlayerPrefs.GetInt("gameprogress") == 26)
            {
                //GameManagerModule4.instance.GetComponent<Lesson4_2>().enabled = true;
                //GameManagerModule4.instance.GetComponent<Lesson4_1>().enabled = false;

                //GameManagerModule4.instance.GetComponent<Lesson4_1>().Lesson4__1.transform.GetChild(0).gameObject.SetActive(false);

                //GameManagerModule4.instance.GetComponent<Lesson4_2>().Lesson4__2.gameObject.SetActive(true);
                //
                GameManagerModule4.instance.GetComponent<Lesson4_4_Controller>().enabled = true;
                GameManagerModule4.instance.transform.GetComponent<Lesson4_4_Controller>().TaskFlow_NextBtn();
            }
            if (PlayerPrefs.GetInt("gameprogress") == 27)
            {
                //GameManagerModule4.instance.GetComponent<Lesson4_2>().enabled = true;
                //GameManagerModule4.instance.GetComponent<Lesson4_1>().enabled = false;

                //GameManagerModule4.instance.GetComponent<Lesson4_1>().Lesson4__1.transform.GetChild(0).gameObject.SetActive(false);

                //GameManagerModule4.instance.GetComponent<Lesson4_2>().Lesson4__2.gameObject.SetActive(true);
                //
                GameManagerModule4.instance.GetComponent<Lesson4_5_Controller>().enabled = true;
                GameManagerModule4.instance.transform.GetComponent<Lesson4_5_Controller>().TaskFlow_NextBtn();
            }
        }

       // GameManagerModule4.instance.CurrentLevel();
        if (SceneManager.GetActiveScene().buildIndex == 2)
            GameManager.instance.OnClickYesBtn();
        else if (SceneManager.GetActiveScene().buildIndex == 3)
            GameManagerModule2.instance.OnClickYesBtn();
        else if (SceneManager.GetActiveScene().buildIndex == 4)
            GameManagerModule3.instance.OnClickYesBtn();
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "grandma" || other.gameObject.tag == "mrgreen" || other.gameObject.tag == "emma")
            GameManager.instance.InteractionHolder.SetActive(false);
        else if(other.gameObject.tag == "patel")
            GameManagerModule2.instance.InteractionHolder.SetActive(false);
    }
}