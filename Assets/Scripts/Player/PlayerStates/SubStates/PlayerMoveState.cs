using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    bool flying;
    public PlayerMoveState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Dochecks()
    {
        base.Dochecks();
    }

    public override void Enter()
    {
        base.Enter();
        flying = false;
    }

    public override void Exit()
    {
        base.Exit();
        if (flying)
        {
            player.AudioController.PlayerFlyEnd();
            flying = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!hasExited)
        {
            player.FlipIfNeed(player.InputHandler.NormInputX);
            if (player.InputHandler.NormInputX == 0 && player.InputHandler.NormInputY == 0 && player.CheckGrounded())
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (Mathf.Abs(player.InputHandler.NormInputY) >= 1e-5)
        {
            player.SetVelocity(playerData.movementVelocity, player.InputHandler.MovementInput);
            if (!flying)
            {
                player.AudioController.PlayerFlyStart();
                flying = true;
            }
            // Debug.Log(flying);
        }
        else
        {
            if (Mathf.Abs(player.InputHandler.NormInputX) > 0)
                player.SetVelocityX(playerData.movementVelocity * player.InputHandler.NormInputX);
            if (flying)
            {
                player.AudioController.PlayerFlyEnd();
                flying = false;
            }
        }
    }
}
