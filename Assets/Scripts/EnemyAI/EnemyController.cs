using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CharacterController))]
public class EnemyController : MonoBehaviour
{
    private CharacterController controller;
    private PunchMechanism punchMechanism;

    private Vector3 targetPoint;
    private Transform targetTransform;
    private Vector3 moveVec;
    public float movementSpeed;
    private float movementLength = 7f;

    [HideInInspector] public bool canMove = false;



    private void Start()
    {
        SetupComponents();

        StartMove();
    }

    public void SetupStats(Stats stats)
    {
        movementSpeed = stats.movementSpeed;
    }

    private void StartMove()
    {
        GetNextTargetPoint();

        canMove = true;
    }

    private void Update()
    {
        if (canMove ) //&& !punchMechanism.isPunching
        {
            CheckTargetTransform();

            Move();
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }

    private void Move()
    {
        Vector3 moveDir = targetPoint - transform.position;

        moveVec = moveDir.normalized;

        controller.Move(moveVec * movementSpeed * Time.deltaTime);

        if (moveVec != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, moveVec, Time.deltaTime * 20f);
        }

        CheckTargetDistance();
    }

    private void CheckTargetTransform()
    {
        if (targetTransform != null)
        {
            targetPoint = targetTransform.position;
        }
    }

    private void CheckTargetDistance()
    {
        float distance = Vector3.Distance(transform.position, targetPoint);

        if (distance <= 0.1f)
        {
            GetNextTargetPoint();
        }
    }

    private void GetNextTargetPoint(int callCount = 0)
    {
        Vector2 randPoint = Random.insideUnitCircle.normalized * movementLength;

        Vector3 possibleTarget = transform.position + new Vector3(randPoint.x, transform.position.y, randPoint.y);

        bool inMap = IsPointInMap(possibleTarget);

        if (!inMap)
        {
            if (callCount < 15)
            {
                callCount++;
                GetNextTargetPoint(callCount);
            }
            return;
        }

        targetPoint = possibleTarget;
    }


    private bool IsPointInMap(Vector3 point)
    {
        if (point.x > -GameConfig.Instance.mapSize * 0.5f && point.x < GameConfig.Instance.mapSize * 0.5f && point.z > -GameConfig.Instance.mapSize * 0.5f && point.z < GameConfig.Instance.mapSize * 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void SetTarget(Transform target)
    {
        targetTransform = target;

        DOVirtual.DelayedCall(GetComponent<PunchMechanism>().punchCoolDown, () => { targetTransform = null; }); // reset target
    }

    private void SetupComponents()
    {
        controller = GetComponent<CharacterController>();

        punchMechanism = GetComponent<PunchMechanism>();
    }
}
