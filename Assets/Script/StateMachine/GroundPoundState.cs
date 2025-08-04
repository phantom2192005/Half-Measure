using UnityEngine;

public class GroundPoundState : State
{
       // điều kiện để chuyển sang trạng thái Groundpound là Onground == false
       //Enter sẽ delay 1 khoảng thời gian rồi tạo 1 lực vào player hướng thắng đứng xuống đất và bật gravity effect
       // khi chạm đất sẽ delay 1 khoảng thời gian trước khi chuyển về trạng thái IdleState


       public override void Enter(PlayerController playerController)
       {
              //Debug.Log("Enter GroundPoundState State");
              playerController.isGravityEffect = false;
              //thiết lập delay time
              playerController.delayGPTimer = playerController.delayGPTime;
              playerController.delayOutGPTimer = playerController.delayGPTime;
              playerController.rb.velocity = Vector3.zero; // Xóa vận tốc cũ để không bị cộng dồn
       }

       public override void FixedUpdate(PlayerController playerController)
       {
              playerController.delayGPTimer -= Time.fixedDeltaTime;

              // delay trước khi thực hiện groundPoundState
              if (playerController.onGround == false && playerController.delayGPTimer <= 0 )
              {
                     // tính lực và hướng
                     Vector3 groundPoundDirection = Vector3.down;
                     Vector3 groundPoundForce = groundPoundDirection * playerController.groundPoundForce;
                     playerController.rb.AddForce(groundPoundForce, ForceMode.Impulse);
              }
              // delay trước khi thoát khỏi trạng thái groundPoundState
              if (playerController.onGround)
              {
                     playerController.delayOutGPTimer -= Time.fixedDeltaTime;
              }
              if (playerController.delayOutGPTimer <= 0)
              {
                     playerController.ChangeState(new IdleState());
              }
       }

       public override void Exit(PlayerController playerController)
       {
              playerController.isGravityEffect = true;
              playerController.delayGPTimer = 0;
              playerController.delayOutGPTimer = 0;
              Debug.Log("Exit GroundPoundState State");
       }

       public override State HandleInput(PlayerController playerController)
       {
              throw new System.NotImplementedException();
       }
}
