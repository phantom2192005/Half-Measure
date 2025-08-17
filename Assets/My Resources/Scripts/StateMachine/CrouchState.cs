using UnityEngine;

public class CrouchState : State
{
       // hoạt ảnh cúi đầu xuống và collider cũng hạ chiều cao xuống
       // nhân vật có thể di chuyển được trên mặt đất và tốc độ của nhân vật giảm xuống
       // có thể thực hiện khi đang trong bất cứ trạng thái nào trừ DashState
       // khi đang ở trạng thái Idle, Walk, Run thì nhân vật sẽ thực hiện việc ngồi xuống... như bình thường
       // khi đang ở trạng thái Jump,Fall thì nhân vật sẽ thực hiện hành vi giẫm đất(Ground Pound) xuống dưới theo hướng từ trên xuống dưới
       // nhân vật sẽ được đẩy 1 lực hướng xuống dưới thẳng đứng và khi chạm mặt đất thì delay 1 khoảng thời gian trước khi thoát trạng thái
       // trong quá trình "ground Pound" thì nhân vật sẽ không chuyển sang bất cứ trạng thái nào khác
       public override void Enter(PlayerController playerController)
       {
              //Debug.Log("Enter Crounch State");
              playerController.ApplyHurtBoxSetting("Crouch");
              playerController.body.localScale = new Vector3
              (
                     playerController.transform.localScale.x,
                     playerController.CrouchScaleY,
                     playerController.transform.localScale.z
              );
              
       }

       public override void FixedUpdate(PlayerController playerController)
       {
              playerController.MoveCharacter();
              if (playerController.crouchAction.IsInProgress() == false && playerController.overHeadChecker == false)
              {
                     playerController.ChangeState(new IdleState());
              }
       }

       public override void Exit(PlayerController playerController)
       {
              playerController.body.localScale = new Vector3
              (
                     playerController.transform.localScale.x,
                     playerController.StartScaleY,
                     playerController.transform.localScale.z
              );
              //Debug.Log("Exit Crounch State");
       }

       public override State HandleInput(PlayerController playerController)
       {
              throw new System.NotImplementedException();
       }
}
