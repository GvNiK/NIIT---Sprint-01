using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  This Script moves the Rigidbody along the Animator values. As we are Using "Root Motion".
public class PlayerAnimationController : MonoBehaviour
{
    private float moveSpeedMultiplayer;
    private Animator animator;
    private Rigidbody rb;

    public void Setup(Animator animator, float moveSpeedMultiplayer, Rigidbody rb)
    {
        this.animator = animator;
        this.moveSpeedMultiplayer = moveSpeedMultiplayer;
        this.rb = rb;
    }

    //OnAnimatorMove = Callback for processing animation movements for modifying root motion.
    public void OnAnimatorMove() 
    {
        if(Time.deltaTime > 0)
        {
            //animator.deltaPosition = The Position value from Last Frame to Current Frame.
            Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplayer) / Time.deltaTime;

            //We want the animator & the Player stuck to the gorund. So we do not modify the Animator's Y value.
            //We keep the Animator's Y value equals to Rigidbody's Y value.
            velocity.y = rb.velocity.y;

            //In this way, we are just moving the Rigidbody along X & Z axes.
            rb.velocity = velocity;

            //animator.deltaRotation = The Rotation value from Last Frame to Current Frame.
            rb.rotation *= animator.deltaRotation;  
        }
            
    }
}
