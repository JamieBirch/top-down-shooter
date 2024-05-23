using UnityEngine;

public class PauseCanvas : MonoBehaviour
{
    public void Close()
    {
        Debug.Log("Continue");
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
    
    public void Quit()
    {
        Application.Quit();
    }

}
