using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData/BaseData")]
public class PlayerData : ScriptableObject
{
   [Header("Move State")] 
   public float movementVelocity = 10f;

   [Header("Jump State")]
   public float jumpVelocity = 15f;
   public int jumpTimes = 1;

   [Header("In Air State")] 
   public float coyoteTime = 0.2f;
   public float variableJumpHeightMultiplier = 0.6f;

   [Header("Wall Slide State")] 
   public float wallSlideVelocity = 2f;

   [Header("Wall Jump State")] 
   public float wallJumpVelocity = 20f;
   public float wallJumpTime = 0.4f;
   public Vector2 wallJumpAngle = new Vector2(1, 2);

   [Header("Dash State")] 
   public float dashCoolDown = 0.5f;
   public float dashVelocity = 30f;
   public float dashDrag = 10f;
   
   [Header("Check Variables")] 
   public float groundCheckDistance = 0.15f;
   public float wallCheckDistance = 0.3f;
   public float backWallCheckDistance = 1f;
   public LayerMask groundLayer;
   public LayerMask wallLayer;
}
