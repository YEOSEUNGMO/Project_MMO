using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField] protected int _level;
    public int Level { get { return _level; } set { _level = value; } }
    [SerializeField] protected int _hp;
    public int Hp { get { return _hp; } set { _hp = value; } }
    [SerializeField] protected int _maxHp;
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    [SerializeField] protected int _attack;
    public int Attack { get { return _attack; } set { _attack = value; } }
    [SerializeField] protected int _defense;
    public int Defense { get { return _defense; } set { _defense = value; } }
    [SerializeField] protected float _moveSpeed;
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    private void Start()
    {
        _level = 1;
        _hp = 100;
        _maxHp = 100;
        _attack = 10;
        _defense = 5;
        _moveSpeed = 5.0f;
    }

    public virtual void OnAttacked(Stat attacker)
    {
        int damage = Mathf.Max(0, attacker.Attack - Defense);
        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            OnDead(attacker);
        }
    }
    protected virtual void OnDead(Stat attacker)
    {
        PlayerStat playerStat = attacker as PlayerStat;
        if (playerStat != null)
        {
            playerStat.Exp += 100;
        }
        Managers.Game.Despawn(gameObject);
    }
}
