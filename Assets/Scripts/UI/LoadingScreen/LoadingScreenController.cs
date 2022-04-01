using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenController    //Assignment - 02
{
    private GameObject loadingScreen;
    private Animator animator;
    private AnimationListener animationListener;
    private bool animInComplete;
    private bool hideCalled;
    public Slider slider;
    public Text progressText;

   public LoadingScreenController()
   {
       GameObject prefab = Resources.Load<GameObject>("UI/LoadingScreen");
       loadingScreen = GameObject.Instantiate(prefab);

       GameObject.DontDestroyOnLoad(loadingScreen);
       animator = loadingScreen.GetComponent<Animator>();

       animationListener = loadingScreen.GetComponent<AnimationListener>();
       animationListener.OnAnimationEvent += AnimationReceived;

       slider = GameObject.FindObjectOfType<Slider>();      //FindObjectOfType - Used to find Components/Objects (Use when GetComponent doesn't work!)
       //slider = loadingScreen.GetComponentInChildren<Slider>();     
       progressText = GameObject.FindObjectOfType<Text>();  

   }

    public void Hide()
    {
        hideCalled = true;
        AttemptHide();
    }

    public void Show()
    {
        hideCalled = false;
        animInComplete = false;
        animator.SetBool("show", true);
    }

    private void AnimationReceived(string payload)
    {
        if(payload == "animInComplete")
        {
            animInComplete = true;
            AttemptHide();
        }
        else if(payload == "animOutComplete")
        {
            animInComplete = false;
        }
    }

    private void AttemptHide()
    {
       if(animInComplete && hideCalled)
       {
           animator.SetBool("show", false);
       }
    }

}

