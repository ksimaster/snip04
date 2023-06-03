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
    GunHanddle gun;
    private float safeDistance = 35.0f; // ƒо куда можно подойти и животное не заметит
    private float safeDistanceRun = 50.0f; // ƒо куда можно бежать и животное не заметит
    private float safeDistanceShoot = 80.0f; // C какого рассто€ни€ можно стрел€ть 150
    private float safeDistanceDewarn = 50.0f; // ѕробежав сколько животное успокоитс€
    public Transform Player;
    private const float MaxSpeed = 15f;
    private const float SpeedUp = 1f;
    private bool IsSpeedUp = false;
    private bool IsWarned = false;
    private FPSController FPSmotor;

    private float uploadTimer = 0f; // 
    private float navTimer = 0f; // 
    private float wanderTimer = 6f; // „ерез сколько успокоитс€
    private float wanderTimerMax = 20f; // „ерез сколько успокоитс€ точно

    private Vector3 wanderPosition; // √де животное спугнули

    private float checkTimer = 0.5f;
    
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        Random_Destination = RandomNavmeshLocation(50);
        anim = GetComponent<Animator>();
        gun = Player.GetComponentInChildren<GunHanddle>();
        FPSmotor = Player.GetComponentInChildren<FPSController>();
    }

    void Update()
    {
        var deltaTime = Time.deltaTime;
        uploadTimer += deltaTime;

        if (uploadTimer < checkTimer)
        {
            return;
        }

        uploadTimer = 0;
        
        if ( /*to check if the animal is already dead*/anim.GetBool("Death"))
        {
            return;
        }

        //Distance of player from the animal...
        var distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        timer += deltaTime;
        navTimer += deltaTime;

        if (navTimer > wanderTimer)
        {
            //Agent.speed = Speed;
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            try
            {
                agent.SetDestination(newPos);
            }
            catch (Exception e)
            {
                Console.Write(e);
            }

            navTimer = 0;
        }

        if (!IsWarned)
        {
            if (distanceToPlayer < safeDistanceShoot && gun.IsShoot())
            {
                RunAway();
            }

            else if (distanceToPlayer < safeDistance)
            {
                RunAway();
            }
            
            else if (distanceToPlayer < safeDistanceRun && FPSmotor.motor != null && FPSmotor.motor.boostMults > 1)
            {
                RunAway();
            }
        }
        

        if (IsWarned && timer >= wanderTimer && distanceToPlayer > safeDistance)
        {
            if (wanderPosition != null)
            {
                var distanceToWanderPlace = Vector3.Distance(transform.position, wanderPosition);
                if (distanceToWanderPlace > safeDistanceDewarn || timer >= wanderTimerMax)
                {
                    IsWarned = false;
                }
            }
            else
            {
                IsWarned = false;
            }
        }

        if (!IsWarned)
        {               
            anim.SetBool("Run", false);
            anim.SetBool("Walk", true);
            agent.speed = 1.5f;
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

    }

    void RunAway()
    {
        IsWarned = true;
        IsSpeedUp = true;
        anim.SetBool("Run", true);
        anim.SetBool("Walk", false);
        wanderPosition = transform.position + new Vector3(0,0,0);
        timer = 0;
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
    public float wanderRadius; //–ассто€ние, на которое животное планирует 
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
