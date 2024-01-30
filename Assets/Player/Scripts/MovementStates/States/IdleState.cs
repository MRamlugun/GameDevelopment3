using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MovementBaseState
{
    bool drawWeapon;
    
        
    public override void EnterState(Movement movement)
    {
        drawWeapon = false;                
        
    }

    public override void UpdateState(Movement movement)
    {
        if (movement.dir.magnitude > 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift))movement.SwitchState(movement.Run);
            else movement.SwitchState(movement.Walk);
        }
        if(Input.GetKeyDown(KeyCode.C)) movement.SwitchState(movement.Crouch);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            movement.previousState = this;
            movement.SwitchState(movement.Jump);

        }

        if(Input.GetKey(KeyCode.R))
        {
            drawWeapon = true;
            
        }

        if(Input.GetKey(KeyCode.Q))
        {
            movement.SwitchState(movement.Attack);
        }
        
        if (drawWeapon)
        {
            movement.SwitchState(movement.Combat);
            movement.anim.SetTrigger("drawWeapon");            
            
        }

        

        

        

        


        

        

                
    }
}
