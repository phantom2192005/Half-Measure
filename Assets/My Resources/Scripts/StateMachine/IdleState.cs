using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class IdleState : State
{
    public override void Enter(PlayerController playerController)
    {
        //Debug.Log("Entered Idle State");
        playerController.ApplyHurtBoxSetting("Idle");
    }
    public override void Update(PlayerController playerController)
    {
        HandleInput(playerController);
    }

    public override State HandleInput(PlayerController playerController)
    {
        if (playerController.MoveInput != Vector2.zero)
        {
            playerController.ChangeState(new WalkState());
        }

        return this;
    }

    public override void Exit(PlayerController playerController)
    {
        //Debug.Log("Exiting Idle State");
    }
}
