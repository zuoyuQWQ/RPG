using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    private int playerLayer;
    private int enemyLayer;
    public PlayerDeadState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        player.gameObject.layer = enemyLayer;
    }

    public override void Exit()
    {
        base.Exit();
        player.gameObject.layer = playerLayer;
    }

    public override void Update()
    {
        base.Update();

        player.ZeroVelocity();
    }
}
