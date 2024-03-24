using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerStatus", menuName ="Scriptable Object/PlayerStatus")]
public class PlayerStatus : ScriptableObject
{
    [HideInInspector]
    public int currentHP;
    [SerializeField]
    public int playerHP = 5;

    [Space(10), Header("Move")]
    public float moveSpeed = 5;

    [Space(10), Header("Jump")]
    public float jumpPower = 30;
    [SerializeField]
    public float jumpCooldown = 0.8f;

    [Space(10), Header("Sit")]
    public float sitDeceleration = 0.5f;
    public float sitDescentSpeed = 30f;

    [Space(10), Header("Dash")]
    public float dashPower = 20;
    public float dashingTime = 0.1f;
    public float invincibilityTimeAfterDash = 0.2f;
    public int dashDamage = 3;
    public float dashCooldown = 1;
    [Space(10)]
    public float parryDashPower = 40;

    [Space(10), Header("Attack")]
    public float attackReboundPower = 30;
    public float attackReboundTime = 0.05f;
    [Space(5)]
    public float lightAttackTime = 0.1f;
    public float lightAttackCooldown = 0.3f;
    public int lightAttackDamage = 1;
    public int lightCriticalAttackDamage = 2;

}
