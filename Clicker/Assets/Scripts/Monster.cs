using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Stat stat;

    public int idx;

    Animator anim;

    public Define.State state = Define.State.Idle;

    float elapsedTIme = 0;

    private void OnEnable()
    {
        anim = GetComponentInChildren<Animator>();
        StatInit();
    }

    void StatInit()
    {
        Stat init = Resources.Load($"Prefabs/Monster/{GameData.stage}", typeof(ScriptableObject)) as Stat;
        this.stat.hp = init.hp;
        this.stat.maxHp = init.maxHp;
        this.stat.objectName = init.objectName;
        this.stat.dmg = init.dmg;
        this.stat.defense = init.defense;
        this.stat.criticaldmg = init.criticaldmg;
        this.stat.critical = init.critical;
        this.stat.atkTime = init.atkTime;
    }

    private void Update()
    {
        switch (state)
        {
            case Define.State.Idle:
                Idle();
                break;
            case Define.State.Move:
                break;
            case Define.State.Attack:
                Attack();
                break;
            case Define.State.Skill:
                break;
            case Define.State.Hit:
                break;
            case Define.State.Die:
                break;
            case Define.State.Win:
                break;
        }
    }

    void Idle()
    {
        elapsedTIme += Time.deltaTime;
        anim.SetBool("Move", false);

        if(elapsedTIme >= stat.atkTime)
        {
            elapsedTIme = 0;
            state = Define.State.Attack;
        }
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
        state = Define.State.Idle;
    }

    public void AttackFunc()
    {
        GameObject.FindObjectOfType<Player>().Hit(stat);
    }

    public void Hit(Stat stat)
    {
        int dmg = stat.dmg - this.stat.defense;

        if (dmg < 0)
            dmg = 1;

        this.stat.hp -= dmg;

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("goblin_idle"))
            anim.SetTrigger("Hit");

        DieCheck();
    }

    void Die()
    {
        anim.SetTrigger("Die");
        state = Define.State.Die;
        GameObject.FindObjectOfType<Player>().state = Define.State.Idle;
        Destroy(transform.parent.gameObject);

        //GameObject.FindObjectOfType<GameData>().PlayerSave(Define.IPADDRESS, (int)Define.Port.Data);
        Stage.MonsterSpawn(GameData.stage);

        //if (GameData.Stage && GameData.stage < GameData.maxStage)
        //{
        //    GameData.stage += 1;
        //}
    }

    void DieCheck()
    {
        if (stat.hp <= 0)
        {
            Die();
        }
        else
        {
            state = Define.State.Idle;
        }
    }
}
