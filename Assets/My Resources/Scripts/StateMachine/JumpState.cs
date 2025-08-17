using UnityEngine;

public class JumpState : State
{
       //thêm 1 lực theo chiều y dương rồi player vẫn có thể di chuyển trong quá trình ở trên cao
       // tùy vào thời lượng nhấn nút nhảy , player có thể nhảy cao hơn hoặc thấp hơn tương ứng
       // giới độ nhảy tối đa và tối thiểu
       // khi đạt độ cao tối đa, player chuyển qua trạng thái Fall State
       // có thể double Jump
       // Di chuyển ngang
       public override void Enter(PlayerController playerController)
       {
              //Debug.Log("Enter Jump State");
              playerController.isGravityEffect = false;
              //reset y velocity
              playerController.rb.velocity = new Vector3(playerController.rb.velocity.x, 0f, playerController.rb.velocity.z);
              playerController.onGround = false;
              playerController.ApplyHurtBoxSetting("Idle");
       }
       public override void FixedUpdate(PlayerController playerController)
       {
              playerController.MoveCharacter();

              if (playerController.jumpTimer <= 0 || playerController.jumpCounter > playerController.jumpMaxCount)
              {
                     playerController.ChangeState(new IdleState());
              }

              // Nhảy theo thời gian giữ nút
              if (playerController.jumpAction.IsPressed() && playerController.jumpTimer > 0)
              {
                     playerController.isGravityEffect = false;
                     playerController.rb.AddForce(Vector3.up * playerController._jumpForce, ForceMode.Force);
              }

              // Giảm timer
              playerController.jumpTimer -= Time.fixedDeltaTime;

              if (playerController.jumpTimer <= 0 || playerController.jumpAction.IsPressed() == false)
              {
                     playerController.isGravityEffect = true;
              }

              // Kiểm tra kết thúc nhảy
       }


       public override void Exit(PlayerController playerController)
       {
              //playerController.isGravityEffect = true;
              //Debug.Log("Exit Jump State");
       }

       public override State HandleInput(PlayerController playerController)
       {
              return this;
       }
}