using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public Vector2 GetMoveDirection()
    {
#if UNITY_ANDROID || UNITY_IOS
        return Joystick.Instance.Direction;
#else
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
#endif
    }
}
