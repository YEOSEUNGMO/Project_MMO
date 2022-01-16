using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField]
    protected Define.State _state = Define.State.Idle;
    protected virtual Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;
            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case Define.State.Die:
                    break;
                case Define.State.Idle:
                    anim.CrossFade("WAIT", 0.1f);
                    break;
                case Define.State.Move:
                    anim.CrossFade("RUN", 0.1f);
                    break;
                case Define.State.Skill:
                    anim.CrossFade("ATTACK", 0.1f, -1, 0);
                    break;
            }
        }
    }
    // float _speed = 10f;
    [SerializeField] protected GameObject _lockTarget = null;
    [SerializeField] protected Vector3 _destPos;
    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;
    private void Start()
    {
        Init();
    }
    public abstract void Init();
    void Update()
    {
        switch (State)
        {
            case Define.State.Die:
                break;
            case Define.State.Move:
                UpdateMoving();
                break;
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Skill:
                UpdateSkill();
                break;
        }
    }
    protected virtual void UpdateMoving(){}
    protected virtual void UpdateIdle(){}
    protected virtual void UpdateSkill(){}
}
