using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Stat stat;

    float elapsedTime = 0;

    [SerializeField]
    public Define.State state;
    Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        GameObject.FindObjectOfType<GameData>().PlayerInit(Define.IPADDRESS, (int)Define.Port.Data);
    }

    private void Update()
    {
        switch (state)
        {
            case Define.State.Idle:
                Idle();
                break;
            case Define.State.Move:
                anim.SetBool("Move", true);
                break;
            case Define.State.Attack:
                Attack();
                break;
            case Define.State.Skill:
                anim.SetBool("Move", false);
                anim.SetTrigger("Skill");
                state = Define.State.Idle;
                break;
            case Define.State.Hit:
                break;
            case Define.State.Die:
                anim.SetTrigger("Die");
                break;
            case Define.State.Win:
                anim.SetTrigger("Win");
                break;
        }
    }

    void Idle()
    {
        anim.SetBool("Move", false);

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= stat.atkTime)
        {
            if (GameObject.FindObjectOfType<Monster>() == null)
                return;
            if (GameObject.FindObjectOfType<Monster>().state == Define.State.Die)
                return;
            elapsedTime = 0;
            state = Define.State.Attack;
        }
    }

    void Attack()
    {
        anim.SetBool("Move", false);
        anim.SetTrigger("Attack");
        state = Define.State.Idle;
    }

    public void Hit(Stat stat)
    {
        anim.SetTrigger("Hit");

        int dmg = stat.dmg - this.stat.defense;

        this.stat.hp -= dmg;

        DieCheck();
    }

    void DieCheck()
    {
        if (stat.hp <= 0)
        {
            stat.hp = stat.maxHp;
        }
    }

    public void AttackFunc()
    {
        Managers.Sound.SoundPlayer(SoundManager.AudioType.SFX, "Click");

        GameObject.FindObjectOfType<Monster>().Hit(stat);
    }
}
