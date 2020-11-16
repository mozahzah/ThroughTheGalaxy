using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace unciphering.Controller
{
    public class AIController : MonoBehaviour
    {
        // General
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] Engine engine;
        [SerializeField] EnemyGun gun;
        [SerializeField] ShipController mainPlayer;
        Vector3 target;

        // Patrol Params
        int currentWaypointIndex = 0;
        float dwellTime = 0.5f;
        float timeSinceLastArrivedAtWaypoint;
        float suspicionTime = 2;
        float waypointTolerance = 10;
        // Attack Params
        float chaseDistance = 30;
        float attackDistance = 30;
        
        void Start()
        {
            gun = GetComponent<EnemyGun>();
            target = mainPlayer.gameObject.transform.position;
            engine = GetComponent<Engine>();
        }

        // Update is called once per frame
        void Update()
        {
            AttackBehaviour();

            target = mainPlayer.gameObject.transform.position;
            timeSinceLastArrivedAtWaypoint += Time.deltaTime;
            
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
                transform.Translate(Vector3.forward * 10 * Time.deltaTime);
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
                gun.Fire();
            }
            else if (InFollowRangeOfPlayer())
            {
                gun.StopFire();
                engine.ProcessLook(target); 
                transform.Translate(Vector3.forward * 20 * Time.deltaTime);
            }
            
            else 
            {
                gun.StopFire();
                PatrolBehaviour();
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
