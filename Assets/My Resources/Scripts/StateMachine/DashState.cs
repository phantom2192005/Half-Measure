using UnityEngine;

public class DashState : State
{
    //tạo 1 lực đẩy nhân vật về hướng dựa theo input , đẩy theo trục nằm ngang (x và z)
    //khi lướt gravity effect tắt (kháng trọng lực)
    //có thể lướt gần như trong mọi State (Jump, Walk,Idle,Run,Fall....)
    //lướt mang thông tin :(tốc độ (lực) , thời gian lướt)
    // trong quá trình lướt thì nhân vật không thể move character lung tung
    public override void Enter(PlayerController playerController)
    {
        //Debug.Log("Enter Dash State");

        playerController.isGravityEffect = false;
        //reset y velocity
        playerController.rb.velocity = new Vector3(playerController.rb.velocity.x, 0f, playerController.rb.velocity.z);
        
        playerController.dashTimer = playerController.dashTime;

        Vector3 dashDirection = playerController.CaculateMoveDirection();
        Vector3 dashForce = dashDirection * playerController._dashForce;

        playerController.rb.velocity = Vector3.zero;
        //playerController.turnSmoothTime = 0.01f;
        playerController.rb.AddForce(dashForce, ForceMode.Impulse);
    }

    public override void FixedUpdate(PlayerController playerController)
    {
        playerController.dashTimer -= Time.fixedDeltaTime;
        playerController.isGravityEffect = false;

        Vector3 dashDirection = playerController.CaculateMoveDirection();
        if (dashDirection.sqrMagnitude > 0.01f) // Tránh LookRotation lỗi nếu direction ≈ zero
        {
            playerController.transform.rotation = Quaternion.LookRotation(dashDirection, Vector3.up);
        }

        if (playerController.dashTimer <= 0)
        {
            playerController.ChangeState(playerController.overHeadChecker
                ? new CrouchState()
                : new IdleState());
        }
    }

    public override void Exit(PlayerController playerController)
    {
        playerController.isGravityEffect = true;
        //Debug.Log("Exit Dash State");
    }

    public override State HandleInput(PlayerController playerController)
    {
        return null;
    }
}
