using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    private bool GameIsPaused; //Default Value = false;
    public GameObject menuUI;
    
    //public CameraController cameraController;
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }

            //GameIsPaused = !GameIsPaused;
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        GameIsPaused = true;
        menuUI.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        GameIsPaused = false;
        menuUI.SetActive(false);      
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        //Application.LoadLevel(Appliaction.LoadLevel);
        //SceneManager.LoadScene(3);
    }

}
