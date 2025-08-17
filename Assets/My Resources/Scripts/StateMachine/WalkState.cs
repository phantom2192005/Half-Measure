using UnityEngine;
using UnityEngine.InputSystem;

public class WalkState : State
{
    public override void Enter(PlayerController playerController)
    {
        //Debug.Log("Entered Walk State");
        // TODO: Play walk animation
    }

    public override void FixedUpdate(PlayerController playerController)
    {
        playerController.MoveCharacter();
        HandleInput(playerController);
    }

    public override State HandleInput(PlayerController playerController)
    {
        if (playerController.MoveInput == Vector2.zero)
        {
            playerController.ChangeState(new IdleState());
        }
        return this;
    }

    public override void Exit(PlayerController playerController)
    {
        //Debug.Log("Exiting Walk State");
        // TODO: Stop walk animation
    }
}
