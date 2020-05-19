using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Character : WorldObject
{
    private CharacterAnimator characterAnimator;
    [SerializeField] private GameObject characterSelectionVisual;
    [SerializeField] private Transform targetDummy;

    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;

    public Transform targetPosition;
    public WorldObject actualTarget;
    public WorldObject returnTarget;
    public Animator animator;

    public float speed = 20f;
    public float turnDistance = 5f;
    public float stoppingDistance = 10f;
    public float viewRange = 5;

    public List<ResourceType> canExtractResourceType = new List<ResourceType>();

    public ResourceType hasResourceType;
    public float hasResourceAmount;
    public float maxResourceAmount = 10;
    public float resourceExtractSpeed = 1;

    Path path;

    protected override void Awake()
    {
        base.Awake();
        worldObjectType = ObjectType.Character;

        characterAnimator = GetComponent<CharacterAnimator>();
        if (player != null)
            player.characters.Add(this);

        animator = GetComponent<Animator>();
        animator.SetBool("HasArrived", true);

        //is nodig om te verplaatsen naar een transform;
        if (targetDummy != null)
            targetDummy.parent = null;
    }

    void Start()
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
        if (targetPosition == null)
            targetPosition = transform;

        PathRequestManager.RequestPath(new PathRequest(transform.position, targetPosition.position, OnPathFound));

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = targetPosition.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);

            if ((targetPosition.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, targetPosition.position, OnPathFound));
                targetPosOld = targetPosition.position;
            }
        }
    }

    public void MoveToPosition(Vector2 position)
    {
        targetDummy.position = position;
        targetPosition = targetDummy;
    }

    IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        //normalized richting bepalen ivm niet kunnen draaien in 2D
        Vector2 direction = (path.lookPoints[0] - transform.position).normalized;
        Vector2 destination = path.lookPoints[0];
        AnimateDirection(direction);

        float speedPercent = 1f;

        while (followingPath)
        {
            if (followingPath)
            {
                if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), destination) < stoppingDistance)
                {
                    if (destination == new Vector2(targetPosition.position.x, targetPosition.position.y))
                    {
                        followingPath = false;
                        animator.SetBool("HasArrived", true);
                        AnimateSpecific(CharacterAnimationState.Idle);
                        yield return null;
                    }

                    pathIndex++;
                    if (pathIndex >= path.lookPoints.Length)
                        destination = targetPosition.position;
                    else
                        destination = path.lookPoints[pathIndex];

                    direction = (destination - new Vector2(transform.position.x, transform.position.y)).normalized;
                    AnimateDirection(direction);
                }
                transform.Translate(direction * Time.deltaTime * speed * speedPercent);
            }
            yield return null;
        }
        AnimateSpecific(CharacterAnimationState.Idle);
        animator.SetBool("HasArrived", true);
    }

    // ====================== MOVE ==============================================================

    public void MoveToGround(Vector2 position)
    {
        MoveToPosition(position);
        actualTarget = null;
        returnTarget = null;
        animator.SetBool("HasArrived", false);
    }

    public void MoveToWorldObject(WorldObject worldObject, Vector2 slotLocation, bool _actualTargetThis = false, bool _returnTargetNull = false)
    {
        MoveToPosition(slotLocation);
        actualTarget = _actualTargetThis ? worldObject : null;
        if (_returnTargetNull)
            returnTarget = null;
        animator.SetBool("HasArrived", false);
    }

    public void FindStorage()
    {
        //een storage zoeken om de resources heen te sturen. kunnen álle storages van deze player zijn.
        returnTarget = actualTarget;
        List<Building> buildings = player.GetBuildingsForResourceStorage(hasResourceType);
        if(buildings.Count==0)
        {
            Debug.Log(worldObjectName + " kan geen storage vinden voor " + hasResourceType);
        }
        else
        {
            actualTarget = BigBookBasic.GetNearestWorldObjectInList(transform.position, buildings);
            Character[] characterArray = new Character[1];
            characterArray[0] = this;
            player.playerController.MoveToObject(actualTarget.gameObject, characterArray);
        }
    }

    public void FindNewResource()
    {
        //als een resource leeg is en character heeft geen resources, dan zoeken naar een nieuwe. moet binnen viewRange.
        returnTarget = null;
        actualTarget = BigBookBasic.GetNearestResourceInRange(transform.position, viewRange, hasResourceType);
        if (actualTarget != null)
        {
            Character[] characterArray = new Character[1];
            characterArray[0] = this;
            player.playerController.MoveToObject(actualTarget.gameObject, characterArray);
        }
        else
        {
            animator.SetBool("HasResources", false);
            animator.SetBool("IsGathering", false);
        }
    }

    // ===========================================================================================

    private void AnimateDirection(Vector2 direction)
    {
        if (characterAnimator)
            characterAnimator.GetAnimationFromVector2(direction.x, direction.y);
    }

    public void AnimateSpecific(CharacterAnimationState stickFigureAnimation)
    {
        if (characterAnimator)
            characterAnimator.ChangeAnimation(stickFigureAnimation);
    }



}