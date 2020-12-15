using System;
using System.Collections.Generic;
using UnityEngine;
using unciphering.Mechanics;
using unciphering.Characters;

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

        private List<ParticleCollisionEvent> collisionEvents;

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
            gameObject.transform.position = patrolPath.transform.position;
        }

        private void OnDrawGizmos() 
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

        void Update()
        {
            target = mainPlayer.gameObject.transform.position;
            if (InAttackRangeOfPlayer())
            {
                AttackBehaviour();
            }
            else if (InFollowRangeOfPlayer() || GetComponent<Enemy>().isProvoked)
            {
                FollowBehavior();
                AlertNearByAllies();
            }
            else if (!InFollowRangeOfPlayer() && isSuspicious)
            {
                SuspiciousBehavior();
            }
            else 
            {
                PatrolBehaviour();
            }
            
            timeSinceLastArrivedAtWaypoint += Time.deltaTime;
            timeSinceLastSeenPlayer += Time.deltaTime;
        }

         private void PatrolBehaviour()
        {
            if (patrolPath == null) return;
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
            engine.ProcessLook(target);
            gun.Fire();
        }

        private void FollowBehavior()
        {
            if (gun == null && engine == null) {print("No gun or Engine Found"); return;}
            //gun.StopFire();
            engine.ProcessLook(target);
            engine.BasicMouvement(attackSpeed);
            lastSeenTarget = target;
            isSuspicious = true;
        }

        private void SuspiciousBehavior()
        {
            if(Vector3.Distance(transform.position, lastSeenTarget) > 5)
            {
                transform.position = Vector3.MoveTowards(transform.position, lastSeenTarget, attackSpeed * Time.deltaTime);
                timeSinceLastSeenPlayer = 0;
            }
            else
            {
                if (timeSinceLastSeenPlayer > suspicionTime)
                {
                    isSuspicious = false;
                    GetComponent<Enemy>().isProvoked = false;
                }
            }
        }

        private bool InAttackRangeOfPlayer()
        {
            float DistanceToPlayer = Vector3.Distance(target, transform.position);

            if(DistanceToPlayer < attackDistance)
            {
                if (InLineOfSight())
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            else
            { 
                return false; 
            }
        }

        private bool InFollowRangeOfPlayer()
        {
            float DistanceToPlayer = Vector3.Distance(target, transform.position);
            if(DistanceToPlayer < chaseDistance && InLineOfSight())
            {
                return true;
            }
            else
            { 
                return false;
            }
        }


        private bool InLineOfSight()
        {
            Debug.Log("here");
            float angleToTarget = Vector3.Angle(transform.forward, transform.position - target);
            RaycastHit hit;
            if (Mathf.Abs(angleToTarget) > 90 && Mathf.Abs(angleToTarget) < 270)
            {
                if (Physics.Raycast(transform.position, target - transform.position, out hit, 1000))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    if (hit.transform.GetComponent<ShipController>())
                    {
                        GetComponent<Enemy>().isProvoked = true;
                        return true;
                    }
                    else {GetComponent<Enemy>().isProvoked = false; return false;}
                }
                else return false;
            }
            else 
            {
                return false;
            }
        }

        private void AlertNearByAllies()
        {
            Int32 layerMask = 1 << 11;
            RaycastHit[] targetedEnemies;
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, 
            50, transform.TransformDirection(Vector3.forward), 1, layerMask);
            targetedEnemies = Array.FindAll(hits, e => e.collider.gameObject.GetComponent<Enemy>());
            foreach (var e in targetedEnemies)
            {
                e.collider.gameObject.GetComponent<Enemy>().isProvoked = true;
            }
        }

        private void OnParticleCollision(GameObject other) 
        {
            collisionEvents = new List<ParticleCollisionEvent>();
            GetComponent<ParticleSystem>().GetCollisionEvents(other, collisionEvents);
            for (int i = 0; i < collisionEvents.Count; i++)
            {
                if (mainPlayer)
                {   
                    mainPlayer.OnDeath();
                }
            }  
        }
    }
}
