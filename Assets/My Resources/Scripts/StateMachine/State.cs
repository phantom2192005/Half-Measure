using UnityEngine;
using UnityEngine.InputSystem;

public abstract class State
{
    public abstract void Enter(PlayerController playerController);

    public virtual void Update(PlayerController playerController) { }

    public virtual void FixedUpdate(PlayerController playerController) { }

    public abstract State HandleInput(PlayerController playerController);

    public abstract void Exit(PlayerController playerController);
}
