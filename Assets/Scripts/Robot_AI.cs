using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Robot_AI : MonoBehaviour
{
    NavMeshAgent agent;
    public Vector3 Random_Destination;
    Animator anim;
    private float safeDistance = 35.0f;
    public Transform Player;
    private float distanceToPlayer;
    private const float MaxSpeed = 15f;
    private const float SpeedUp = 1f;
    private bool IsSpeedUp = false;
    private float wanderTimer = 6f; // Через сколько успокоится

    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        Random_Destination = RandomNavmeshLocation(50);
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if ( /*to check if the animal is already dead*/anim.GetBool("Death"))
        {
            return;
        }

        //Distance of player from the animal...
        distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        //anim.SetBool("Run", false);
        //anim.SetBool("Shoot", false);
        timer += Time.deltaTime;

        if (timer >= wanderTimer && distanceToPlayer > safeDistance)
        {
            //Agent.speed = Speed;
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            try
            {
                agent.SetDestination(newPos);
            } catch (Exception e)
            {
                Console.Write(e);
            }
               
            anim.SetBool("Run", false);
            anim.SetBool("Walk", true);
            agent.speed = 1.5f;
            timer = 0;
        }
        if (IsSpeedUp)
        {
            if (agent.speed < MaxSpeed)
            {
                agent.speed += SpeedUp;
            }
            else
            {
                IsSpeedUp = false;
            }
        }

        if(distanceToPlayer < safeDistance)
        {
            RunAway();
            IsSpeedUp = true;
        }
    }

    void RunAway()
    {
        anim.SetBool("Run", true);
        anim.SetBool("Walk", false);

        //Invoke("ResumeNormalActivity", 5.0f);
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {

        //Debug.Log("setiiing sedtination");
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * (radius);
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }


    #region Wander
    public float wanderRadius; // безсмысленная переменная
    private Transform target;
    private float timer;
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
    #endregion
}
