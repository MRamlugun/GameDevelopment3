using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : MovementBaseState
{
    float timePassed;
    float clipLength, clipSpeed;
    bool attack;

    // Called when entering the attack state
    public override void EnterState(Movement movement)
    {
        attack = false; // Reset the attack flag
        timePassed = 0f; // Reset time passed
        movement.anim.SetTrigger("attack"); // Trigger the attack animation
    }

    // Called every frame while in the attack state
    public override void UpdateState(Movement movement)
    {
        timePassed += Time.deltaTime; // Track the time passed in this state
        clipLength = movement.anim.GetCurrentAnimatorClipInfo(1)[0].clip.length; // Get the length of the attack animation
        clipSpeed = movement.anim.GetCurrentAnimatorStateInfo(1).speed; // Get the speed of the attack animation

        // Check if the attack input is pressed
        if (Input.GetKey(KeyCode.Q))
        {
            attack = true;

        }

        // Check if it's time to transition to another state (Attack or Combat)
        if (timePassed >= clipLength / clipSpeed && attack)
        {
            movement.SwitchState(movement.Attack); // Switch to the Attack state
        }

        // Check if it's time to transition to the Combat state
        if (timePassed >= clipLength)
        {
            movement.anim.SetTrigger("move"); // Trigger the move animation
            movement.SwitchState(movement.Combat); // Switch to the Combat state
        }
    }

}
