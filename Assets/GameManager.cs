using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PauseCanvas pauseCanvas;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            if (pauseCanvas.gameObject.activeSelf)
            {
                pauseCanvas.Close();
            }
            else
            {
                SetPause();
            }
        }
    }

    private void SetPause()
    {
        Time.timeScale = 0f;
        pauseCanvas.gameObject.SetActive(true);
    }
}
