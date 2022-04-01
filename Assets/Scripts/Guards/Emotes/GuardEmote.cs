using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardEmote : MonoBehaviour //Part II
{   
    public Sprite alertedEmote;
    public Sprite lostPlayerEmote;
    public Sprite suspiciousEmote;

    public float emoteDisplayTime = 3.0f;
    private float currentEmoteDisplayTime;
    private Animator animator;
    public SpriteRenderer spriteRenderer;

    private void OnEnable() 
    {
        animator = GetComponent<Animator>();    
    }

    void Update()
    {
        //Track time emote shown for
        currentEmoteDisplayTime -= Time.deltaTime;
        if(currentEmoteDisplayTime <= 0)
        {
            HideEmote();
        }
    }

    public void ShowAlertedEmote()
    {
        spriteRenderer.sprite = alertedEmote;
        DisplayEmote();
    }

    public void ShowPlayerLostEmote()
    {
        spriteRenderer.sprite = lostPlayerEmote;
        DisplayEmote();
    }

    public void ShowSuspiciousEmote()
    {
        spriteRenderer.sprite = suspiciousEmote;
        DisplayEmote();
    }    

    public void DisplayEmote()
    {
        animator.SetBool("Visible", true);
        currentEmoteDisplayTime = emoteDisplayTime;
    }

    public void HideEmote()
    {
        animator.SetBool("Visible", false);
        currentEmoteDisplayTime = 0;
    }
}
