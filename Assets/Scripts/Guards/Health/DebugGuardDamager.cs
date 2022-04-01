using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DebugGuardDamager : MonoBehaviour
{
    Guards[] guards;
    bool damageButton;

    //S2 - Assignment 02
    public Transform levelObj;
    Animator anim;
    GameObject obj;
    LevelController levelController;

    private void Start()    //S2 - Assignment 02
    {
        levelObj = GetComponent<Transform>();
        //obj = GameObject.Find("Player").GetComponent<GameObject>();
        levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
        anim = GameObject.Find("Player").GetComponentInChildren<Animator>();    //IMP: Use "GameObject" whenever not working with other components.
        //anim = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
            guards = gameObject.GetComponentsInChildren<Guards>();

        #endif

        //guradHealth = new GuardHealth(guards.generalData.maxHealth);    //When it is not present on GameObjct or not a MonoBehaviour
        //guardHelath = GetComponent<GuardHealth>();  //Need to be on a GameObject
    }
    
    private void OnGUI()
    {
        if(guards == null)
        {
            return;
        }
        
        if(GUI.Button(new Rect(0, 30, 100, 30), "Hit Back"))   //Button1 - Back    //S2 - Assignment 02
            //anim?.SetTrigger("HitFront");
            levelController.player.Controller.playerHealth.OnDamageTaken(0f);
            //Debug.Log(levelObj);

        if(GUI.Button(new Rect(0, 60, 100, 30), "Hit Front"))   //Button2 - Front    //S2 - Assignment 02
            //levelController.player.Controller.playerHealth.OnDamageTaken(50f);
            anim?.SetTrigger("HitFront");


        /*for(int i = 0; i < guards.Length; i++)
        {
            if(guards[i] == null)
            {
                continue;   //Increments 'i' value. Does not execute below code
            }     

            float yPos = Screen.height - (30 * (i+1));
            Rect rect = new Rect(0, yPos, 200, 30);
            
            damageButton = GUI.Button(rect, "Damage" + guards[i].gameObject.name);
     
            if(damageButton)
            {
                guards[i].TakeDamage(guards[i].generalData.attackDamage, transform);    //transform = instigator
            }

        }*/
    }
}
