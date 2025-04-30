using System;
using UnityEngine;
using UnityEngine.UI;
public class LivesTimer : MonoBehaviour
{
    public static LivesTimer instance;
    public Text Livestime;
    public static DateTime dt;
    TimeSpan difference, min;
    [SerializeField] GameObject LifeButton;   
    private void Start()
    {
        Livestime.gameObject.SetActive(true);
    }
    private void Update()
    {    
        difference = DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("LastRewardTime", DateTime.MinValue.ToString()));
        if (PlayerPrefs.GetInt("lives") == -1)
        {
            Livestime.text = "Next trial in " + (19 - (Int32)difference.TotalMinutes).ToString() + ":" + (60 - (Int32)difference.TotalSeconds%60).ToString();
            if (difference.TotalSeconds >= 1200)
            {
                PlayerPrefs.SetString("LastRewardTime", DateTime.Now.ToString());
                PlayerPrefs.SetInt("lives", 0);
                return;
            }
        }
        else if (PlayerPrefs.GetInt("lives") ==0)
        {
            Livestime.text= "Next trial in " + (19 -(Int32)difference.TotalMinutes).ToString() +":"+ (60 - (Int32)difference.TotalSeconds % 60).ToString();
            if (difference.TotalSeconds >= 1200)
            {
                PlayerPrefs.SetString("LastRewardTime", DateTime.Now.ToString());
                PlayerPrefs.SetInt("lives", 1);
                return;
            }
        }
        else if(PlayerPrefs.GetInt("lives") == 1)
        {
            Livestime.text = "Next trial in " + (19 - (Int32)difference.TotalMinutes).ToString() + ":" + (60 - (Int32)difference.TotalSeconds % 60).ToString();
            if (difference.TotalSeconds >= 1200)
            {           
                PlayerPrefs.SetInt("lives", 2);
                LifeButton.SetActive(false);
                return;
            }
        }
        else
            Livestime.gameObject.SetActive(false);     
    }
}