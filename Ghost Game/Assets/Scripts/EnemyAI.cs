using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{
    public static event System.Action OnEnemyHasSpottedPlayer;

    [SerializeField] Transform pathHolder;
    [SerializeField] Light spotLight;

    [SerializeField] float speed = 5f;
    [SerializeField] float waitTime = .3f;
    [SerializeField] float turnSpeed = 90;
    [SerializeField] float timeToSpotPlayer = .5f;

    [SerializeField] float viewDistance;
    [SerializeField] LayerMask viewMask;
    [Header("Explosion")]
    public bool inRangeOfExp;
    [SerializeField] float radiusSpotExp;

    [SerializeField] Animator animator;

    float playerVisibleTimer;
    float viewAngle;
    Transform player;
    Color origSpotLightColor;

    Vector3[] waypoints;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        viewAngle = spotLight.spotAngle;
        origSpotLightColor = spotLight.color;

        waypoints = new Vector3[pathHolder.childCount];

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }

        StartCoroutine(FollowPath());
        
        GameManager.gm.BombExploded += OnBombExploded;
    }
    private void Update()
    {
        if (CanSeePlayer())
        {
            playerVisibleTimer += Time.deltaTime;
            spotLight.color = Color.red;
        }
        else
        {
            playerVisibleTimer -= Time.deltaTime;

            spotLight.color = origSpotLightColor;
        }
        playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
        spotLight.color = Color.Lerp(origSpotLightColor, Color.red, playerVisibleTimer / timeToSpotPlayer);
        if(playerVisibleTimer >= timeToSpotPlayer)
        {
            if(OnEnemyHasSpottedPlayer != null)
            {
                OnEnemyHasSpottedPlayer();

            }
        }

    }
    void OnBombExploded(object sender, EventArgs args)
    {
        Vector3 explosionPosition = ((Explosion)sender).transform.position;
        if(Vector3.Distance(transform.position, explosionPosition) >= radiusSpotExp) 
        {
            return;
        }

        StopAllCoroutines();
        StartCoroutine(GoToBomb(explosionPosition));
        transform.position = Vector3.MoveTowards(transform.position, explosionPosition, speed * Time.deltaTime);
        
        
    }
    IEnumerator FollowPath()
    {
        
        int targetWaypointIndex = 0;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];

        transform.LookAt(targetWaypoint);

        while(true)
        {

            animator.SetBool("isRunning", true);

            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
            TurnFace(targetWaypoint);
            if ((transform.position - targetWaypoint).magnitude < .1f)
            {

                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];


                animator.SetBool("isRunning", false);
                yield return new WaitForSeconds(waitTime);

            }
            yield return null;
            
           

        }
    }
    void TurnFace(Vector3 target)
    {
        Vector3 dirToLookTarget = (target - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
        rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);      
        transform.rotation = rotation;
    }
    IEnumerator GoToBomb(Vector3 lookTarget)
    {
        Vector3 origPosition = transform.position;

        lookTarget.y = transform.position.y;
      
        
        while ((transform.position - lookTarget).magnitude > 1)
        {
            TurnFace(lookTarget);
            transform.position = Vector3.MoveTowards(transform.position, lookTarget, speed * Time.deltaTime);
            yield return null;
        }
        animator.SetBool("isRunning", false);

        yield return new WaitForSeconds(1);

        lookTarget = origPosition;
        while ((transform.position - lookTarget).magnitude > 1)
        {
            animator.SetBool("isRunning", true);

            TurnFace(lookTarget);
            transform.position = Vector3.MoveTowards(transform.position, lookTarget, speed * Time.deltaTime);
            yield return null;
        }
        StartCoroutine(FollowPath());
    }
    bool CanSeePlayer()
    {
        if(Vector3.Distance(transform.position, player.position) < viewDistance && !FindObjectOfType<Invisibility>().invisible)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if(angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if(!Physics.Linecast(transform.position, player.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach(Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);

        Color color = Color.grey;
        color.a = .1f;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position,radiusSpotExp);
        
    }




}
