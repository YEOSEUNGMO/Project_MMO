using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Die,
        Idle,
        Move,
        Skill,
    }
    [SerializeField]
    PlayerState _state = PlayerState.Idle;
    public PlayerState State
    {
        get { return _state; }
        set
        {
            _state = value;
            switch (_state)
            {
                case PlayerState.Die:
                    break;
                case PlayerState.Idle:
                    anim.CrossFade("WAIT", 0.1f);
                    break;
                case PlayerState.Move:
                    anim.CrossFade("RUN", 0.1f);
                    break;
                case PlayerState.Skill:
                    anim.CrossFade("ATTACK", 0.1f, -1, 0);
                    break;
            }
        }
    }
    // float _speed = 10f;

    [SerializeField] Vector3 _destPos;
    Animator anim = null;

    PlayerStat _stat;

    void Start()
    {
        _stat = gameObject.GetOrAddComponent<PlayerStat>();
        anim = GetComponent<Animator>();
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    public void UpdateMoving()
    {
        // 몬스터가 내 사정거리내에 들어 왔을때 공격.
        if (_lockTarget != null)
        {
            float dist = (_destPos - transform.position).magnitude;
            if (dist <= 1)
            {
                State = PlayerState.Skill;
                return;
            }
        }

        //이동
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.1f)
        {
            State = PlayerState.Idle;
        }
        else
        {
            float moveDist = Mathf.Clamp(Time.deltaTime * _stat.MoveSpeed, 0, dir.magnitude);
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.blue);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if (Input.GetMouseButton(0) == false)
                    State = PlayerState.Idle;
                return;
            }

            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            nma.Move(dir.normalized * moveDist);


            // transform.position += (dir.normalized * moveDist);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }

    public void UpdateIdle()
    {

    }

    void UpdateSkill()
    {
        if (_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    void OnHItEvent()
    {
        if(_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            int damage = Mathf.Max(0,_stat.Attack-targetStat.Defense);
            Debug.Log($"Damage : {damage}");
        }
        if (_stopSkill)
        {
            State = PlayerState.Idle;
        }
        else
        {
            State = PlayerState.Skill;
        }
    }

    void Update()
    {
        switch (State)
        {
            case PlayerState.Die:
                break;
            case PlayerState.Move:
                UpdateMoving();
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Skill:
                UpdateSkill();
                break;
        }
    }

    public void OnKeyboard()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            transform.position += (Vector3.forward * Time.deltaTime * _stat.MoveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            transform.position += (Vector3.back * Time.deltaTime * _stat.MoveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            transform.position += (Vector3.left * Time.deltaTime * _stat.MoveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            transform.position += (Vector3.right * Time.deltaTime * _stat.MoveSpeed);
        }
    }

    int clickLayer = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    GameObject _lockTarget = null;

    bool _stopSkill = false;
    public void OnMouseEvent(Define.MouseEvent mouseEvent)
    {
        switch (State)
        {
            case PlayerState.Idle:
                OnMouseEvent_IdleRun(mouseEvent);
                break;
            case PlayerState.Move:
                OnMouseEvent_IdleRun(mouseEvent);
                break;
            case PlayerState.Skill:
                if (mouseEvent == Define.MouseEvent.PointerUp)
                {
                    _stopSkill = true;
                }
                break;
            case PlayerState.Die:
                break;
        }
    }

    void OnMouseEvent_IdleRun(Define.MouseEvent mouseEvent)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, clickLayer);

        switch (mouseEvent)
        {
            case Define.MouseEvent.PointerDown:
                if (raycastHit)
                {
                    _destPos = hit.point;
                    _destPos.y = transform.position.y;
                    State = PlayerState.Move;
                    _stopSkill = false;

                    if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                        _lockTarget = hit.collider.gameObject;
                    else
                        _lockTarget = null;
                }
                break;
            case Define.MouseEvent.PointerUp:
                _stopSkill = true;
                break;
            case Define.MouseEvent.Click:
                break;
            case Define.MouseEvent.Press:
                if (_lockTarget == null && raycastHit)
                {
                    _destPos = hit.point;
                    _destPos.y = transform.position.y;
                }
                break;
        }

    }
}
