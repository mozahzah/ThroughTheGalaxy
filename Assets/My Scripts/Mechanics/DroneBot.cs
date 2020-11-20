using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using unciphering.Characters;

namespace unciphering.Mechanics
{

    

    public class DroneBot : MonoBehaviour
    {
        // General Params
        public Camera cam;
        RaycastHit[] targetedEnemies;
        [SerializeField] float scanRadius = 500;
        Int32 layerMask;

        void Start()
        {
            layerMask = 1 << 11;
        }

        private void RayCastForTracking()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, scanRadius, transform.TransformDirection(Vector3.forward), 1, layerMask);
            targetedEnemies = Array.FindAll(hits, e => e.collider.gameObject.GetComponent<Enemy>());
        }

        public void ScanEnemies()
        {
            RayCastForTracking();
            foreach (var e in targetedEnemies)
            {
                e.collider.gameObject.GetComponent<Enemy>().TagEnemy();
            }
        }

        public void TagEnemy()
        {
            Vector3 crossairLocation = cam.
            ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;
            if (Physics.Linecast(crossairLocation, cam.transform.TransformDirection(Vector3.forward) * 10000, out hit, layerMask))
            {
                if (hit.collider.gameObject.GetComponent<Enemy>())
                {
                    hit.collider.gameObject.GetComponent<Enemy>().TagEnemy();
                }
            }
        }
    }
}
