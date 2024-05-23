using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Continue()
    {
        //TODO load last save
        Debug.Log("Load game");
    }
    
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }
}
