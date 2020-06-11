using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MovementController : MonoBehaviour
{
    public AIController ac;
    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .1f;

    public float speed = 20f;
    public float stoppingDistance = 10f;

    Path path;

    private void Awake()
    {
        ac = GetComponent<AIController>();
        //is nodig om te verplaatsen naar een transform;
        if (ac.targetDummy != null)
            ac.targetDummy.parent = null;
    }

    private void Start()
    {
        StartCoroutine(UpdatePath());
    }

    public void OnPathFound(Vector3[] wayPoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = new Path(wayPoints, transform.position, stoppingDistance);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);

        }
        if (ac.targetPosition == null)
            ac.targetPosition = transform;

        PathRequestManager.RequestPath(new PathRequest(transform.position, ac.targetPosition.position, OnPathFound));

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = ac.targetPosition.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);

            if ((ac.targetPosition.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, ac.targetPosition.position, OnPathFound));
                targetPosOld = ac.targetPosition.position;
            }
        }
    }

    public void MoveToPosition(Vector2 position)
    {
        ac.targetDummy.position = position;
        ac.targetPosition = ac.targetDummy;
    }

    IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        //normalized richting bepalen ivm niet kunnen draaien in 2D
        Vector2 direction = (path.lookPoints[0] - transform.position).normalized;
        Vector2 destination = path.lookPoints[0];
        ac.AnimateDirection(direction, CharacterAnimationState.Walk);

        float speedPercent = 1f;

        while (followingPath)
        {
            if (followingPath)
            {
                if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), destination) < stoppingDistance)
                {
                    if (destination == new Vector2(ac.targetPosition.position.x, ac.targetPosition.position.y))
                    {
                        followingPath = false;
                        ac.animator.SetBool("HasArrived", true);
                        ac.AnimateSpecific(CharacterAnimationState.Idle);
                        yield return null;
                    }

                    pathIndex++;
                    if (pathIndex >= path.lookPoints.Length)
                        destination = ac.targetPosition.position;
                    else
                        destination = path.lookPoints[pathIndex];

                    direction = BigBookBasic.GetDirectionVector2(destination,transform.position);
                    ac.AnimateDirection(direction, CharacterAnimationState.Walk);
                }
                transform.Translate(direction * Time.deltaTime * speed * speedPercent);
            }
            yield return null;
        }
        ac.AnimateSpecific(CharacterAnimationState.Idle);
        ac.animator.SetBool("HasArrived", true);
    }

    // ====================== MOVE ==============================================================

    public void MoveToGround(Vector2 position)
    {
        MoveToPosition(position);
        ac.actualTarget = null;
        ac.returnTarget = null;
        ac.animator.SetBool("HasArrived", false);
    }

    public void MoveToWorldObject(WorldObject worldObject, Vector2 slotLocation, bool _actualTargetThis = false, bool _returnTargetNull = false)
    {
        MoveToPosition(slotLocation);
        ac.actualTarget = _actualTargetThis ? worldObject : null;
        if (_returnTargetNull)
            ac.returnTarget = null;
        ac.animator.SetBool("HasArrived", false);
    }
}