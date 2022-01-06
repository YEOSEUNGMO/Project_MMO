using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    enum PlayerState
    {
        Die,
        Idle,
        Move,
        Skill,
    }
    PlayerState _state = PlayerState.Idle;
    // float _speed = 10f;

    Vector3 _destPos;
    Animator anim = null;

    PlayerStat _stat;
    void Start()
    {
        _stat = gameObject.GetOrAddComponent<PlayerStat>();
        anim = GetComponent<Animator>();
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    public void UpdateMoving()
    {
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.1f)
        {
            _state = PlayerState.Idle;
        }
        else
        {
            float moveDist = Mathf.Clamp(Time.deltaTime * _stat.MoveSpeed, 0, dir.magnitude);
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.blue);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                _state = PlayerState.Idle;
                return;
            }

            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            nma.Move(dir.normalized * moveDist);


            // transform.position += (dir.normalized * moveDist);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }

        //animation
        anim.SetFloat("speed", _stat.MoveSpeed);
    }

    public void UpdateIdle()
    {
        anim.SetFloat("speed", 0);
    }

    void Update()
    {
        switch (_state)
        {
            case PlayerState.Die:
                break;
            case PlayerState.Move:
                UpdateMoving();
                break;
            case PlayerState.Idle:
                UpdateIdle();
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

    int clickLayer =(1<<(int)Define.Layer.Ground) | (1<<(int)Define.Layer.Monster);
    public void OnMouseClicked(Define.MouseEvent mouseEvent)
    {
        if (_state == PlayerState.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, clickLayer))
        {
            _destPos = hit.point;
            _state = PlayerState.Move;

            if(hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                Debug.Log("Click Monster!!");
            }
            else
            {
                Debug.Log("Click Ground");
            }
        }
    }
}
