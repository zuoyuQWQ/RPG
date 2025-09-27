using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    private int playerLayer;
    private int dashingLayer;

    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
        // 在构造函数中缓存图层索引，以提高性能
        playerLayer = LayerMask.NameToLayer("Player");
        dashingLayer = LayerMask.NameToLayer("GhostStatus");
    }

    public override void Enter()
    {
        base.Enter();

        player.gameObject.layer = dashingLayer; // 进入冲刺状态，切换到GhostStatus层
        player.skill.clone.CreatCloneOnDashStart();

        stateTimer = player.dashDurition;
    }

    public override void Exit()
    {
        base.Exit();

        player.gameObject.layer = playerLayer; // 退出冲刺状态，切换回Player层
        player.skill.clone.CreateCloneOnDashOver();
        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        // 在空中冲刺时施加一点重力，使其感觉更自然
        if (!player.IsGroundDetected() && player.rb.velocity.y < 0)
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.rb.velocity.y * .9f);

        player.SetVelocity(player.dashDir * player.dashSpeed, rb.velocity.y);

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
