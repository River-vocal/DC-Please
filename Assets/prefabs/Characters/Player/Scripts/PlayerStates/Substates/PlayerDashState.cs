using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerUseAbilityState
{
    public bool CanDash { get; private set; }
    private float lastDashTime = 0f;
    private bool isTouchingWall;
    private float dashStartTime;
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animationTriggerParameter) : base(player, stateMachine, playerData, animationTriggerParameter)
    {
    }

    public bool CheckIfCanDash()
    {
        return CanDash && Time.time >= lastDashTime + PlayerData.dashCoolDown;
    }

    public void ResetCanDash()
    {
        CanDash = true;
    }

    public override void Enter(params Object[] args)
    {
        base.Enter();
        CanDash = false;
        Player.InputHandler.ConsumeDashInput();
        Player.SetDrag(PlayerData.dashDrag);
        //change facing direction if necessary
        Player.CheckIfShouldFlip((int)Player.InputHandler.MovementInput.x);
        Player.SetXVelocity(PlayerData.dashVelocity * Player.FacingDirection);
        dashStartTime = Time.time;
        Player.DashBlue.SetActive(true);
        Player.SoundManager.PlaySound("dash");
    }

    public override void Exit()
    {
        base.Exit();
        lastDashTime = Time.time;
        Player.ResetDrag();
        Player.DashBlue.SetActive(false);
    }

    public override void Update()
    {
        base.Update();
        
        if (StateMachine.CurState == this)
        {
            if (animationFinished || isTouchingWall || (Time.time > dashStartTime + PlayerData.dashMinimumTime && (Player.InputHandler.JumpPressed || Player.InputHandler.RangeAttackPressed || Player.InputHandler.AttackComboIndex > 0)))
            {
                isAbilityDone = true;
            }
            else
            {
                Player.SetYVelocity(Player.CurVelocity.y * PlayerData.dashYVelocityMultiplier);
            }
        }
    }

    public override void Check()
    {
        base.Check();

        isTouchingWall = Player.CheckIfTouchingWall();
    }
}
