using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    void AfterAttack(Vector2 attackDir);
    void Hit(int damage, Vector2 attackDir, bool isHeavyAttack, int criticalDamage = 0);
}

public interface IParry
{
    void Parried();
}

public class Attack : MonoBehaviour
{
    private const float PARRY_ALLOW_TIME = 0.2f; 

    private Collider2D col;
    private SpriteRenderer render;
    private Animator anim;

    private IAttack afterAttack;
    private IParry parried;
    public IParry Parried => parried;

    private string attackFrom;
    public bool isAttackFromMe(string me) => string.Equals(me, attackFrom);
    private Vector2 attackDir;
    private int attackDamage;
    private int criticalDamage;

    // �÷��̾�� ����, ���ʹ� ����̿��� �и� ����
    // ������ ��� ���� => ���� ���̸� ���
    private bool isHeavyAttack;
    private bool isAttackEnable = false;

    private float attackTime = 0f;

    private bool isParriedAttack = false;
    public bool IsParryAllow => (!isHeavyAttack && attackTime < PARRY_ALLOW_TIME);

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        render = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (isAttackEnable)
        {
            attackTime += Time.deltaTime;
        }
    }
    public void SetAttack(string from, IAttack after, IParry parry = null)
    {
        attackFrom = from;
        afterAttack = after;
        parried = parry;
        AttackDisable();
    }

    public void AttackDisable()
    {
        col.enabled = false;
        render.enabled = false;
        isAttackEnable = false;
        attackTime = 0f;
    }

    public void AttackAble(Vector2 dir, int damage, bool isHeavy, int critical = 0)
    {
        col.enabled = true;
        render.enabled = true;
        isAttackEnable = true;
        attackDir = dir;
        attackDamage = damage;
        criticalDamage = critical;
        isHeavyAttack = isHeavy;
        isParriedAttack = false;
        anim.SetTrigger("attackTrigger");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (string.Equals(attackFrom, PlayManager.PLAYER_TAG) && isHeavyAttack && collision.CompareTag(PlayManager.ATTACK_TAG))
        {
            Attack enemy = collision.GetComponent<Attack>();
            Projectile misile = collision.GetComponent<Projectile>();
            if (null != enemy)
            {
                if (enemy.IsParryAllow)
                {
                    enemy.parried.Parried();
                    Debug.Log("�и� ����");
                    isParriedAttack = true;
                }
                else
                {
                    Debug.Log("�и�����");
                }
            }
            else if(null != misile)
            {
                if (misile.IsParryAllow)
                {
                    Vector2 dir = new Vector2(attackDir.x, attackDir.y);
                    misile.Parried(gameObject, dir ,attackDamage);
                    Debug.Log("�и� ����");
                }
                else
                {
                    Debug.Log("�и�����");
                }
            }
        }
        else if (!collision.CompareTag(attackFrom) && !isParriedAttack)
        {
            if (null != afterAttack)
            {
                afterAttack.AfterAttack(attackDir);
            }
            collision.GetComponent<IAttack>()?.Hit(attackDamage, attackDir, isHeavyAttack, criticalDamage);
        }
    }
}