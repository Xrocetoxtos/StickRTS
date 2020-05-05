using UnityEngine;
using System.Collections;
using System;

public class Character : WorldObject
{
    private CharacterAnimator characterAnimator;
    [SerializeField] private GameObject characterSelectionVisual;
    [SerializeField] private Transform targetDummy;

    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold=.5f;
    
    public Transform target;
    public float speed = 20f;
    public float turnDistance = 5f;
    public float turnSpeed = 3f;
    public float stoppingDistance = 10f;

    public ResourceType hasResourceType;
    public int hasResourceAmount;
    public int maxResourceAmount = 10;

    Path path;

    protected override void Awake()
    {
        base.Awake();

        characterAnimator = GetComponent<CharacterAnimator>();
        if (player != null)
            player.characters.Add(this);

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
            path = new Path(wayPoints, transform.position,turnDistance,stoppingDistance);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator UpdatePath()
    {
        if(Time.timeSinceLevelLoad<.3f)
        {
            yield return new WaitForSeconds(.3f);

        }
        if (target == null)
            target = transform;

        PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);

            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
                targetPosOld = target.position;
            }
        }
    }

    public void MoveToPosition(Vector2 position)
    {
        targetDummy.position = position;
        target = targetDummy;
    }

    IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        //normalized richting bepalen ivm niet kunnen draaien in 2D
        Vector2 direction = (path.lookPoints[0]- transform.position).normalized;
        Vector2 destination = path.lookPoints[0];
        AnimateDirection(direction);

        float speedPercent = 1f;
      
        while (followingPath)
        {
            if (followingPath)
            {
                if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), destination) < stoppingDistance)
                {
                    if (destination == new Vector2(target.position.x, target.position.y))
                    {
                        followingPath = false;
                        AnimateSpecific(CharacterAnimationState.Idle);
                        yield return null;                             
                    }

                    pathIndex++;
                    if (pathIndex >= path.lookPoints.Length)
                        destination = target.position;
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
    }

    private void AnimateDirection(Vector2 direction)
    {
        if (characterAnimator)
            characterAnimator.GetAnimationFromVector2(direction.x, direction.y);
    }

    private void AnimateSpecific(CharacterAnimationState stickFigureAnimation)
    {
        if (characterAnimator)
            characterAnimator.ChangeAnimation(stickFigureAnimation);
    }



}