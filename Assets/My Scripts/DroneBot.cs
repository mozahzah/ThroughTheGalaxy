using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ThroughtTheGalaxy.Characters;

namespace ThroughtTheGalaxy.Mechanics
{
    public class DroneBot : MonoBehaviour
    {
        [SerializeField] Camera MainCamera;
        [SerializeField] Camera DroneCamera;


        RaycastHit[] targetedEnemies;

        // Start is called before the first frame update
        void Start()
        {
            MainCamera = Camera.main;
            MainCamera.enabled = false;
            DroneCamera.enabled = true;
        }

        
        void Update()
        {
            
        }

        private void RayCastForTracking()
        {
                RaycastHit[] hits = Physics.SphereCastAll(transform.position, 50, transform.TransformDirection(Vector3.forward), Mathf.Infinity);
                targetedEnemies = Array.FindAll(hits, e => e.collider.gameObject.GetComponent<Enemy>());

                // Array Sorting by Distance
                for (int i = 0; i < targetedEnemies.Length - 1; i++)
                {
                    for (int j = i + 1; j < targetedEnemies.Length; j++)
                    {
                        if (Vector3.Distance(targetedEnemies[i].collider.transform.position,
                            Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)) +
                            Camera.main.transform.forward * Vector3.Distance(targetedEnemies[i].collider.transform.position, Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)))) >

                            Vector3.Distance(targetedEnemies[j].collider.transform.position,
                            Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)) +
                            Camera.main.transform.forward * Vector3.Distance(targetedEnemies[i].collider.transform.position, Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)))))
                        {
                            var temp = targetedEnemies[i];
                            targetedEnemies[i] = targetedEnemies[j];
                            targetedEnemies[j] = temp;
                        }
                    }
                }
                TurnOffTargeting(); 
        }

        private void TurnOffTargeting()
        {
            foreach (var e in targetedEnemies)
            {
                e.collider.gameObject.GetComponent<Enemy>().isSelected = false;
            }
                
        }   
    }
}
