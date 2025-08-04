using UnityEngine;

public class RunState : State
{
       //Run State là State con của WalkState, khác với Walk ở phần tốc độ di chuyển nhanh hơn
       //để kích hoạt RunState cần có 2 điều kiện là (người chơi đang trong WalkState và nhấn giữ nút Run)
       public override void Enter(PlayerController playerController)
       {
              //Debug.Log("Entered Run State");
              playerController.moveAcceleration += playerController.runAcceleration;
              playerController.ApplyHurtBoxSetting("Idle");
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
              playerController.moveAcceleration -= playerController.runAcceleration;
              //Debug.Log("Exiting Run State");
              // TODO: Stop walk animation
       }
}

