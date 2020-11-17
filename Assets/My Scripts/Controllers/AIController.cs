using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace unciphering.Controller
{
    public class AIController : MonoBehaviour
    {
        // General
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] ShipController mainPlayer;
        Engine engine;
        EnemyGun gun;
        Vector3 target;
        Vector3 lastSeenTarget;
        bool isSuspicious;

        // Patrol Params
        int currentWaypointIndex = 0;
        float timeSinceLastArrivedAtWaypoint;
        float timeSinceLastSeenPlayer;
        float waypointTolerance = 10;

        
        [Header("Patrol Settings")]
        [SerializeField][Tooltip("in sec")] [Range(0,5)] float dwellTime = 0.5f;
        [SerializeField][Tooltip("in sec")] [Range(0,5)] float suspicionTime = 2;
        [SerializeField][Tooltip("in m/s")] [Range(0,50)]float patrolSpeed = 20;
        

        // Attack Params
        [Header("Attack Settings")]
        [SerializeField][Tooltip("in m")] float chaseDistance = 60;
        [SerializeField][Tooltip("in m")] float attackDistance = 30;
        [SerializeField][Tooltip("in m/s")] [Range(0,50)] float attackSpeed = 30;
        
        void Start()
        {
            gun = GetComponent<EnemyGun>();
            target = mainPlayer.gameObject.transform.position;
            engine = GetComponent<Engine>();
        }

        private void OnDrawGizmos() 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
        // Update is called once per frame
        void Update()
        {
            
            AttackBehaviour();

            target = mainPlayer.gameObject.transform.position;
            timeSinceLastArrivedAtWaypoint += Time.deltaTime;
            timeSinceLastSeenPlayer += Time.deltaTime;
        }

         private void PatrolBehaviour()
        {
            Vector3 nextPosition = patrolPath.GetWaypoint(currentWaypointIndex);
            if (AtWaypoint())
            {
                timeSinceLastArrivedAtWaypoint = 0f;
                CycleWayPoint(); 
            }
            else if (timeSinceLastArrivedAtWaypoint < dwellTime)
            {
                return;
            }
            else 
            { 
                nextPosition = GetCurrentWaypoint();
                engine.ProcessLook(nextPosition);
                engine.BasicMouvement(patrolSpeed);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            if (distanceToWaypoint < waypointTolerance)
            {
                return true;
            }
            else {return false;}
        }
        private void CycleWayPoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }
        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }


        private void AttackBehaviour()
        {
            if (InAttackRangeOfPlayer())
            {
                engine.ProcessLook(target);
                //gun.Fire();
            }
            else if (InFollowRangeOfPlayer())
            {
                //gun.StopFire();
                engine.ProcessLook(target); 
                engine.BasicMouvement(attackSpeed);
                lastSeenTarget = target;
                isSuspicious = true;
            }
            else if (isSuspicious)
            {
                SuspiciousBehavior();
            }
            else
            {
                PatrolBehaviour();
            }
        }

        private void SuspiciousBehavior()
        {
            if(Vector3.Distance(transform.position, lastSeenTarget) > 5)
            {
                Debug.Log(gameObject.name + "Is MOVING TO that last seen");
                transform.position = Vector3.MoveTowards(transform.position, lastSeenTarget, attackSpeed * Time.deltaTime);
                timeSinceLastSeenPlayer = 0;
            }
            else
            {
                Debug.Log(Vector3.Distance(transform.position, lastSeenTarget));
                Debug.Log(gameObject.name + "Is At that last seen");
                if (timeSinceLastSeenPlayer > suspicionTime)
                {
                    isSuspicious = false;
                }
                
            }
            
        }

        private bool InAttackRangeOfPlayer()
        {
            float DistanceToPlayer = Vector3.Distance(target, transform.position);
            if(DistanceToPlayer < attackDistance)
            {
                return true;
            }
            else
            { 
                return false; 
            }
        }

        private bool InFollowRangeOfPlayer()
        {
            float DistanceToPlayer = Vector3.Distance(target, transform.position);
            if(DistanceToPlayer < chaseDistance)
            {
                return true;
            }
            else
            { 
                return false; 
            }
        }

    }
}
