using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public void LevelSelectorScene()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("modules", 2);
        PlayerPrefs.SetInt("gameprogress", 9);
        PlayerPrefs.SetInt("lives", 2);
        SceneManager.LoadScene(1);

    }
}
