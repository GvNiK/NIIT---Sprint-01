using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    private TimeController timeController;
    public Action OnResume = delegate { }; //Action - delegate
    public Action OnPause = delegate { }; //Action - delegate
    public Action OnRestart = delegate { };
    public GameObject pauseMenuUI;
    
    public void Show()
    {
        pauseMenuUI.SetActive(true);
    }

    public void Hide()
    {
        pauseMenuUI.SetActive(false);   
    }

    public void Resume()
    {
        OnResume.Invoke();  //Invokes whenever & wherever it is called. Also can be written as "OnResume();"
    }

    public void Pause()
    {
        OnPause.Invoke();   //Invokes whenever & wherever it is called.     
    }

    public void Restart()
    {
        OnRestart.Invoke(); //Invokes whenever & wherever it is called.
    }
}
