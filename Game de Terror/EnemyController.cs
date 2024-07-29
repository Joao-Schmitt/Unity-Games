using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("IA Follow Navegation")]
    public float minDistanceToFollow = 1.2f;
    public float minDistanceToPoint = 1.0f;
    public float timeToStartNavegation = 3.0f;
    public float speedOfNavegation = 1.0f;
    public float speedOfFollow = 3.0f;

    [Header("IA Vision")]
    public Transform eyes;
    public float visionRadius = 10f;
    public float visionAngle = 45f;
    public float timeOfNavegation;
    public GameObject[] navegationsPoints;

    private float timerNav;
    private int pointIndex;
    private bool canSeePlayer = false;

    private Animator anim;
    private Transform target;
    private NavMeshAgent navMesh;


    void Start()
    {
        anim = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navegationsPoints = GameObject.FindGameObjectsWithTag("NavegationsPoint");
        pointIndex = GetRandomPointIndex();
    }

    void Update()
    {
        timerNav = Math.Clamp(timerNav -= Time.deltaTime, 0, timeOfNavegation);
        canSeePlayer = CanSeePlayer();

        if (canSeePlayer || timerNav > 0)
            Follow();
        else
            Navegation(); 
    }

    private bool CanSeePlayer()
    {
        if (target == null)
            return false;

        var directionToPlayer = (target.position - eyes.position).normalized;

        if (Vector3.Angle(eyes.forward, directionToPlayer) < visionAngle / 2f)
        {
            RaycastHit hit;
            if (Physics.Raycast(eyes.position, directionToPlayer, out hit, visionRadius))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    timerNav = timeOfNavegation;
                    return true;
                }
            }
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = canSeePlayer ? Color.blue : Color.red;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-visionAngle / 2f, transform.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(visionAngle / 2f, transform.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.DrawRay(eyes.position, leftRayDirection * visionRadius);
        Gizmos.DrawRay(eyes.position, rightRayDirection * visionRadius);
        Gizmos.DrawLine(eyes.position, eyes.position + leftRayDirection * visionRadius);
        Gizmos.DrawLine(eyes.position, eyes.position + rightRayDirection * visionRadius);
    }

    private void Navegation()
    {
        anim.SetBool("Follow", false);
        navMesh.speed = speedOfNavegation;

        if (navMesh.enabled && Vector3.Distance(transform.position, navegationsPoints[pointIndex].transform.position) > minDistanceToPoint)
        {
            anim.SetBool("Navegation", true);
            navMesh.SetDestination(navegationsPoints[pointIndex].transform.position);
        }
        else
        {
            navMesh.enabled = false;
            anim.SetBool("Navegation", false);

            pointIndex = GetRandomPointIndex();

            StartCoroutine("StartNavegation");
        }
    }

    private void Follow()
    {
        navMesh.speed = speedOfFollow;

        if (Vector3.Distance(transform.position, target.position) > minDistanceToFollow)
        {
            anim.SetBool("Follow", true);
            navMesh.enabled = true;
            navMesh.SetDestination(target.position);
        }
        else
        {
            var lookPos = new Vector3(target.position.x, 0, target.position.z);
            transform.LookAt(lookPos);

            navMesh.enabled = false;
            anim.SetBool("Follow", false);
        }
    }
    private int GetRandomPointIndex()
    {
        var i = UnityEngine.Random.Range(0, (navegationsPoints.Length - 1));

        if (pointIndex == i)
            return GetRandomPointIndex();

        var enemys = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemys)
        {
            if (enemy.GetComponent<EnemyController>().pointIndex == i)
                return GetRandomPointIndex();
        }

        return i;
    }

    private IEnumerator StartNavegation()
    {
        yield return new WaitForSeconds(timeToStartNavegation);
        navMesh.enabled = true;
    }
}
